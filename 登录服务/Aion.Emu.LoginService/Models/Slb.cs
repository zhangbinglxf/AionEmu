using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class Slb : IComparable<Slb>
	{
		private int _count;

		private string _ip;

		private byte[] _Address;

		public string SlbIP => _ip;

		public byte[] Address => _Address;

		public bool IsActive
		{
			get
			{
				try
				{
					return new Ping().Send(_ip, 30).Status == IPStatus.Success;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					bool result = false;
					ProjectData.ClearProjectError();
					return result;
				}
			}
		}

		public Slb(string ip)
		{
			_ip = ip;
			_Address = IPAddress.Parse(ip).GetAddressBytes();
		}

		public void Increment()
		{
			Interlocked.Increment(ref _count);
		}

		public void Decrement()
		{
			Interlocked.Decrement(ref _count);
		}

		public int CompareTo(Slb other)
		{
			return _count.CompareTo(other._count);
		}

		int IComparable<Slb>.CompareTo(Slb other)
		{
			//ILSpy generated this explicit interface implementation from .override directive in CompareTo
			return this.CompareTo(other);
		}
	}
}
