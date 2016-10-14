using System;
using System.Linq;

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Ack packet with header but no payload.
	/// </summary>
	public class AckPacket
		: Packet<IAckPacketHeader>, IAckPacket
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacket"/> class.
		/// </summary>
		/// <param name="header">The Ack packet header.</param>
		public AckPacket(IAckPacketHeader header)
			: base(header)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacket"/> class.
		/// </summary>
		/// <param name="packetBytes">Ack packet bytes.</param>
		public AckPacket(byte[] packetBytes)
			: base(packetBytes)
		{
		}

		/// <summary>
		/// Parses the header.
		/// </summary>
		/// <returns>The header.</returns>
		/// <param name="packetBytes">Packet bytes.</param>
		protected override IAckPacketHeader ParseHeader(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetHeader = new AckPacketHeader(packetBytes[0]);
				return packetHeader;
			}

			return default(IAckPacketHeader);
		}
	}

	/// <summary>
	/// Ack packet with header and payload.
	/// </summary>
	public class AckPacket<TPacketPayload>
		: Packet<IAckPacketHeader, TPacketPayload>, IAckPacket<TPacketPayload> where TPacketPayload : IProvideBytes
	{
		readonly Func<byte[], TPacketPayload> ackPayloadBuilder;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacket`1"/> class.
		/// </summary>
		/// <param name="header">Ack packet header.</param>
		/// <param name="ackPayloadBuilder">Ack payload builder/delegate.</param>
		public AckPacket(IAckPacketHeader header, Func<byte[], TPacketPayload> ackPayloadBuilder)
			: base(header, default(TPacketPayload))
		{
			if (ackPayloadBuilder == null)
			{
				ackPayloadBuilder = (arg) => default(TPacketPayload);
			}

			this.ackPayloadBuilder = ackPayloadBuilder;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacket`1"/> class.
		/// </summary>
		/// <param name="packetBytes">Ack packet bytes.</param>
		/// <param name="ackPayloadBuilder">Ack payload builder/delegate.</param>
		public AckPacket(byte[] packetBytes, Func<byte[], TPacketPayload> ackPayloadBuilder)
			: base(packetBytes)
		{
			if (ackPayloadBuilder == null)
			{
				ackPayloadBuilder = (arg) => default(TPacketPayload);
			}

			this.ackPayloadBuilder = ackPayloadBuilder;
		}

		/// <summary>
		/// Parses the header.
		/// </summary>
		/// <returns>The header.</returns>
		/// <param name="packetBytes">Packet bytes.</param>
		protected override IAckPacketHeader ParseHeader(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetHeader = new AckPacketHeader(packetBytes[0]);
				return packetHeader;
			}

			return default(IAckPacketHeader);
		}

		/// <summary>
		/// Parses the payload.
		/// </summary>
		/// <returns>The payload.</returns>
		/// <param name="packetBytes">Packet bytes.</param>
		protected override TPacketPayload ParsePayload(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length > 0)
			{
				var packetHeader = ackPayloadBuilder(packetBytes.Skip(1).ToArray());
				return packetHeader;
			}

			return default(TPacketPayload);
		}
	}
}