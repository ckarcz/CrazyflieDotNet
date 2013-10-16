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

#region Imports

using CrazyflieDotNet.Crazyradio;
using System;

#endregion Imports

namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public sealed class CRTPAckPacketHeader
	{
		private byte? _bytesCached;

		public CRTPAckPacketHeader(byte packetHeaderByte)
		{
			throw new NotImplementedException();
		}

		public CRTPAckPacketHeader(MessageAckRetryCount retryCount, bool powerDetector, bool ackRecieved)
		{
			RetryCount = retryCount;
			PowerDetector = powerDetector;
			AckRecieved = ackRecieved;
		}

		public MessageAckRetryCount RetryCount { get; private set; }

		public bool PowerDetector { get; private set; }

		public bool AckRecieved { get; private set; }

		internal byte HeaderByte
		{
			get { return (_bytesCached ?? (_bytesCached = GetByte(this))).Value; }
		}

		public static byte GetByte(CRTPAckPacketHeader packetHeader)
		{
			// Header Format (1 byte):
			//  7  6  5  4  3  2  1  0
			// [# Retries ][Res.][P][A]
			// Res. = reserved for transfer layer.
			// P = Power detector triggered
			// A = Ack received

			var retryCountByte = (byte)(packetHeader.RetryCount);
			var retryCountByteAnd15 = (byte)(retryCountByte & 0x0F);
			var portByteAnd15LeftShifted4 = (byte)(retryCountByteAnd15 << 4);
			var reservedLeftShifted2 = (byte)(0x03 << 2);
			var powerDetectorByte = Convert.ToByte(packetHeader.PowerDetector);
			var powerDetectorByteAnd1 = (byte)(powerDetectorByte & 0x01);
			var powerDetectorByteLeftShifted1 = (byte)(powerDetectorByteAnd1 << 1);
			var ackRecievedByte = Convert.ToByte(packetHeader.AckRecieved);
			var ackRecievedByteAnd1 = (byte)(ackRecievedByte & 0x01);

			return (byte)(portByteAnd15LeftShifted4 | reservedLeftShifted2 | powerDetectorByteLeftShifted1 | ackRecievedByteAnd1);
		}
	}
}