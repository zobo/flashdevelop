﻿using System;
using System.Collections.Generic;
using System.Text;
using FlashDebugger.Debugger.HxCpp.Server;
using PluginCore.Managers;
using System.Net.Sockets;
using FlashDebugger.Controls;
using PluginCore.Localization;

namespace FlashDebugger.Debugger.HxCpp
{
	class HxCppInterface : DebuggerInterface
	{
		public event TraceEventHandler TraceEvent;

		public event DebuggerEventHandler DisconnectedEvent;

		public event DebuggerEventHandler PauseFailedEvent;

		public event DebuggerEventHandler StartedEvent;

		public event DebuggerEventHandler BreakpointEvent;

		public event DebuggerEventHandler FaultEvent;

		public event DebuggerEventHandler PauseEvent;

		public event DebuggerEventHandler StepEvent;

		public event DebuggerEventHandler ScriptLoadedEvent;

		public event DebuggerEventHandler WatchpointEvent;

		public event DebuggerEventHandler UnknownHaltEvent;

		public event DebuggerProgressEventHandler ProgressEvent;

		public event DebuggerEventHandler ThreadsEvent;

		Manager manager;
		Session session;
		bool initialThreadStop;
		int activeThread;
		Dictionary<int, HxCppThread> threads;
		HxCppImmediateProvider immediateProvider;

		public bool Initialize()
		{
			manager = new Manager();
			manager.ProgressEvent += new DebuggerProgressEventHandler(manager_ProgressEvent);
			threads = new Dictionary<int, HxCppThread>();
			return true;
		}

		public void Start()
		{
			try
			{
				initialThreadStop = true;
				activeThread = 0;

				manager.Listen();
				session = manager.Accept();
				session.Bind();
				manager.StopListen();

				immediateProvider = new HxCppImmediateProvider(session);

				if (StartedEvent != null) { StartedEvent(this); };
				// send some events
				// test

				while (true)
				{
					handleEvents();
					if (!session.Connected)
					{
						break;
					}

					// sleep for a bit, then process our events.
					try
					{
						System.Threading.Thread.Sleep(25);
					}
					catch { }
				}
			}
			catch (ManagerAcceptTimeoutExceptio)
			{
				TraceManager.AddAsync("[No debugger connection request]", -1);
			}
			catch (SocketException se)
			{
				// if we requested stop, it's ok, otherwise ...
				// this is not true!
				if (session != null) throw se;
			}
			finally
			{
				if (DisconnectedEvent != null) { DisconnectedEvent(this); }
				if (manager.Listening)
				{
					manager.StopListen();
				}
				if (session != null && session.Connected)
				{
					session.Unbind();
				}
				session = null;
			}
		}

		void manager_ProgressEvent(object sender, int current, int total)
		{
			if (ProgressEvent != null) ProgressEvent(this, current, total);
		}

