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

namespace CrazyflieDotNet.Crazyradio
{
	/// <summary>
	///   Chosen defaults values for the Crazyradio.
	///   These might be the actual defaults for the Crazyradio, but there's no
	///   way to tell. So on initiliazation of Crazyradio driver instance, set all defaults.
	/// </summary>
	public static class CrazyradioDefaults
	{
		/// <summary>
		///   The default mode the Crazyradio uses. (NormalFlightMode).
		/// </summary>
		public static readonly RadioMode DefaultMode = RadioMode.NormalFlightMode;

		/// <summary>
		///   The default channel the Crazyradio uses. (Channel2).
		/// </summary>
		public static readonly RadioChannel DefaultChannel = RadioChannel.Channel2;

		/// <summary>
		///   The default address the Crazyradio uses. (0xE7-0xE7-0xE7-0xE7-0xE7).
		/// </summary>
		public static readonly RadioAddress DefaultAddress = new RadioAddress(0xE7, 0xE7, 0xE7, 0xE7, 0xE7);

		/// <summary>
		///   The default data rate the Crazyradio uses. (DataRate2Mps).
		/// </summary>
		public static readonly RadioDataRate DefaultDataRate = RadioDataRate.DataRate2Mps;

		/// <summary>
		///   The default power level the Crazyradio uses. (TODO).
		/// </summary>
		public static readonly RadioPowerLevel DefaultPowerLevel = RadioPowerLevel.ZeroDbm;

		/// <summary>
		///   The default ACK mode the Crazyflie uses. (AutoAckOn).
		/// </summary>
		public static MessageAckMode DefaultAckMode = MessageAckMode.AutoAckOn;

		/// <summary>
		///   The default ACK retry count the Crazyradio uses. (Retry3Times).
		/// </summary>
		public static readonly MessageAckRetryCount DefaultAckRetryCount = MessageAckRetryCount.Retry3Times;

		/// <summary>
		///   The default ACK retry delay the Crazyradio uses. (UseAckPacketMethod).
		/// </summary>
		public static readonly MessageAckRetryDelay DefaultAckRetryDelay = MessageAckRetryDelay.UseAckPacketMethod;

		/// <summary>
		///   The default ACK payload length the Crazyradio uses. (Length32Bytes).
		/// </summary>
		public static readonly MessageAckPayloadLength DefaultAckPayloadLength = MessageAckPayloadLength.Length32Bytes;
	}
}