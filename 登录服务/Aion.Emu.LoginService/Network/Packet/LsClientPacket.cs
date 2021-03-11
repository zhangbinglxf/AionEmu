using System;
using Aion.Emu.Common;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public abstract class LsClientPacket : BaseClientPacket<AionConnection>
	{
		private State _state;

		public LsClientPacket(int opcode, State state)
			: base(opcode)
		{
			_state = state;
		}

		public bool Read()
		{
			try
			{
				if (!IsValid())
				{
					log.WarnFormat("数据包 {0} 不适用于当前连接状态，已丢弃。", (object)(GetType().Name + " State:" + base.Client.State));
					return false;
				}
				readImpl();
				if (base.RemainingLength > 0)
				{
					log.WarnFormat("数据包 {0} 剩余 {1} 字节没有处理", (object)GetType().Name, (object)base.RemainingLength);
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

		protected void SendPacket(LsServerPacket packet)
		{
			base.Client.SendPacket(packet);
		}

		public override BasePacket ClonePacket()
		{
			try
			{
				return (BasePacket)MemberwiseClone();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				BasePacket result = null;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private bool IsValid()
		{
			return base.Client.State == _state;
		}
	}
}
