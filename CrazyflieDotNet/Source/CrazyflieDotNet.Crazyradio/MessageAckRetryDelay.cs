/* 
 *										 _ _  _     
 *			   ____ ___  ___  __________(_|_)(_)____
 *			  / __ `__ \/ _ \/ ___/ ___/ / _ \/ ___/
 *			 / / / / / /  __(__  |__  ) /  __/ /    
 *			/_/ /_/ /_/\___/____/____/_/\___/_/  
 *
 *				Copyright 2013 - http://www.messier.com
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace CrazyflieDotNet.Crazyradio
{
	/// <summary>
	///   Possible values for the delay after sending a message to wait for an acknowledgement packet (ACK).
	/// </summary>
	public enum MessageAckRetryDelay
	{
		UseAckPacketMethod = -1,

		Wait250us = 0x00,

		Wait2500us = 0x01,

		Wait750us = 0x02,

		Wait1000us = 0x03,

		Wait1250us = 0x04,

		Wait1500us = 0x05,

		Wait1750us = 0x06,

		Wait2000s = 0x07,

		Wait2250us = 0x08,

		Wait7500us = 0x09,

		Wait2750us = 0x10,

		Wait3000us = 0x11,

		Wait3250us = 0x12,

		Wait3500us = 0x13,

		Wait3750us = 0x14,

		Wait4000us = 0x0F,
	}
}