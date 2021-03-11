using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class XmlDataHandle<T> where T : IDataTemplate
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static ValidationEventHandler _0024I4_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			internal void _Lambda_0024__4_002D0(object x, ValidationEventArgs y)
			{
				XmlDataHandle<T>.log.Error((object)y.Message);
			}
		}

		private static ILog log = LogManager.GetLogger(typeof(T));

		public static void LoadFile(ref T rc, string fileName)
		{
			LoadFile(ref rc, fileName, create: true);
		}

		public static void LoadFile(ref T rc, string fileName, bool create)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(Strings.LSet("正在加载: " + Path.GetFileName(fileName), 40));
			Console.ForegroundColor = ConsoleColor.DarkGray;
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
			{
				ValidationType = ValidationType.Schema
			};
			xmlReaderSettings.ValidationEventHandler += ((_Closure_0024__._0024I4_002D0 != null) ? _Closure_0024__._0024I4_002D0 : (_Closure_0024__._0024I4_002D0 = delegate(object x, ValidationEventArgs y)
			{
				log.Error((object)y.Message);
			}));
			try
			{
				using FileStream input = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				using (XmlReader xmlReader = XmlReader.Create(input, xmlReaderSettings))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
					rc = Conversions.ToGenericParameter<T>(xmlSerializer.Deserialize(xmlReader));
					if (create)
					{
						rc.CreateIndex();
					}
				}
				Console.Write("\r\r");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				throw ex2;
			}
		}
	}
}
