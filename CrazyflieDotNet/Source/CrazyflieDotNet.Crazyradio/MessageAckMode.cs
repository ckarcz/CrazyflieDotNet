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

namespace CrazyflieDotNet.Crazyradio
{
	/// <summary>
	///   The mode in which Crazyradio will handle packet acknowledgement (ACK) from Crazyflie quadcopters.
	/// </summary>
	public enum MessageAckMode
	{
		/// <summary>
		///   The Crazyradio will automatically wait for acknowledgement packets after sending messages.
		/// </summary>
		AutoAckOn = 0,

		/// <summary>
		///   The Crazyradio will not wait for acknowledgement packets after sending messages. There will be no garauntee that the messages are received.
		/// </summary>
		AutoAckOff
	}
}