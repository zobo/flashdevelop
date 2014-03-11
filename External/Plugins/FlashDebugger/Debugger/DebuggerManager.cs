﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using flash.tools.debugger;
using PluginCore.Localization;
using PluginCore.Managers;
using ProjectManager.Projects;
using ProjectManager.Projects.AS3;
using ScintillaNet;
using PluginCore;
using net.sf.jni4net;
using PluginCore.Helpers;
using ProjectManager.Projects.Haxe;
using FlashDebugger.Debugger.Flash;
using FlashDebugger.Debugger;
using FlashDebugger.Controls;
using FlashDebugger.Debugger.HxCpp;

namespace FlashDebugger
{
    public delegate void StateChangedEventHandler(object sender, DebuggerState state);

	public enum DebuggerState
	{
		Initializing,
		Stopped,
		Starting,
		Running,
		Pausing,
		PauseHalt,
		BreakHalt,
		ExceptionHalt
	}

    public enum DebuggerEngine
    {
        Flash,
        HxCpp
    }

	public class DebuggerManager
    {
        public event StateChangedEventHandler StateChangedEvent;

        internal Project currentProject;
		private BackgroundWorker bgWorker;
        private DebuggerInterface m_Interface;
        private FlashInterface m_FlashInterface;
		private HxCppInterface m_HxCppInterface;
		private DbgLocation m_CurrentLocation = null;
        private Int32 m_CurrentFrame = 0;

        public DebuggerManager()
        {
			m_FlashInterface = new FlashInterface();
            registerInterfaceEvents(m_FlashInterface);
			m_HxCppInterface = new HxCppInterface();
			registerInterfaceEvents(m_HxCppInterface);
			SelectDebugger(DebuggerEngine.Flash);
			m_FlashInterface.ThreadsEvent += new DebuggerEventHandler(m_FlashInterface_ThreadsEvent);
        }

        private void registerInterfaceEvents(DebuggerInterface debugger)
        {
            debugger.StartedEvent += new DebuggerEventHandler(flashInterface_StartedEvent);
            debugger.DisconnectedEvent += new DebuggerEventHandler(flashInterface_DisconnectedEvent);
            debugger.BreakpointEvent += new DebuggerEventHandler(flashInterface_BreakpointEvent);
            debugger.FaultEvent += new DebuggerEventHandler(flashInterface_FaultEvent);
            debugger.PauseEvent += new DebuggerEventHandler(flashInterface_PauseEvent);
            debugger.StepEvent += new DebuggerEventHandler(flashInterface_StepEvent);
            debugger.ScriptLoadedEvent += new DebuggerEventHandler(flashInterface_ScriptLoadedEvent);
            debugger.WatchpointEvent += new DebuggerEventHandler(flashInterface_WatchpointEvent);
            debugger.UnknownHaltEvent += new DebuggerEventHandler(flashInterface_UnknownHaltEvent);
            debugger.ProgressEvent += new DebuggerProgressEventHandler(flashInterface_ProgressEvent);
			debugger.TraceEvent += new TraceEventHandler(flashInterface_TraceEvent);
        }

        /*
        private void unregisterInterfaceEvents()
        {
            m_FlashInterface.StartedEvent -= new DebuggerEventHandler(flashInterface_StartedEvent);
            m_FlashInterface.DisconnectedEvent -= new DebuggerEventHandler(flashInterface_DisconnectedEvent);
            m_FlashInterface.BreakpointEvent -= new DebuggerEventHandler(flashInterface_BreakpointEvent);
            m_FlashInterface.FaultEvent -= new DebuggerEventHandler(flashInterface_FaultEvent);
            m_FlashInterface.PauseEvent -= new DebuggerEventHandler(flashInterface_PauseEvent);
            m_FlashInterface.StepEvent -= new DebuggerEventHandler(flashInterface_StepEvent);
            m_FlashInterface.ScriptLoadedEvent -= new DebuggerEventHandler(flashInterface_ScriptLoadedEvent);
            m_FlashInterface.WatchpointEvent -= new DebuggerEventHandler(flashInterface_WatchpointEvent);
            m_FlashInterface.UnknownHaltEvent -= new DebuggerEventHandler(flashInterface_UnknownHaltEvent);
            m_FlashInterface.ProgressEvent -= new DebuggerProgressEventHandler(flashInterface_ProgressEvent);
        }
         */

