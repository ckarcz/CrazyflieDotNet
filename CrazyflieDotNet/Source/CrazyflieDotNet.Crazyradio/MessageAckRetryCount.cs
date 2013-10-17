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
	///   The number of times to rety waiting for an acknowledgement packet (ACK) from the crazyflie.
	/// </summary>
	public enum MessageAckRetryCount
	{
		RetryNever = 0,

		Retry1Time = 1,

		Retry2Times = 2,

		Retry3Times = 3,

		Retry4Times = 4,

		Retry5Times = 5,

		Retry6Times = 6,

		Retry7Times = 7,

		Retry8Times = 8,

		Retry9Times = 9,

		Retry10Times = 10,

		Retry11Times = 11,

		Retry12Times = 12,

		Retry13Times = 13,

		Retry14Times = 14,

		Retry15Times = 15,
	}
}