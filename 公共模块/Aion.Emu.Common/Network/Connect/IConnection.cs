namespace Aion.Emu.Common
{
	public interface IConnection
	{
		long ClientID
		{
			get;
		}

		string IP
		{
			get;
		}

		MessageProtocol MessageProtocol
		{
			get;
		}

		bool IsRuning
		{
			get;
		}

		void StartReceivePacket();

		void SendPacket(BasePacket pak);

		void PacketHandle(BasePacket pak);

		void Initialized();

		void Disconnect();
	}
}
