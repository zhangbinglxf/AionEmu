using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.Common
{
	public class Util
	{
		public delegate bool HandlerRoutine(int dw);

		[CompilerGenerated]
		internal sealed class _Closure_0024__15_002D0
		{
			public string _0024VB_0024Local_out;

			public _Closure_0024__15_002D0(_Closure_0024__15_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_out = arg0._0024VB_0024Local_out;
				}
			}

			internal void _Lambda_0024__0(byte b)
			{
				_0024VB_0024Local_out = _0024VB_0024Local_out + Conversion.Hex(b).PadLeft(2, '0') + " ";
			}

			internal void _Lambda_0024__1(byte b)
			{
				if (b > 31 && b < 128)
				{
					_0024VB_0024Local_out += Conversions.ToString(Strings.Chr(b));
				}
				else
				{
					_0024VB_0024Local_out += ".";
				}
			}
		}

		private static readonly DateTime StaticDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);


		public static void RemoveMenu()
		{
			
		}

		public static void RegeditCloseEvent(ref HandlerRoutine handler, bool add)
		{
			
		}

		public static long CurrentTimeMillis()
		{
			return checked((long)Math.Round(DateTime.Now.Subtract(StaticDate).TotalMilliseconds));
		}

		public static long GetTime(DateTime value)
		{
			return checked((long)Math.Round(value.Subtract(StaticDate).TotalMilliseconds));
		}

		public static int GetDay(DateTime value)
		{
			return checked((int)Math.Round((double)GetTime(value) / 1000.0 / 3600.0 / 24.0));
		}

		public static void SystemInfo()
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Section("版本信息");
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			Version version = callingAssembly.GetName().Version;
			Console.WriteLine("# 程序名称: Aion Emu - {0} 主版本号:{1}.{2} 编译版本:{3}.{4}", callingAssembly.GetName().Name, version.Major, version.Minor, version.Build, version.Revision);
			Console.WriteLine("# 运行平台: Microsoft .NET Framework (.Net core) Version:{0}.{1}.{2}", Environment.Version.Major, Environment.Version.Minor, Environment.Version.Build);
			Section("");
		}

		public static object StringLength(string data)
		{
			int num = 0;
			checked
			{
				foreach (char @string in data)
				{
					num = ((!((Strings.Asc(@string) >= 48) & (Strings.Asc(@string) <= 122))) ? (num + 2) : (num + 1));
				}
				int num2 = Console.WindowWidth - num;
				return Operators.DivideObject(Interaction.IIf(unchecked(num2 % 2) != 0, num2 + 1, num2), 2);
			}
		}

		public static void Section(string data)
		{
			int num = 4;
			string text = data;
			checked
			{
				foreach (char @string in text)
				{
					num = ((!((Strings.Asc(@string) >= 48) & (Strings.Asc(@string) <= 122))) ? (num + 2) : (num + 1));
				}
				if (!string.IsNullOrEmpty(data))
				{
					data = "[ " + data + " ]";
				}
				else
				{
					num = 0;
				}
				while (num < Console.WindowWidth - 1)
				{
					num++;
					data = "-" + data;
				}
				Console.WriteLine(data);
			}
		}

		public static string ToHex(byte[] value, int offset)
		{
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			checked
			{
				_Closure_0024__15_002D0 closure_0024__15_002D = default(_Closure_0024__15_002D0);
				while (offset < value.Length)
				{
					closure_0024__15_002D = new _Closure_0024__15_002D0(closure_0024__15_002D);
					int num2 = Conversions.ToInteger(Interaction.IIf(value.Length - offset >= 16, 16, value.Length - offset));
					byte[] array = new byte[num2 - 1 + 1];
					Array.Copy(value, offset, array, 0, num2);
					offset += num2;
					num++;
					closure_0024__15_002D._0024VB_0024Local_out = Conversion.Hex(num * 16).PadLeft(4, '0') + ": ";
					array.ToList().ForEach(closure_0024__15_002D._Lambda_0024__0);
					closure_0024__15_002D._0024VB_0024Local_out = Strings.LSet(closure_0024__15_002D._0024VB_0024Local_out, 60);
					Array.ForEach(array, closure_0024__15_002D._Lambda_0024__1);
					stringBuilder.AppendLine(closure_0024__15_002D._0024VB_0024Local_out);
				}
				return stringBuilder.ToString();
			}
		}
	}
}