		private void handleEvents()
		{
			while (session.GetEventCount() > 0)
			{
				Message e = session.GetNextEvent();
				if (e == null)
				{
					// was disconnect
					return;
				}
				if (TraceEvent != null) { TraceEvent(this, e.ToString()); }
				if (e is Message.ThreadStopped)
				{
					Message.ThreadStopped ev = e as Message.ThreadStopped;

					// the first thread stop is used to load breakpoints
					if (initialThreadStop)
					{
						initialThreadStop = false;

						// create the primary thread
						threads.Add(0, new HxCppThread(0));
						threads[0].IsSuspended = true;

						//session.Request(Command.DeleteAllBreakpoints());
						UpdateBreakpoints(PluginMain.breakPointManager.BreakPoints);

						if (ScriptLoadedEvent != null) { ScriptLoadedEvent(this); }
						continue;
					}

					// chcek if we are in this thread or not suspended

					bool sendEvent = true;

					if (ActiveThreadId != ev.number && IsDebuggerSuspended)
					{
						sendEvent = false;
					}

					threads[ev.number].IsSuspended = true;

					Message.ThreadsWhere msg = (Message.ThreadsWhere)session.Request(Command.WhereAllThreads());
					List<ThreadWhereList.Where> tl = MessageUtil.ToList(msg.list);
					foreach (ThreadWhereList.Where w in tl)
					{
						if (threads.ContainsKey(w.number))
						{
							threads[w.number].ThreadStatus = w.status;
						}
					}

					if (threads[ev.number].ThreadStatus is ThreadStatus.StoppedBreakpoint)
					{
						TraceManager.AddAsync("Thread " + ev.number + ": Breakpoint in "+ev.functionName+" on "+ev.fileName+":"+ev.lineNumber);
					}
					if (threads[ev.number].ThreadStatus is ThreadStatus.StoppedCriticalError)
					{
						// switch active thread?
						StringBuilder sb = new StringBuilder();
						sb.Append(TextHelper.GetString("Info.LinePrefixWhenDisplayingFault"));
						sb.Append(" Critical Error");
						sb.Append(TextHelper.GetString("Info.InformationAboutFault"));
						sb.Append(((ThreadStatus.StoppedCriticalError)threads[ev.number].ThreadStatus).description);
						TraceManager.AddAsync("Thread " + ev.number + ": " + sb.ToString(), 3);
					}
					if (threads[ev.number].ThreadStatus is ThreadStatus.StoppedImmediate)
					{
						// ??
						//OnStepEvent();
					}
					if (threads[ev.number].ThreadStatus is ThreadStatus.StoppedUncaughtException)
					{
						// switch active thread?
						StringBuilder sb = new StringBuilder();
						sb.Append(TextHelper.GetString("Info.LinePrefixWhenDisplayingFault"));
						sb.Append(" Uncaught Exception");
						TraceManager.AddAsync("Thread " + ev.number + ": " + sb.ToString(), 3);
						//OnFaultEvent();
					}
					if (sendEvent)
					{
						ActiveThreadId = ev.number;
						frames = null;
						getFrames();
						OnBreakpointEvent();
					}
					else
					{
						OnThreadsEvent();
					}
				}
				if (e is Message.ThreadStarted)
				{
					Message.ThreadStarted ev = e as Message.ThreadStarted;
					threads[ev.number].IsSuspended = false;
					OnThreadsEvent();
				}
				if (e is Message.ThreadCreated)
				{
					Message.ThreadCreated ev = e as Message.ThreadCreated;
					threads.Add(ev.number, new HxCppThread(ev.number));
					OnThreadsEvent();
				}
				if (e is Message.ThreadTerminated)
				{
					Message.ThreadTerminated ev = e as Message.ThreadTerminated;
					threads.Remove(ev.number);
					// check current thread?
					OnThreadsEvent();
				}
				

			}

		}



		public void Continue()
		{
			session.Request(Command.Continue(1));
		}

		public void Detach()
		{
			session.Request(Command.Detach());
		}

		public bool IsDebuggerStarted
		{
			get { return session != null && session.Connected; }
		}

		public bool IsDebuggerSuspended
		{
			get { return threads[ActiveThreadId].IsSuspended; }
		}

		public void UpdateBreakpoints(List<BreakPointInfo> breakpoints)
		{
			// add new ones
			// todo, cache this in session, should not change while running, we don't dyn load anything.. right?
			string[] fres = ((Message.Files)session.Request(Command.Files())).list;
			string[] fullres = ((Message.FilesFullPath)session.Request(Command.FilesFullPath())).list;
			HxCppSourceFile.Clear();
			for (int i = 0; i < fres.Length; i++)
			{
				if (i < fullres.Length)
				{
					HxCppSourceFile.MapAdd(fres[i], fullres[i]);
				}
			}
			Dictionary<string, string> map = new Dictionary<string, string>(fres.Length);
			foreach (string f in fres)
			{
				String localPath = HxCppSourceFile.FromString(f).LocalPath;
				if (localPath != null)
				{
					map.Add(localPath, f);
				}
			}

			foreach (BreakPointInfo bp in breakpoints)
			{
				if (bp.InternalData == null)
				{
					if (bp.IsEnabled && !bp.IsDeleted)
					{
						if (map.ContainsKey(bp.FileFullPath))
						{
							string remo = map[bp.FileFullPath];
							Message res = session.Request(Command.AddFileLineBreakpoint(remo, bp.Line + 1)); // todo, handle line fixes somewhere else
							if (res is Message.FileLineBreakpointNumber)
							{
								bp.InternalData = ((Message.FileLineBreakpointNumber)res).number;
							}
						}
					}
				}
				else
				{
					if (bp.IsDeleted || !bp.IsEnabled)
					{
						Message res = session.Request(Command.DeleteBreakpointRange((int)bp.InternalData, (int)bp.InternalData));
						bp.InternalData = null;
					}
				}
			}
		}

