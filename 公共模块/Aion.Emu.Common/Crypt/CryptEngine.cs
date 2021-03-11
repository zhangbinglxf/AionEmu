namespace Aion.Emu.Common.ncrypt
{
	public class CryptEngine
	{
		private byte[] key = new byte[16]
		{
			107,
			96,
			203,
			91,
			130,
			206,
			144,
			177,
			204,
			43,
			108,
			85,
			108,
			108,
			108,
			108
		};

		private bool updatedKey = false;

		private BlowfishCipher cipher;

		public CryptEngine()
		{
			cipher = new BlowfishCipher(key);
		}

		public void updateKey(byte[] newKey)
		{
			key = newKey;
		}

		public bool decrypt(ref byte[] data, int offset, int length)
		{
			cipher.decipher(data, offset, length);
			return verifyChecksum(ref data, offset, length);
		}

		public int encrypt(ref byte[] data, int offset, int length)
		{
			if (!updatedKey)
			{
				encXORPass(ref data, offset, length, 1234);
				cipher.cipher(ref data, offset, length);
				cipher.updateKey(key);
				updatedKey = true;
			}
			else
			{
				appendChecksum(ref data, offset, length);
				cipher.cipher(ref data, offset, length);
			}
			return length;
		}

		private bool verifyChecksum(ref byte[] data, int offset, int length)
		{
			if (((uint)length & 3u) != 0 || length <= 4)
			{
				return false;
			}
			long num = 0L;
			int num2 = length - 4;
			int i;
			long num3;
			for (i = offset; i < num2; i += 4)
			{
				num3 = data[i] & 0xFF;
				num3 |= (uint)((data[i + 1] << 8) & 0xFF00);
				num3 |= (uint)((data[i + 2] << 16) & 0xFF0000);
				num3 |= (uint)((data[i + 3] << 24) & -16777216);
				num ^= num3;
			}
			num3 = data[i] & 0xFF;
			num3 |= (uint)((data[i + 1] << 8) & 0xFF00);
			num3 |= (uint)((data[i + 2] << 16) & 0xFF0000);
			num3 |= (data[i + 3] << 24) & 0xFF000000u;
			num3 = data[i] & 0xFF;
			num3 |= (uint)((data[i + 1] << 8) & 0xFF00);
			num3 |= (uint)((data[i + 2] << 16) & 0xFF0000);
			num3 |= (data[i + 3] << 24) & 0xFF000000u;
			return num == 0;
		}

		private void appendChecksum(ref byte[] raw, int offset, int length)
		{
			long num = 0L;
			int num2 = length - 4;
			int i;
			long num3;
			for (i = offset; i < num2; i += 4)
			{
				num3 = raw[i] & 0xFF;
				num3 |= (uint)((raw[i + 1] << 8) & 0xFF00);
				num3 |= (uint)((raw[i + 2] << 16) & 0xFF0000);
				num3 |= (raw[i + 3] << 24) & 0xFF000000u;
				num ^= num3;
			}
			num3 = raw[i] & 0xFF;
			num3 |= (uint)((raw[i + 1] << 8) & 0xFF00);
			num3 |= (uint)((raw[i + 2] << 16) & 0xFF0000);
			num3 |= (raw[i + 3] << 24) & 0xFF000000u;
			raw[i] = (byte)(num & 0xFF);
			raw[i + 1] = (byte)((num >> 8) & 0xFF);
			raw[i + 2] = (byte)((num >> 16) & 0xFF);
			raw[i + 3] = (byte)((num >> 24) & 0xFF);
		}

		public void encXORPass(ref byte[] data, int offset, int length, int key)
		{
			int num = length - 8;
			int num2 = 4 + offset;
			int num3 = key;
			while (num2 < num)
			{
				int num4 = data[num2] & 0xFF;
				num4 |= (data[num2 + 1] & 0xFF) << 8;
				num4 |= (data[num2 + 2] & 0xFF) << 16;
				num4 |= (data[num2 + 3] & 0xFF) << 24;
				num3 += num4;
				num4 ^= num3;
				data[num2++] = (byte)((uint)num4 & 0xFFu);
				data[num2++] = (byte)((uint)(num4 >> 8) & 0xFFu);
				data[num2++] = (byte)((uint)(num4 >> 16) & 0xFFu);
				data[num2++] = (byte)((uint)(num4 >> 24) & 0xFFu);
			}
			data[num2++] = (byte)((uint)num3 & 0xFFu);
			data[num2++] = (byte)((uint)(num3 >> 8) & 0xFFu);
			data[num2++] = (byte)((uint)(num3 >> 16) & 0xFFu);
			data[num2] = (byte)((uint)(num3 >> 24) & 0xFFu);
		}
	}
}
