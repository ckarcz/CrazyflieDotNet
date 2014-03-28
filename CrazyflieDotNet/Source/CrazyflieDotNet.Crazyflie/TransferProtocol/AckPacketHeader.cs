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

#endregion Imports

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public class AckPacketHeader
		: IAckPacketHeader
	{
		private readonly byte[] _bytesCached;

		public AckPacketHeader(byte packetHeaderByte)
		{
			// todo - set properties intead of caching bytes
			_bytesCached = new[] {packetHeaderByte};
		}

		public AckPacketHeader(MessageAckRetryCount retryCount, bool powerDetector, bool ackRecieved)
		{
			RetryCount = retryCount;
			PowerDetector = powerDetector;
			AckRecieved = ackRecieved;
		}

		public MessageAckRetryCount RetryCount { get; private set; }

		public bool PowerDetector { get; private set; }

		public bool AckRecieved { get; private set; }

		public byte[] GetBytes()
		{
			// Header Format (1 byte):
			//  7  6  5  4  3  2  1  0
			// [# Retries ][Res.][P][A]
			// Res. = reserved for transfer layer.
			// P = Power detector triggered
			// A = Ack received

			// todo remove this line once properties are able to be set from input byte
			if (_bytesCached != null)
				return _bytesCached;

			var retryCountByte = (byte)RetryCount;
			var retryCountByteAnd15 = (byte)(retryCountByte & 0x0F);
			var portByteAnd15LeftShifted4 = (byte)(retryCountByteAnd15 << 4);
			var reservedLeftShifted2 = (byte)(0x03 << 2);
			var powerDetectorByte = Convert.ToByte(PowerDetector);
			var powerDetectorByteAnd1 = (byte)(powerDetectorByte & 0x01);
			var powerDetectorByteLeftShifted1 = (byte)(powerDetectorByteAnd1 << 1);
			var ackRecievedByte = Convert.ToByte(AckRecieved);
			var ackRecievedByteAnd1 = (byte)(ackRecievedByte & 0x01);

			return new[]{(byte)(portByteAnd15LeftShifted4 | reservedLeftShifted2 | powerDetectorByteLeftShifted1 | ackRecievedByteAnd1)};
		}
	}
}