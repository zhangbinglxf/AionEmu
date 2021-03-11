using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
    public class SM_PLAY_OK : LsServerPacket
    {
        private SessionKey _key;

        private byte _serverId;

        public SM_PLAY_OK(SessionKey key, byte serverId)
        {
            _key = key;
            _serverId = serverId;
        }

        protected override void writeImpl(AionConnection con)
        {
            writeD(_key.PlayerOk1);
            writeD(_key.PlayerOk2);

            //if (Operators.CompareString(LoginConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
            //{
            writeC(_serverId);
            writeB(14);
            //}
        }
    }
}
