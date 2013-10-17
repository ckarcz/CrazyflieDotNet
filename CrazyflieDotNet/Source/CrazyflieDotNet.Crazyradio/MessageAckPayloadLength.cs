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
	///   The acknowledgement packet (ACK) length. This sets the auto retry delay for the Crazyradio to retry sending messages it did get receive an ACK for.
	/// </summary>
	public enum MessageAckPayloadLength
	{
		UseAckRetryDelayMethod = -1,

		Length0Bytes = 0x80,

		Length1Byte = 0x81,

		Length2Byte = 0x82,

		Length3Byte = 0x83,

		Length4Byte = 0x84,

		Length5Byte = 0x85,

		Length6Byte = 0x86,

		Length7Byte = 0x87,

		Length8Byte = 0x88,

		Length9Byte = 0x89,

		Length10Bytes = 0x8A,

		Length11Bytes = 0x8B,

		Length12Bytes = 0x8C,

		Length13Bytes = 0x8D,

		Length14Bytes = 0x8E,

		Length15Bytes = 0x8F,

		Length16Bytes = 0x90,

		Length17Bytes = 0x91,

		Length18Bytes = 0x92,

		Length19Bytes = 0x93,

		Length20Bytes = 0x94,

		Length21Bytes = 0x95,

		Length22Bytes = 0x96,

		Length23Bytes = 0x97,

		Length24Bytes = 0x98,

		Length25Bytes = 0x99,

		Length26Bytes = 0x9A,

		Length27Bytes = 0x9B,

		Length28Bytes = 0x9C,

		Length29Bytes = 0x9D,

		Length30Bytes = 0x9E,

		Length31Bytes = 0x9F,

		Length32Bytes = 0xA0,
	}
}