using System;
using System.IdentityModel.Selectors;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class LoginStService : SecurityTokenManager
	{
		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		private static LoginStService instance;

		//private RsaSecurityToken key;

		public static LoginStService GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new LoginStService();
				}
				return instance;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(@lock);
				}
			}
		}

		private LoginStService()
		{
		}

		public override SecurityTokenProvider CreateSecurityTokenProvider(SecurityTokenRequirement tokenRequirement)
		{
			throw new NotImplementedException();
		}

		public override SecurityTokenSerializer CreateSecurityTokenSerializer(SecurityTokenVersion version)
		{
			throw new NotImplementedException();
		}

        public override SecurityTokenAuthenticator CreateSecurityTokenAuthenticator(SecurityTokenRequirement tokenRequirement, out SecurityTokenResolver outOfBandTokenResolver)
        {
            throw new NotImplementedException();
        }
    }
}
