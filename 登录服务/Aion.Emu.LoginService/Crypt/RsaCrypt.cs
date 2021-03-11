using ManagedOpenSsl.NetCore.Core;
using ManagedOpenSsl.NetCore.Crypto;
using System;


namespace Aion.Emu.LoginService
{
	public class RsaCrypt
	{
		private RSA _rsa;

		private byte[] encryptedModulus;

		public byte[] Modulus => encryptedModulus;

		public RsaCrypt(RSA rsa)
		{
			_rsa = rsa;
			_rsa.GenerateKeys(1024, new BigNumber(65537u), null, null);
			encryptModulus();
		}

		private void encryptModulus()
		{
			
			encryptedModulus = new BigNumber(_rsa.PublicModulus);
			if ((encryptedModulus.Length == 129) & (encryptedModulus[0] == 0))
			{
				byte[] destinationArray = new byte[128];
				Array.Copy(encryptedModulus, 1, destinationArray, 0, 128);
				encryptedModulus = destinationArray;
			}
			int num = 0;
			int num2;
			checked
			{
				do
				{
					byte b = encryptedModulus[num];
					encryptedModulus[num] = encryptedModulus[77 + num];
					encryptedModulus[77 + num] = b;
					num++;
				}
				while (num <= 3);
				num2 = 0;
			}
			do
			{
				encryptedModulus[num2] = (byte)(encryptedModulus[num2] ^ encryptedModulus[checked(64 + num2)]);
				num2 = checked(num2 + 1);
			}
			while (num2 <= 63);
			int num3 = 0;
			do
			{
				encryptedModulus[checked(13 + num3)] = (byte)checked(encryptedModulus[13 + num3] ^ encryptedModulus[52 + num3]);
				num3 = checked(num3 + 1);
			}
			while (num3 <= 3);
			int num4 = 0;
			do
			{
				encryptedModulus[checked(64 + num4)] = (byte)(encryptedModulus[checked(64 + num4)] ^ encryptedModulus[num4]);
				num4 = checked(num4 + 1);
			}
			while (num4 <= 63);
		}

		public object Decrypt(byte[] data)
		{
			return _rsa.PrivateDecrypt(data, (RSA.Padding)3);
		}
	}
}
