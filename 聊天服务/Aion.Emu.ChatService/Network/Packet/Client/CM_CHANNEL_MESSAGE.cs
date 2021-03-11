using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.ChatService
{
	public class CM_CHANNEL_MESSAGE : CsClientPacket
	{
		private int channelId;

		private byte[] message;

		public CM_CHANNEL_MESSAGE(int opcode)
			: base(opcode)
		{
		}

		protected override void readImpl()
		{
			if (Operators.CompareString(ChatConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
			{
				readB(19);
				channelId = readD();
				readC();
			}
			else
			{
				readB(11);
				channelId = readD();
			}
			int length = checked(readH() * 2);
			message = readB(length);
		}

		protected override void runImpl()
		{
			GameService.SendMessage(base.Client.ServerId, base.Client.PlayerId, channelId, message);
		}
	}
}
