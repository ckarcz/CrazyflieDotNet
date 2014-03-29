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

namespace CrazyflieDotNet.Crazyradio.Driver
{
	/// <summary>
	///   The mode in which the Crazyradio USB dongle will operate.
	/// </summary>
	public enum RadioMode
	{
		/// <summary>
		///   This is normal flight.
		/// </summary>
		NormalFlightMode = 0,

		/// <summary>
		///   This is a testing mode in which a continuous non-modulated sine wave is emitted. This allows testing the radio. No packets are transmitted.
		/// </summary>
		ContinuousCarrierMode = 1
	}
}