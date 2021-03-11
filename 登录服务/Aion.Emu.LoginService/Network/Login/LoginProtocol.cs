using System;
using System.Collections.Generic;
using Aion.Emu.Common;
using Aion.Emu.Common.ncrypt;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class LoginProtocol : MessageProtocol
	{
		private CryptEngine crypt;

		private Random Rnd;

		private byte[] _key;

		public override byte[] Keys => _key;

		public LoginProtocol(PacketHandleFactory handle)
			: base(handle)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			Rnd = new Random();
			_key = new byte[16];
			crypt = new CryptEngine();
			Rnd.NextBytes(_key);
			crypt.updateKey(_key);
		}

		protected override bool ReadPacket(ref List<BasePacket> packets)
		{
			read.Position = 0L;
			if (read.Length < 2)
			{
				return false;
			}
			checked
			{
				try
				{
					byte[] array = new byte[2];
					read.Read(array, 0, 2);
					short num = BitConverter.ToInt16(array, 0);
					if (read.Length < num)
					{
						return false;
					}
					array = new byte[num - 3 + 1];
					read.Read(array, 0, array.Length);
					if (crypt.decrypt(ref array, 0, array.Length))
					{
						byte op = array[0];
						byte[] array2 = new byte[array.Length - 2 + 1];
						Array.Copy(array, 1, array2, 0, array2.Length);
						BasePacket packet = base.Handle.GetPacket(op, array2);
						if (!Information.IsNothing(packet))
						{
							packets.Add(packet);
						}
					}
					else
					{
						log.Error((object)"数据解密错误!");
					}
					if (read.Length - read.Position > 2)
					{
						array = new byte[(int)(read.Length - read.Position - 1) + 1];
						read.Read(array, 0, array.Length);
						read.Dispose();
						ReSet();
						read.Write(array, 0, array.Length);
						return true;
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					log.Error((object)ex2);
					ProjectData.ClearProjectError();
				}
				ReSet();
				return false;
			}
		}

		public override void EnCrypt(ref byte[] packetData, int v, int length)
		{
			crypt.encrypt(ref packetData, v, length);
		}
	}
}
