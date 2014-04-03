/* 
 *						 _ _  _     
 *		       ____ ___  ___  __________(_|_)(_)____
 *		      / __ `__ \/ _ \/ ___/ ___/ / _ \/ ___/
 *		     / / / / / /  __(__  |__  ) /  __/ /    
 *		    /_/ /_/ /_/\___/____/____/_/\___/_/  
 *
 *	     Copyright 2013 - Messier/Chris Karcz - ckarcz@gmail.com
 *
 *	This Source Code Form is subject to the terms of the Mozilla Public
 *	License, v. 2.0. If a copy of the MPL was not distributed with this
 *	file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

#region Imports

using System;
using CrazyflieDotNet.Crazyradio.Driver;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public class CrazyradioMessenger
		: ICrazyflieMessenger
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

		#region ICrazyflieMessenger Members

		public IAckPacket SendMessage(IPacket packet)
		{
			var packetBytes = packet.GetBytes();
			var responseBytes = _crazyradioDriver.SendData(packetBytes);

			return new AckPacket(responseBytes);
		}

		#endregion
	}
}