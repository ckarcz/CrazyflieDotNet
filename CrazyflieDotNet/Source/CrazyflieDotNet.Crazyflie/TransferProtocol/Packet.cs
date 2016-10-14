#region Imports

using System;
using System.Data;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Packet.
	/// </summary>
	public abstract class Packet<TPacketHeader>
		: Packet<TPacketHeader, IPacketPayload> where TPacketHeader : IPacketHeader
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.Packet`1"/> class.
		/// </summary>
		/// <param name="packetBytes">Packet bytes.</param>
		protected Packet(byte[] packetBytes)
			: base(packetBytes)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.Packet`1"/> class.
		/// </summary>
		/// <param name="header">Header.</param>
		protected Packet(TPacketHeader header)
			: base(header, null)
		{
		}

		/// <summary>
		/// Parses the payload.
		/// </summary>
		/// <returns>The payload.</returns>
		/// <param name="packetBytes">Packet bytes.</param>
		protected override IPacketPayload ParsePayload(byte[] packetBytes)
		{
			return null;
		}
	}

	/// <summary>
	/// Packet.
	/// </summary>
	public abstract class Packet<TPacketHeader, TPacketPayload>
		: IPacket<TPacketHeader, TPacketPayload> where TPacketHeader : IProvideBytes where TPacketPayload : IProvideBytes
	{
		private static readonly byte[] EmptyPacketBytes = new byte[0];

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.Packet`2"/> class.
		/// </summary>
		/// <param name="packetBytes">Packet bytes.</param>
		protected Packet(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length > 0)
			{
				Header = ParseHeader(packetBytes);
				Payload = ParsePayload(packetBytes);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.Packet`2"/> class.
		/// </summary>
		/// <param name="header">Header.</param>
		/// <param name="payload">Payload.</param>
		protected Packet(TPacketHeader header, TPacketPayload payload)
		{
			Header = header;
			Payload = payload;
		}

		/// <summary>
		/// Gets the packet bytes.
		/// </summary>
		/// <returns>The packet bytes.</returns>
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

		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <value>The header.</value>
		public TPacketHeader Header { get; }

		/// <summary>
		/// Gets the payload.
		/// </summary>
		/// <value>The payload.</value>
		public TPacketPayload Payload { get; }

		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <value>the header.</value>
		IProvideBytes IPacket.Header
		{
			get
			{
				return Header;
			}
		}

		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <value>the header.</value>
		IProvideBytes IPacket.Payload
		{
			get
			{
				return Payload;
			}
		}

		/// <summary>
		/// Parses the header.
		/// </summary>
		/// <returns>The header.</returns>
		/// <param name="packetBytes">Packet bytes.</param>
		protected abstract TPacketHeader ParseHeader(byte[] packetBytes);

		/// <summary>
		/// Parses the payload.
		/// </summary>
		/// <returns>The payload.</returns>
		/// <param name="packetBytes">Packet bytes.</param>
		protected abstract TPacketPayload ParsePayload(byte[] packetBytes);

		/// <summary>
		/// Gets the bytes.
		/// </summary>
		/// <returns>The bytes.</returns>
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

		public override string ToString()
		{
			return string.Format("[Header={0}, Payload={1}]", Header, Payload);
		}
	}
}