/* 
 *										 _ _  _     
 *			   ____ ___  ___  __________(_|_)(_)____
 *			  / __ `__ \/ _ \/ ___/ ___/ / _ \/ ___/
 *			 / / / / / /  __(__  |__  ) /  __/ /    
 *			/_/ /_/ /_/\___/____/____/_/\___/_/  
 *
 *				Copyright 2013 - www.messier.com
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

#region Imports

using System;

#endregion Imports

namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public sealed class Packet
	{
		public static readonly Packet PingPacket = new Packet(new PacketHeader(Port.All));

		private byte[] _cachedPacketBytes;

		public Packet(PacketHeader header)
			: this(header, null)
		{
		}

		public Packet(PacketHeader header, byte[] data)
		{
			Header = header;
			Data = data;
		}

		public PacketHeader Header { get; private set; }

		public byte[] Data { get; private set; }

		public byte[] GetPacketBytes()
		{
			return _cachedPacketBytes ?? (_cachedPacketBytes = GeneratePacketBytes());
		}

		private byte[] GeneratePacketBytes()
		{
			if (Header == null)
			{
				throw new CRTPException("CRTP packet header is null");
			}

			var packetBytesArraySize = (Header != null ? 1 : 0) + (Data != null ? Data.Length : 0);
			var packetBytesArray = new byte[packetBytesArraySize];

			packetBytesArray[0] = Header.HeaderByte;

			if (Data != null && Data.Length > 0)
			{
				Array.Copy(Data, 0, packetBytesArray, 1, Data.Length);
			}

			return packetBytesArray;
		}
	}
}