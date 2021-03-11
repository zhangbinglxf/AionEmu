using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.ChatService
{
	public class CM_CHANNEL_REQUEST : CsClientPacket
	{
		private short channelIndex;

		private byte[] channelIdentifier;

		public CM_CHANNEL_REQUEST(int opcode)
			: base(opcode)
		{
		}

		protected override void readImpl()
		{
			readC();
			readH();
			channelIndex = readH();
			checked
			{
				if (Operators.CompareString(ChatConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
				{
					readB(18);
					int length = readH() * 2;
					channelIdentifier = readB(length);
					readD();
				}
				else
				{
					int length2 = readH() * 2;
					channelIdentifier = readB(length2);
				}
			}
		}

		protected override void runImpl()
		{
			GameService.RegisterPlayerChannel(base.Client, channelIndex, channelIdentifier);
		}
	}
}
