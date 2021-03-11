using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.VisualBasic;

namespace Aion.Emu.LoginService
{
	[Serializable]
	public class PayRewardTemplate
	{
		[XmlElement("reward_group", Form = XmlSchemaForm.Unqualified)]
		public List<PayRewardGroup> rewardGroup;


		[XmlAttribute]
		public int id
		{
			get;
			set;
		}

		[XmlAttribute]
		public string pay_name
		{
			get;
			set;
		}

		[XmlAttribute]
		public PayRewardType type
		{
			get;
			set;
		}

		[XmlAttribute]
		public string start_time
		{
			get;
			set;
		}

		[XmlAttribute]
		public string end_time
		{
			get;
			set;
		}

		[XmlAttribute]
		public string msg
		{
			get;
			set;
		}

		[XmlIgnore]
		public string announce
		{
			get;
			set;
		}

		public bool IsActive()
		{
			if (DateAndTime.Now.Ticks > DateTime.Parse(start_time).Ticks)
			{
				return DateAndTime.Now.Ticks < DateTime.Parse(end_time).Ticks;
			}
			return false;
		}
	}
}
