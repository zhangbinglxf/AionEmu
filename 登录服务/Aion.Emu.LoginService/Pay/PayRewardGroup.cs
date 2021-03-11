using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Aion.Emu.LoginService
{
	[Serializable]
	public class PayRewardGroup
	{

		[XmlElement("reward_part", Form = XmlSchemaForm.Unqualified)]
		public List<RewardItemPart> parts;

		[XmlAttribute]
		public int price
		{
			get;
			set;
		}
	}
}
