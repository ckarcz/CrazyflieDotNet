using System;

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Ping packet header.
	/// </summary>
	public sealed class PingPacketHeader
		: PacketHeader, IPingPacketHeader
	{
		private readonly byte pingPacketHeaderByte;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.PingPacketHeader"/> class.
		/// </summary>
		/// <param name="headerByte">Header byte.</param>
		public PingPacketHeader(byte headerByte = 0xff)
		{
			pingPacketHeaderByte = headerByte;
		}

		/// <summary>
		/// Gets the packet header bytes.
		/// </summary>
		/// <returns>The packet header bytes.</returns>
		protected override byte[] GetPacketHeaderBytes()
		{
			return new[] { pingPacketHeaderByte };
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.PingPacketHeader"/>.
		/// </summary>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.PingPacketHeader"/>.</returns>
		public override string ToString()
		{
			return string.Format("[{0}]", BitConverter.ToString(GetBytes()));
		}
	}
}