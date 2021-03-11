using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Aion.Emu.LoginService
{
	[Serializable]
	public class RewardItemPart
	{
		[XmlAttribute]
		public int itemId
		{
			get;
			set;
		}

		[XmlAttribute]
		public int count
		{
			get;
			set;
		}
	}
}
