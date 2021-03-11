using System;

namespace Aion.Emu.Common
{
	public class MyPropertyAttribute : Attribute
	{
		private string _key;

		private string _defaultValue;

		public string Key
		{
			get
			{
				return _key;
			}
			set
			{
				_key = value;
			}
		}

		public string DefaultValue
		{
			get
			{
				return _defaultValue;
			}
			set
			{
				_defaultValue = value;
			}
		}

		public MyPropertyAttribute()
		{
			_defaultValue = "";
		}
	}
}