		public bool ShouldBreak(BreakPointInfo bpInfo)
		{
			if (bpInfo.Exp == null || bpInfo.Exp.Length == 0) return true;
			Message ret = session.Request(Command.GetExpression(false, bpInfo.Exp));
			if (ret is Message.Variable)
			{
				Message.Variable var_ = (Message.Variable)ret;
				VariableValue.Item item = (VariableValue.Item)var_.value;
				if (item.type == "Bool")
				{
					return item.value == "true";
				}
				// todo, other types
			}
			else
			{
				// an error
			}

			return true;
		}


		public void Next()
		{
			session.Request(Command.Next(1));
		}

		public void Step()
		{
			session.Request(Command.Step(1));
		}

		public void Pause()
		{
			session.Request(Command.BreakNow());
		}

		public void Stop()
		{
			if (manager.Listening)
			{
				manager.StopListen();
			}
			else
			{
				session.Request(Command.Exit());
			}
		}

		public void Finish()
		{
			session.Request(Command.Finish(1));
		}

		private FrameList.Frame[] frames = null;
		private ThreadStatus threadStatus = null;

		private FrameList.Frame[] getFrames()
		{
			if (frames == null)
			{
				Message.ThreadsWhere msg = (Message.ThreadsWhere)session.Request(Command.WhereCurrentThread(false));
				List<ThreadWhereList.Where> tl = MessageUtil.ToList(msg.list);
				threadStatus = tl[0].status;
				List<FrameList.Frame> fl = MessageUtil.ToList(tl[0].frameList);
				frames = fl.ToArray();
			}
			return frames;
		}

		public DbgLocation GetCurrentLocation()
		{
			try
			{
				return HxCppLocation.FromFrame(getFrames()[0]);
			}
			catch (Exception)
			{
				throw;
				//return null;
			}
		}


		public DbgFrame[] GetFrames()
		{
			try
			{
				FrameList.Frame[] fl = getFrames();
				DbgFrame[] ret = new DbgFrame[fl.Length];
				for (int i=0; i<fl.Length; i++)
				{
					ret[i] = HxCppFrame.FromFrame(fl[i]);
				}
				return ret;
			}
			catch (Exception)
			{
				throw;
				//return null;
			}
		}

		public DataNode[] GetVariableNodes(int frameNumber)
		{
			setCurrentFrame(frameNumber);
			Message.Variables msg = (Message.Variables)session.Request(Command.Variables(false));
			string[] vars = msg.list;
			List<HxCppDataNode> ret = new List<HxCppDataNode>(vars.Length);
			foreach (string var in vars)
			{
				ret.Add(new HxCppDataNode(var, var, session));
			}
			return ret.ToArray();
		}

		// we need this, because the frames in HxCpp are somehow upside-down in comparison to flash
		private void setCurrentFrame(int frameNumber)
		{
			session.Request(Command.SetFrame(getFrames()[frameNumber].number));
		}

		public DataNode GetExpressionNode(string expr)
		{
			// this should be allowed to throw.
			return new HxCppDataNode(expr, expr, session);
		}

		public int ActiveThreadId
		{
			get
			{
				return activeThread;
			}
			set
			{
				if (activeThread != value)
				{
					if (threads.ContainsKey(value) && !threads[value].IsSuspended)
					{
						// currently hxcpp engine cannot switch to a running thread... bummer
						return;
					}
					session.Request(Command.SetCurrentThread(value));
					activeThread = value;
					OnThreadsEvent();
				}
			}
		}

		public DbgThread[] GetThreads()
		{
			HxCppThread[] ret = new HxCppThread[threads.Count];
			threads.Values.CopyTo(ret, 0);
			return ret;
		}

		public ImmediateProvider ImmediateProvider
		{
			get { return immediateProvider; }
		}

		#region Event helpers
		public virtual void OnThreadsEvent()
		{
			if (ThreadsEvent != null)
			{
				ThreadsEvent(this);
			}
		}
		public virtual void OnFaultEvent()
		{
			if (FaultEvent != null)
			{
				FaultEvent(this);
			}
		}
		public virtual void OnStepEvent()
		{
			if (StepEvent != null)
			{
				StepEvent(this);
			}
		}
		public virtual void OnBreakpointEvent()
		{
			if (BreakpointEvent != null)
			{
				BreakpointEvent(this);
			}
		}
		#endregion
	}
}
