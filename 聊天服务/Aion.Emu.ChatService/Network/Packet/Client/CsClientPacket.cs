using System;
using Aion.Emu.Common;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.ChatService
{
	public abstract class CsClientPacket : BaseClientPacket<AionConnection>
	{
		public CsClientPacket(int opcode)
			: base(opcode)
		{
		}

		public bool Read()
		{
			try
			{
				readImpl();
				if (base.RemainingLength > 0)
				{
					log.DebugFormat("数据包 {0} 剩余 {1} 字节没有处理", (object)GetType().Name, (object)base.RemainingLength);
				}
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public void Run()
		{
			try
			{
				runImpl();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				ProjectData.ClearProjectError();
			}
		}

		protected abstract void readImpl();

		protected abstract void runImpl();

		protected void SendPacket(CsServerPacket packet)
		{
			base.Client.SendPacket(packet);
		}

		public override BasePacket ClonePacket()
		{
			try
			{
				return (BasePacket)MemberwiseClone();
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				BasePacket result = null;
				ProjectData.ClearProjectError();
				return result;
			}
		}
	}
}
