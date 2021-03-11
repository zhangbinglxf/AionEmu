using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.Common
{
	public class ConfigurableProcessor
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<FieldInfo, bool> _0024I7_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			internal bool _Lambda_0024__7_002D0(FieldInfo x)
			{
				return x.GetCustomAttributes(inherit: false).Length == 1;
			}
		}

		private static ILog log = LogManager.GetLogger(typeof(ConfigurableProcessor));

		private static Hashtable properties = new Hashtable();

		public static void Process(object o, string path)
		{
			if (!o.GetType().IsClass)
			{
				throw new NotSupportedException("传入类型不是一个有效的实例");
			}
			LoadFile(path);
			SetTargetValue(RuntimeHelpers.GetObjectValue(o));
		}

		private static void LoadFile(string filePath)
		{
			try
			{
				string[] array = File.ReadAllLines(filePath);
				foreach (string text in array)
				{
					if (!text.StartsWith("#") && !string.IsNullOrEmpty(text) && text.Contains("="))
					{
						PutValue(text);
					}
				}
				log.Info((object)("读取配置: " + filePath));
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				log.WarnFormat("配置文件: {0} 不存在!", (object)filePath);
				ProjectData.ClearProjectError();
			}
		}

		private static void PutValue(string line)
		{
			string[] array = line.Split('=');
			string text = array[0].Trim();
			string text2 = array[1].Trim();
			if (properties.ContainsKey(text))
			{
				log.WarnFormat("重复配置:{0} = {1}", (object)text, (object)text2);
			}
			else
			{
				properties.Add(array[0].Trim(), array[1].Trim());
			}
		}

		private static void SetTargetValue(object o)
		{
			Type type = o.GetType();
			FieldInfo[] array = type.GetFields().Where((_Closure_0024__._0024I7_002D0 != null) ? _Closure_0024__._0024I7_002D0 : (_Closure_0024__._0024I7_002D0 = (FieldInfo x) => x.GetCustomAttributes(inherit: false).Length == 1)).ToArray();
			foreach (FieldInfo fieldInfo in array)
			{
				MyPropertyAttribute myPropertyAttribute = (MyPropertyAttribute)fieldInfo.GetCustomAttributes(inherit: false)[0];
				string expression = myPropertyAttribute.DefaultValue;
				if (properties.ContainsKey(myPropertyAttribute.Key))
				{
					expression = Conversions.ToString(properties[myPropertyAttribute.Key]);
				}
				else
				{
					log.WarnFormat("默认:{1} = {2}", (object)type.Name, (object)myPropertyAttribute.Key, (object)myPropertyAttribute.DefaultValue);
				}
				fieldInfo.SetValue(RuntimeHelpers.GetObjectValue(o), RuntimeHelpers.GetObjectValue(Conversion.CTypeDynamic(expression, fieldInfo.FieldType)));
			}
		}
	}
}
