/* 
 *						 _ _  _     
 *		       ____ ___  ___  __________(_|_)(_)____
 *		      / __ `__ \/ _ \/ ___/ ___/ / _ \/ ___/
 *		     / / / / / /  __(__  |__  ) /  __/ /    
 *		    /_/ /_/ /_/\___/____/____/_/\___/_/  
 *
 *		    Copyright 2013 - http://www.messier.com
 *
 *	This Source Code Form is subject to the terms of the Mozilla Public
 *	License, v. 2.0. If a copy of the MPL was not distributed with this
 *	file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

#region Imports

using CrazyflieDotNet.Crazyradio;
using System;

#endregion Imports

namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public class CrazyradioMessenger
		: ICrazyradioMessenger
	{
		private readonly ICrazyradioDriver _crazyradioDriver;

		public CrazyradioMessenger(ICrazyradioDriver crazyradioDriver)
		{
			if (crazyradioDriver == null)
			{
				throw new ArgumentNullException("crazyradioDriver");
			}

			_crazyradioDriver = crazyradioDriver;
		}

		public CRTPAckPacket SendMessage(CRTPDataPacket dataPacket)
		{
			var responseBytes = _crazyradioDriver.SendData(dataPacket.PacketBytes);

			return new CRTPAckPacket(responseBytes);
		}
	}
}