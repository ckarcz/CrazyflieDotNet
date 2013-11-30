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

namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public sealed class CRTPOutPacketHeader
	{
		private byte? _packetHeaderByteCached;

		public CRTPOutPacketHeader(byte packetHeaderByte)
		{
			_packetHeaderByteCached = packetHeaderByte;
		}

		public CRTPOutPacketHeader(CRTPPort port, CRTPChannel channel = DefaultChannel)
		{
			Port = port;
			Channel = channel;
		}

		public CRTPChannel Channel { get; private set; }

		public CRTPPort Port { get; private set; }

		public const CRTPChannel DefaultChannel = CRTPChannel.Channel0;

		internal byte HeaderByte
		{
			get { return (_packetHeaderByteCached ?? (_packetHeaderByteCached = GetByte(this))).Value; }
		}

		public static byte GetByte(CRTPOutPacketHeader packetHeader)
		{
			// Header Format (1 byte):
			//  7  6  5  4  3  2  1  0
			// [   Port   ][Res. ][Ch.]
			// Res. = reserved for transfer layer. not much info on this...

			var portByte = (byte)(packetHeader.Port);
			var portByteAnd15 = (byte)(portByte & 0x0F);
			var portByteAnd15LeftShifted4 = (byte)(portByteAnd15 << 4);
			var reservedLeftShifted2 = (byte)(0x03 << 2);
			var channelByte = (byte)(packetHeader.Channel);
			var channelByteAnd3 = (byte)(channelByte & 0x03);

			return (byte)(portByteAnd15LeftShifted4 | reservedLeftShifted2 | channelByteAnd3);
		}
	}
}