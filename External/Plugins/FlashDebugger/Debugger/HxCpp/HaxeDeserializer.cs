﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FlashDebugger.Debugger.HxCpp
{
	public class HaxeEnum
	{
		public string name;
		public string constructor;
		public List<object> arguments;

		public HaxeEnum(string name, string constructor)
		{
			this.name = name;
			this.constructor = constructor;
			this.arguments = new List<object>();
		}

		public override string ToString()
		{
			string ret = name + "::" + constructor + "(";
			bool first = true;
			foreach (object o in arguments)
			{
				if (!first) ret += ", ";
				first = false;
				ret += o.ToString();
			}
			ret += ")";
			return ret;
		}
	}

	// todo
	public class HaxeList : List<object>
	{
	}

	public class HaxeDeserializer
	{
		private List<string> stringCache = new List<string>();
		private List<object> objectCache = new List<object>();

		public HaxeDeserializer()
		{
		}

		public Object Deserialize(Stream buffer)
		{
			int pref = buffer.ReadByte();
			switch (pref)
			{
				case 'n':
					return null;
				case 'z':
					return 0;
				case 'i':
					return deserializeInt(buffer);
				case 'k':
					return float.NaN;
				case 'm':
					return float.NegativeInfinity;
				case 'p':
					return float.PositiveInfinity;
				case 'd':
					return deserializeFloat(buffer);
				case 't':
					return true;
				case 'f':
					return false;
				case 'y':
					return deserializeString(buffer, false);
				case 'R':
					return deserializeString(buffer, true);
				case 'o':
					// anon object
					throw new NotImplementedException();
				case 'g':
					// end of object
					throw new NotImplementedException();
				case 'l':
					// list
					return deserializeList(buffer);
				case 'h':
					// end of list, array, hash, inthash
					return new ListTerminator();
				case 'a':
					// array
					return deserializeArray(buffer);
				case 'v':
					// date
					throw new NotImplementedException();
				case 'b':
					// hash
					throw new NotImplementedException();
				case 'q':
					// int hash
					throw new NotImplementedException();
				case 's':
					// bytes
					throw new NotImplementedException();
				case 'x':
					// exception
					throw new NotImplementedException();
				case 'c':
					// class
					throw new NotImplementedException();
				case 'w':
					// enum
					return deserializeEnum(buffer, true);
				case 'j':
					// index enum
					return deserializeEnum(buffer, false);
				case 'C':
					// custom class
					throw new NotImplementedException();
				case 'r':
					// object hash
					int i = deserializeInt(buffer);
					return objectCache[i];
				default:
					// unknown
					throw new NotImplementedException("Unknown haxe serialization prefix "+(char)pref);
			}
		}

		private int deserializeInt(Stream buffer)
		{
			// i1234
			int ret = 0;
			int sign = 1;
			while (true)
			{
				int n = buffer.ReadByte();
				if (n == '-')
				{
					sign = -1;
				}
				else if (n >= '0' && n <= '9')
				{
					ret = ret * 10 + (n - '0');
				}
				else if (n == -1)
				{
					break;
				}
				else
				{
					buffer.Position = buffer.Position - 1;
					break;
				}
			}
			return ret * sign;
		}

		private float deserializeFloat(Stream buffer)
		{
			// d1.45e-8
			string data = "";
			while (true)
			{
				int n = buffer.ReadByte();
				if (n == '-' || (n >= '0' && n <= '9') || n == '.' || n == '+' || n == 'e')
				{
					data += (char)n;
				}
				else if (n == -1)
				{
					break;
				}
				else
				{
					buffer.Position = buffer.Position - 1;
					break;
				}
			}
			return float.Parse(data, System.Globalization.CultureInfo.InvariantCulture);
		}

		private string deserializeString(Stream buffer, bool fromCache)
		{
			// y10:hi%20there for "hi there".
			// R123
			if (fromCache)
			{
				int i = deserializeInt(buffer);
				return stringCache[i];
			}
			int len = deserializeInt(buffer);
			// checked len?
			int delim = buffer.ReadByte();
			if (delim != ':')
			{
				throw new HaxeSerializationFormatException();
			}
			byte[] tmp = new byte[len];
			buffer.Read(tmp, 0, len);
			string tmp2 = System.Text.Encoding.Default.GetString(tmp);
			string ret = System.Web.HttpUtility.UrlDecode(tmp2);
			stringCache.Add(ret);
			return ret;
		}

		private HaxeEnum deserializeEnum(Stream buffer, bool byName)
		{
			// Enum (by name) : w followed by the enum name, the constructor name (as String), the number of arguments and the arguments itself	
			// example : wy3:Fooy1:A0 for Foo.A (with no arguments)
			// example : wy3:Fooy1:B2i4n for Foo.B(4,null)

			// Enum (by index) : same as by name but using j instead of w for prefix, and <int>: instead of the constructor name :

			// NOTE: DOCUMENTATION IS WRONG, here real example

			// wy16:debugger.Messagey13:ThreadStopped:5zy4:Mainy4:mainy7:Main.hxi21

			string name = (string)Deserialize(buffer);
			string constructor;
			if (byName)
			{
				constructor = (string)Deserialize(buffer);
			}
			else
			{
				int idelim = buffer.ReadByte();
				if (idelim != ':') throw new HaxeSerializationFormatException();
				constructor = string.Format("{0}", deserializeInt(buffer));
			}
			int delim = buffer.ReadByte();
			if (delim != ':') throw new HaxeSerializationFormatException();
			int numargs = deserializeInt(buffer);
			HaxeEnum ret = new HaxeEnum(name, constructor);
			objectCache.Add(ret);
			for (int i = 0; i < numargs; i++)
			{
				ret.arguments.Add(Deserialize(buffer));
			}
			return ret;
		}

		private HaxeList deserializeList(Stream buffer)
		{
			// List	 : l followed by the list of serialized items, and ending with a h (ex : lnnh for a List containing two nulls)
			HaxeList ret = new HaxeList();
			object obj = Deserialize(buffer); 
			while (!(obj is ListTerminator))
			{
				ret.Add(obj);
				obj = Deserialize(buffer);
			}
			return ret;
		}

		private Object[] deserializeArray(Stream buffer)
		{
			// Array : a followed by serialized items, and ending with a h
			// if there are several consecutive nulls, we can store u5 instead of nnnnn
			// example : ai1i2u4i7ni9h for [1,2,null,null,null,null,7,null,9]
			List<Object> ret = new List<Object>();
			object obj = Deserialize(buffer);
			while (!(obj is ListTerminator))
			{
				ret.Add(obj);
				obj = Deserialize(buffer);
			}
			return ret.ToArray();
		}

		private class ListTerminator
		{
		}
	}

	public class HaxeSerializationFormatException : Exception
	{
	}
}
