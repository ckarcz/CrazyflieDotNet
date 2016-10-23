using System;

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Packet with typed header and payload.
	/// </summary>
	public interface IPacket<out TPacketHeader, out TPacketPayload>
		: IPacket<TPacketHeader> where TPacketHeader : IProvideBytes where TPacketPayload : IProvideBytes
	{
		/// <summary>
		/// Gets the payload.
		/// </summary>
		/// <value>The payload.</value>
		new TPacketPayload Payload { get; }
	}

	/// <summary>
	/// Packet with typed header and generic payload.
	/// </summary>
	public interface IPacket<out TPacketHeader>
		: IPacket where TPacketHeader : IProvideBytes
	{
		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <value>The header.</value>
		new TPacketHeader Header { get; }
	}

	/// <summary>
	/// Packet with generic header and payload.
	/// </summary>
	public interface IPacket
		: IProvideBytes
	{
		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <value>The header.</value>
		IProvideBytes Header { get; }

		/// <summary>
		/// Gets the payload.
		/// </summary>
		/// <value>The payload.</value>
		IProvideBytes Payload { get; }
	}
}