using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Aion.Emu.LoginService
{
	[Serializable]
	[XmlType(AnonymousType = true)]
	[XmlRoot(ElementName = "pay_rewards", Namespace = "", IsNullable = false)]
	public class PayRewardTemplates : IDataTemplate
	{
		[XmlElement("pay_reward", Form = XmlSchemaForm.Unqualified)]
		public List<PayRewardTemplate> rewards;

		[XmlIgnore]
		public Dictionary<int, PayRewardTemplate> rewardForId;

		public int Count => rewards.Count;

		[IndexerName("Get")]
		public PayRewardTemplate this[int id]
		{
			get
			{
				if (rewardForId.ContainsKey(id))
				{
					return rewardForId[id];
				}
				return null;
			}
		}

		public PayRewardTemplates()
		{
			rewardForId = new Dictionary<int, PayRewardTemplate>();
		}

		public void CreateIndex()
		{
			foreach (PayRewardTemplate reward in rewards)
			{
				rewardForId.Add(reward.id, reward);
				string text = "";
				switch (reward.type)
				{
				case PayRewardType.FIRST_PAY:
					foreach (RewardItemPart part in reward.rewardGroup[0].parts)
					{
						text += $"[item:{part.itemId}]x{part.count},";
					}
					break;
				case PayRewardType.APPOINT_PAY:
					foreach (PayRewardGroup item in reward.rewardGroup)
					{
						text += $"充值{item.price}点卷可获得:";
						foreach (RewardItemPart part2 in item.parts)
						{
							text += $"[item:{part2.itemId}]x{part2.count},";
						}
					}
					break;
				case PayRewardType.TOTAL_PAY:
					foreach (PayRewardGroup item2 in reward.rewardGroup)
					{
						text += $"累计充值满{item2.price}点卷可获得:";
						foreach (RewardItemPart part3 in item2.parts)
						{
							text += $"[item:{part3.itemId}]x{part3.count},";
						}
					}
					break;
				case PayRewardType.TOTAL_CONSUM:
					foreach (PayRewardGroup item3 in reward.rewardGroup)
					{
						text += $"累计消费满{item3.price}点卷可获得:";
						foreach (RewardItemPart part4 in item3.parts)
						{
							text += $"[item:{part4.itemId}]x{part4.count},";
						}
					}
					break;
				}
				text = text.TrimEnd(',');
				text += ".";
				reward.announce = reward.msg + text;
			}
		}

		void IDataTemplate.CreateIndex()
		{
			//ILSpy generated this explicit interface implementation from .override directive in CreateIndex
			this.CreateIndex();
		}
	}
}
