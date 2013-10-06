#region Imports

using System;

#endregion Imports

namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public sealed class CRTPPacket
	{
		public static readonly CRTPPacket PingPacket = new CRTPPacket(new CRTPPacketHeader(CRTPPort.All));

		private byte[] _cachedPacketBytes;

		public CRTPPacket(byte[] packetBytes)
		{
			if (packetBytes == null)
			{
				throw new ArgumentNullException("packetBytes");
			}

			if (packetBytes.Length < 1)
			{
				throw new ArgumentException("CRTP packet must contain at least one byte (header).");
			}

			var packetHeader = new CRTPPacketHeader(packetBytes[0]);
			var packetData = new byte[packetBytes.Length - 1];

			if (packetBytes.Length > 1)
			{
				Array.Copy(packetBytes, 1, packetData, 0, packetData.Length);
			}

			Header = packetHeader;
			Data = packetData;

			_cachedPacketBytes = packetBytes;
		}

		public CRTPPacket(CRTPPacketHeader header)
			: this(header, null)
		{
		}

		public CRTPPacket(CRTPPacketHeader header, byte[] data)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}

			Header = header;
			Data = data ?? new byte[0];
		}

		public CRTPPacketHeader Header { get; private set; }

		public byte[] Data { get; private set; }

		public byte[] PacketBytes
		{
			get { return _cachedPacketBytes ?? (_cachedPacketBytes = GetPacketBytes(this)); }
		}

		public static byte[] GetPacketBytes(CRTPPacket packet)
		{
			if (packet == null)
			{
				throw new ArgumentNullException("packet");
			}

			if (packet.Header == null)
			{
				throw new CRTPException("CRTP packet header is null");
			}

			var packetBytesArraySize = (packet.Header != null ? 1 : 0) + (packet.Data != null ? packet.Data.Length : 0);
			var packetBytesArray = new byte[packetBytesArraySize];

			packetBytesArray[0] = packet.Header.HeaderByte;

			if (packet.Data != null && packet.Data.Length > 0)
			{
				Array.Copy(packet.Data, 0, packetBytesArray, 1, packet.Data.Length);
			}

			return packetBytesArray;
		}
	}
}