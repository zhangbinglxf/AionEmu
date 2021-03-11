using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.ChatService
{
	public class CM_QUIT_CHANNEL : CsClientPacket
	{
		private int channelId;

		public CM_QUIT_CHANNEL(int opcode)
			: base(opcode)
		{
		}

		protected override void readImpl()
		{
			if (Operators.CompareString(ChatConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
			{
				readB(19);
			}
			else
			{
				readC();
				readH();
			}
			channelId = readD();
		}

		protected override void runImpl()
		{
			base.Client.ChatClient.RemoveChannel(channelId);
		}
	}
}