        #region Startup

        public void SelectDebugger(DebuggerEngine debugger)
        {
            // if not the same as now?
            // fail if running
            if (debugger == DebuggerEngine.Flash)
            {
                m_Interface = m_FlashInterface;
            }
			else if (debugger == DebuggerEngine.HxCpp)
			{
				m_Interface = m_HxCppInterface;
			}
			else
			{
				throw new Exception("UNIMPLEMENTED");
			}
        }



        /// <summary>
        /// 
        /// </summary>
        private bool CheckCurrent()
        {
            try
            {
                IProject project = PluginBase.CurrentProject;
                if (project == null || !project.EnableInteractiveDebugger) return false;
                currentProject = project as Project;

                // ignore non-flash haxe targets
                if (project is HaxeProject)
                {
                    HaxeProject hproj = project as HaxeProject;
                    if (hproj.MovieOptions.Platform == HaxeMovieOptions.NME_PLATFORM
                        && (hproj.TargetBuild != null && !hproj.TargetBuild.StartsWith("flash") && hproj.TargetBuild != "windows"))
                        return false;
                }
                // Give a console warning for non external player...
                if (currentProject.TestMovieBehavior == TestMovieBehavior.NewTab || currentProject.TestMovieBehavior == TestMovieBehavior.NewWindow)
                {
                    TraceManager.Add(TextHelper.GetString("Info.CannotDebugActiveXPlayer"));
					return false;
                }
            }
            catch (Exception e) 
            { 
                ErrorManager.ShowError(e);
                return false;
            }
			return true;
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool Start()
        {
            return Start(false);
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool Start(bool alwaysStart)
        {
            if (!alwaysStart && !CheckCurrent()) return false;
            UpdateMenuState(DebuggerState.Starting);

			DebuggerInterface.Initialize();

            PluginBase.MainForm.ProgressBar.Visible = true;
            PluginBase.MainForm.ProgressLabel.Visible = true;
            PluginBase.MainForm.ProgressLabel.Text = TextHelper.GetString("Info.WaitingForPlayer");
            if (bgWorker == null || !bgWorker.IsBusy)
            {
                // only run a debugger if one is not already runnin - need to redesign core to support multiple debugging instances
                // other option: detach old worker, wait for it to exit and start new one
                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += bgWorker_DoWork;
                bgWorker.RunWorkerAsync();
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
				DebuggerInterface.Start();
            }
            catch (Exception ex)
            {
				(PluginBase.MainForm as Form).BeginInvoke((MethodInvoker)delegate()
				{
					ErrorManager.ShowError("Internal Debugger Exception", ex);
				});
            }
			FlashSourceFile.Clear();
			HxCppSourceFile.Clear();
        }

        #endregion

        #region Properties

		public FlashInterface FlashInterface
		{
			get { return m_FlashInterface; }
		}

		public DebuggerInterface DebuggerInterface
		{
			get { return m_Interface; }
		}

		public int CurrentFrame
        {
            get { return m_CurrentFrame; }
            set
            {
                if (m_CurrentFrame != value)
                {
                    m_CurrentFrame = value;
                    UpdateLocalsUI();
                }
            }
        }

        public DbgLocation CurrentLocation
        {
            get { return m_CurrentLocation; }
            set
            {
                if (m_CurrentLocation != value)
                {
                    if (m_CurrentLocation != null)
                    {
                        ResetCurrentLocation();
                    }
                    m_CurrentLocation = value;
                    if (m_CurrentLocation != null)
                    {
                        GotoCurrentLocation(true);
                    }
                }
            }
        }

        #endregion

		#region LayoutHelpers

		/// <summary>
		/// Check if old layout is sotred and restores it. It also deletes this temporary layout file.
		/// </summary>
		public void RestoreOldLayout()
		{
			String oldLayoutFile = Path.Combine(Path.Combine(PathHelper.DataDir, "FlashDebugger"), "oldlayout.fdl");
			if (File.Exists(oldLayoutFile))
			{
				PluginBase.MainForm.CallCommand("RestoreLayout", oldLayoutFile);
				File.Delete(oldLayoutFile);
			}
		}

		#endregion

        #region FlashInterface Control

        /// <summary>
        /// 
        /// </summary>
        public void Cleanup()
        {
			FlashSourceFile.Clear();
			HxCppSourceFile.Clear();
			m_FlashInterface.Cleanup();
			// todo hxcpp cleanup?
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateMenuState(DebuggerState state)
        {
            if (StateChangedEvent != null) StateChangedEvent(this, state);
        }

        #endregion

        #region FlashInterface Events
        
        /// <summary>
        /// 
        /// </summary>
        private void flashInterface_StartedEvent(object sender)
		{
			if ((PluginBase.MainForm as Form).InvokeRequired)
			{
				(PluginBase.MainForm as Form).BeginInvoke((MethodInvoker)delegate()
				{
					flashInterface_StartedEvent(sender);
				});
				return;
			}
			UpdateMenuState(DebuggerState.Running);
            PluginBase.MainForm.ProgressBar.Visible = false;
            PluginBase.MainForm.ProgressLabel.Visible = false;
			if (PluginMain.settingObject.SwitchToLayout != null)
			{
				// save current state
				String oldLayoutFile = Path.Combine(Path.Combine(PathHelper.DataDir, "FlashDebugger"), "oldlayout.fdl");
				PluginBase.MainForm.DockPanel.SaveAsXml(oldLayoutFile);
				PluginBase.MainForm.CallCommand("RestoreLayout", PluginMain.settingObject.SwitchToLayout);
			}
			else if (!PluginMain.settingObject.DisablePanelsAutoshow)
			{
                PanelsHelper.watchPanel.Show();
                PanelsHelper.pluginPanel.Show();
                PanelsHelper.threadsPanel.Show();
                PanelsHelper.immediatePanel.Show();
				PanelsHelper.breakPointPanel.Show();
                PanelsHelper.stackframePanel.Show();
			}
		}

        /// <summary>
        /// 
        /// </summary>
        private void flashInterface_DisconnectedEvent(object sender)
		{
			if ((PluginBase.MainForm as Form).InvokeRequired)
			{
				(PluginBase.MainForm as Form).BeginInvoke((MethodInvoker)delegate()
				{
					flashInterface_DisconnectedEvent(sender);
				});
				return;
			}
			CurrentLocation = null;
			UpdateMenuState(DebuggerState.Stopped);
			if (PluginMain.settingObject.SwitchToLayout != null)
            {
				RestoreOldLayout();
			}
            else if (!PluginMain.settingObject.DisablePanelsAutoshow)
            {
                PanelsHelper.watchPanel.Hide();
                PanelsHelper.pluginPanel.Hide();
                PanelsHelper.threadsPanel.Hide();
                PanelsHelper.immediatePanel.Hide();
                PanelsHelper.breakPointPanel.Hide();
                PanelsHelper.stackframePanel.Hide();
            }
			PanelsHelper.pluginUI.TreeControl.Nodes.Clear();
			PanelsHelper.stackframeUI.ClearItem();
			PanelsHelper.watchUI.Clear();
			PanelsHelper.threadsUI.ClearItem();
			PluginMain.breakPointManager.ResetAll();
            PluginBase.MainForm.ProgressBar.Visible = false;
            PluginBase.MainForm.ProgressLabel.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void flashInterface_BreakpointEvent(object sender)
		{
			DbgLocation loc = DebuggerInterface.GetCurrentLocation();
			// todo checking for loc here, but should handle swfloaded case and wait with breakpoint event
			if (loc != null && PluginMain.breakPointManager.ShouldBreak(loc.File, loc.Line))
			{
				UpdateUI(DebuggerState.BreakHalt);
			}
			else
			{
				m_Interface.Step();
				//FlashInterface.StepResume();
			}
		}

        /// <summary>
        /// 
        /// </summary>
        private void flashInterface_FaultEvent(object sender)
		{
			UpdateUI(DebuggerState.ExceptionHalt);
		}

        /// <summary>
        /// 
        /// </summary>
        private void flashInterface_StepEvent(object sender)
		{
			UpdateUI(DebuggerState.BreakHalt);
		}

        /// <summary>
        /// 
        /// </summary>
        private void flashInterface_ScriptLoadedEvent(object sender)
		{
			// this was moved directly into flashInterface
            // force all breakpoints update after new as code loaded into debug movie 
            //PluginMain.breakPointManager.ForceBreakPointUpdates();
			//m_Interface.UpdateBreakpoints(PluginMain.breakPointManager.GetBreakPointUpdates());
			m_Interface.Continue();
		}

        /// <summary>
        /// 
        /// </summary>
        private void flashInterface_WatchpointEvent(object sender)
		{
			UpdateUI(DebuggerState.BreakHalt);
		}

        /// <summary>
        /// 
        /// </summary>
        private void flashInterface_UnknownHaltEvent(object sender)
		{
			UpdateUI(DebuggerState.ExceptionHalt);
		}

		/// <summary>
		/// 
		/// </summary>
		private void flashInterface_PauseEvent(Object sender)
		{
			UpdateUI(DebuggerState.PauseHalt);
		}

        /// <summary>
        /// 
        /// </summary>
		void m_FlashInterface_ThreadsEvent(object sender)
		{
			if (FlashInterface.IsDebuggerSuspended)
			{
				// TODO there will be redunandt calls
				UpdateUI(DebuggerState.BreakHalt);
			}
			else
			{
				// TODO should get a signal that thread has changed, keep local number...
				UpdateThreadsUI();
				UpdateMenuState(DebuggerState.Running);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        private void UpdateUI(DebuggerState state)
		{
			if ((PluginBase.MainForm as Form).InvokeRequired)
			{
				(PluginBase.MainForm as Form).BeginInvoke((MethodInvoker)delegate()
				{
					UpdateUI(state);
				});
				return;
			}
            try
            {
                CurrentLocation = DebuggerInterface.GetCurrentLocation();
                UpdateStackUI();
                UpdateLocalsUI();
                UpdateMenuState(state);
				UpdateThreadsUI();
                (PluginBase.MainForm as Form).Activate();
            }
            catch (PlayerDebugException ex)
            {
                ErrorManager.ShowError("Internal Debugger Exception", ex);
            }
		}

        /// <summary>
        /// 
        /// </summary>
        private void UpdateStackUI()
		{
			m_CurrentFrame = 0;
			DbgFrame[] frames = m_Interface.GetFrames();
			PanelsHelper.stackframeUI.AddFrames(frames);
		}

        /// <summary>
        /// 
        /// </summary>
		private void UpdateLocalsUI()
		{
			if ((PluginBase.MainForm as Form).InvokeRequired)
			{
				(PluginBase.MainForm as Form).BeginInvoke((MethodInvoker)delegate()
				{
					UpdateLocalsUI();
				});
				return;
			}
			DbgFrame[] frames = m_Interface.GetFrames();
            if (frames != null && m_CurrentFrame < frames.Length)
			{
				PanelsHelper.pluginUI.Clear();
				DataNode[] all = m_Interface.GetVariableNodes(m_CurrentFrame);
				if (all.Length > 0)
				{
					PanelsHelper.pluginUI.SetDataNodes(all);
				}
				CurrentLocation = frames[m_CurrentFrame].Location;
				PanelsHelper.watchUI.UpdateElements();
			}
			else CurrentLocation = null;
		}

		private void UpdateThreadsUI()
		{
			if ((PluginBase.MainForm as Form).InvokeRequired)
			{
				(PluginBase.MainForm as Form).BeginInvoke((MethodInvoker)delegate()
				{
					UpdateThreadsUI();
				});
				return;
			}
			PanelsHelper.threadsUI.SetThreads(m_Interface.GetThreads());
		}

        /// <summary>
        /// 
        /// </summary>
        private void ResetCurrentLocation()
		{
			if ((PluginBase.MainForm as Form).InvokeRequired)
			{
				(PluginBase.MainForm as Form).BeginInvoke((MethodInvoker)delegate()
				{
					ResetCurrentLocation();
				});
				return;
			}
			if (CurrentLocation.File != null)
			{
				ScintillaControl sci;
				String localPath = CurrentLocation.File.LocalPath;
				if (localPath != null)
				{
					sci = ScintillaHelper.GetScintillaControl(localPath);
					if (sci != null)
					{
						Int32 i = ScintillaHelper.GetScintillaControlIndex(sci);
						if (i != -1)
						{
							Int32 line = CurrentLocation.Line - 1; // TODO THIS OFFSETTING SHOULD BE MOVED INTO CONCRETE IMPLEMENTATION, DEPENDS ON DEBUGGER
							sci.MarkerDelete(line, ScintillaHelper.markerCurrentLine);
						}
					}
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
        private void GotoCurrentLocation(bool bSetMarker)
		{
			if ((PluginBase.MainForm as Form).InvokeRequired)
			{
				(PluginBase.MainForm as Form).BeginInvoke((MethodInvoker)delegate()
				{
					GotoCurrentLocation(bSetMarker);
				});
				return;
			}
			if (CurrentLocation != null && CurrentLocation.File != null)
			{
				ScintillaControl sci;
				String localPath = CurrentLocation.File.LocalPath;
				if (localPath != null)
				{
					sci = ScintillaHelper.GetScintillaControl(localPath);
					if (sci == null)
					{
						PluginBase.MainForm.OpenEditableDocument(localPath);
						sci = ScintillaHelper.GetScintillaControl(localPath);
					}
					if (sci != null)
					{
						Int32 i = ScintillaHelper.GetScintillaControlIndex(sci);
						if (i != -1)
						{
							PluginBase.MainForm.Documents[i].Activate();
							Int32 line = CurrentLocation.Line - 1;
							sci.GotoLine(line);
							if (bSetMarker)
							{
								sci.MarkerAdd(line, ScintillaHelper.markerCurrentLine);
							}
						}
					}
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
        private void flashInterface_PauseNotRespondEvent(object sender)
        {
            if ((PluginBase.MainForm as Form).InvokeRequired)
            {
                (PluginBase.MainForm as Form).BeginInvoke((MethodInvoker)delegate()
                {
                    flashInterface_PauseNotRespondEvent(sender);
                });
                return;
            }
            DialogResult res = MessageBox.Show(PluginBase.MainForm, TextHelper.GetString("Title.CloseProcess"), TextHelper.GetString("Info.ProcessNotResponding"), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            if (res == DialogResult.OK)
            {
				m_FlashInterface.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void flashInterface_TraceEvent(Object sender, String trace)
        {
            TraceManager.AddAsync(trace, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        private void flashInterface_ProgressEvent(object sender, int current, int total)
        {
            if ((PluginBase.MainForm as Form).InvokeRequired)
            {
                (PluginBase.MainForm as Form).BeginInvoke((MethodInvoker)delegate()
                {
                    flashInterface_ProgressEvent(sender, current, total);
                });
                return;
            }
            PluginBase.MainForm.ProgressBar.Maximum = total;
            PluginBase.MainForm.ProgressBar.Value = current;
        }

        #endregion

        #region MenuItem Event Handling

        /// <summary>
        /// 
        /// </summary>
        internal void Stop_Click(Object sender, EventArgs e)
        {
            PluginMain.liveDataTip.Hide();
			CurrentLocation = null;
			m_Interface.Stop();
        }

		/// <summary>
		/// 
		/// </summary>
		internal void Current_Click(Object sender, EventArgs e)
		{
			if (DebuggerInterface.IsDebuggerStarted && DebuggerInterface.IsDebuggerSuspended)
			{
				GotoCurrentLocation(false);
			}
		}

		/// <summary>
        /// 
        /// </summary>
        internal void Next_Click(Object sender, EventArgs e)
        {
			CurrentLocation = null;
			m_Interface.Next();
			UpdateMenuState(DebuggerState.Running);
		}

        /// <summary>
        /// 
        /// </summary>
        internal void Step_Click(Object sender, EventArgs e)
        {
			CurrentLocation = null;
			m_Interface.Step();
			UpdateMenuState(DebuggerState.Running);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Continue_Click(Object sender, EventArgs e)
        {
            try
            {
				CurrentLocation = null;
				// this should not be needed, as we update breakpoints right away
				//m_Interface.UpdateBreakpoints(PluginMain.breakPointManager.GetBreakPointUpdates());
				m_Interface.Continue();
				UpdateMenuState(DebuggerState.Running);
				UpdateThreadsUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Pause_Click(Object sender, EventArgs e)
        {
			CurrentLocation = null;
			m_Interface.Pause();
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Finish_Click(Object sender, EventArgs e)
        {
			CurrentLocation = null;
			m_Interface.Finish();
			UpdateMenuState(DebuggerState.Running);
		}

        #endregion

    }

}
