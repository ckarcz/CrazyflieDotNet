#region Imports

using System;
using System.Data;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public abstract class Packet<TPacketHeader>
		: Packet<TPacketHeader, IPacketPayload> where TPacketHeader : IPacketHeader
	{
		protected Packet(byte[] packetBytes)
			: base(packetBytes)
		{
		}

		protected Packet(TPacketHeader header)
			: base(header, null)
		{
		}

		protected override IPacketPayload ParsePayload(byte[] packetBytes)
		{
			return null;
		}
	}

	public abstract class Packet<TPacketHeader, TPacketPayload>
		: IPacket<TPacketHeader, TPacketPayload> where TPacketHeader : IProvideBytes where TPacketPayload : IProvideBytes
	{
		protected static readonly byte[] EmptyPacketBytes = new byte[0];

		protected Packet(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length > 0)
			{
				Header = ParseHeader(packetBytes);
				Payload = ParsePayload(packetBytes);
			}
		}

		protected Packet(TPacketHeader header, TPacketPayload payload)
		{
			Header = header;
			Payload = payload;
		}

		protected virtual byte[] GetPacketBytes()
		{
			var headerBytes = Header != null ? Header.GetBytes() : null;
			var headerBytesLength = (headerBytes != null ? 1 : 0);

			var payloadBytes = Payload != null ? Payload.GetBytes() : null;
			var payloadBytesLength = (payloadBytes != null ? payloadBytes.Length : 0);

			var packetBytesArraySize = headerBytesLength + payloadBytesLength;
			var packetBytesArray = new byte[packetBytesArraySize];

			if (headerBytes != null && headerBytesLength > 0)
			{
				Array.Copy(headerBytes, 0, packetBytesArray, 0, headerBytesLength);
			}

			if (payloadBytes != null && payloadBytesLength > 0)
			{
				Array.Copy(payloadBytes, 0, packetBytesArray, headerBytesLength, payloadBytesLength);
			}

			return packetBytesArray;
		}

		protected abstract TPacketHeader ParseHeader(byte[] packetBytes);

		protected abstract TPacketPayload ParsePayload(byte[] packetBytes);

		public override abstract string ToString();

		#region IPacket<TPacketHeader,TPacketPayload> Members

		public TPacketHeader Header { get; }

		public TPacketPayload Payload { get; }

		IProvideBytes IPacket.Header
		{
			get
			{
				return Header;
			}
		}

		IProvideBytes IPacket.Payload
		{
			get
			{
				return Payload;
			}
		}

		public byte[] GetBytes()
		{
			try
			{
				var packetBytes = GetPacketBytes();

				// so we never return null
				return packetBytes ?? EmptyPacketBytes;
			}
			catch (Exception ex)
			{
				throw new DataException("Error obtaining packet bytes.", ex);
			}
		}

		#endregion
	}
}