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

using System;

#endregion Imports

namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public sealed class CRTPAckPacket
	{
		private byte[] _cachedPacketBytes;

		public CRTPAckPacket(byte[] packetBytes)
		{
			if (packetBytes == null)
			{
				throw new ArgumentNullException("packetBytes");
			}

			if (packetBytes.Length < 1)
			{
				throw new ArgumentException("CRTP packet must contain at least one byte (header).");
			}

			var packetHeader = new CRTPAckPacketHeader(packetBytes[0]);
			var payload = new byte[packetBytes.Length - 1];

			if (packetBytes.Length > 1)
			{
				Array.Copy(packetBytes, 1, payload, 0, payload.Length);
			}

			Header = packetHeader;
			Payload = payload;

			_cachedPacketBytes = packetBytes;
		}

		public CRTPAckPacket(CRTPAckPacketHeader header)
			: this(header, null)
		{
		}

		public CRTPAckPacket(CRTPAckPacketHeader header, byte[] payload)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			Header = header;
			Payload = payload ?? new byte[0];
		}

		public CRTPAckPacketHeader Header { get; private set; }

		public byte[] Payload { get; private set; }

		public byte[] PacketBytes
		{
			get { return _cachedPacketBytes ?? (_cachedPacketBytes = GetPacketBytes(this)); }
		}

		public static byte[] GetPacketBytes(CRTPAckPacket packet)
		{
			if (packet == null)
			{
				throw new ArgumentNullException("packet");
			}

			if (packet.Header == null)
			{
				throw new CRTPException("CRTP packet header is null");
			}

			var packetBytesArraySize = (packet.Header != null ? 1 : 0) + (packet.Payload != null ? packet.Payload.Length : 0);
			var packetBytesArray = new byte[packetBytesArraySize];

			packetBytesArray[0] = packet.Header.HeaderByte;

			if (packet.Payload != null && packet.Payload.Length > 0)
			{
				Array.Copy(packet.Payload, 0, packetBytesArray, 1, packet.Payload.Length);
			}

			return packetBytesArray;
		}
	}
}