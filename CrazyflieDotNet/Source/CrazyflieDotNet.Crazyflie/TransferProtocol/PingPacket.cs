using System;

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Ping packet.
	/// </summary>
	public sealed class PingPacket
		: Packet<IPingPacketHeader, IProvideBytes>, IPingPacket
	{
		private static PingPacket _instance;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.PingPacket"/> class.
		/// </summary>
		public PingPacket()
			: base(new PingPacketHeader(0xff), null)
		{
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static PingPacket Instance
		{
			get { return _instance ?? (_instance = new PingPacket()); }
		}

		/// <summary>
		/// Parses the header.
		/// </summary>
		/// <returns>The header.</returns>
		/// <param name="packetBytes">Packet bytes.</param>
		protected override IPingPacketHeader ParseHeader(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetHeader = new PingPacketHeader(packetBytes[0]);
				return packetHeader;
			}

			return null;
		}

		/// <summary>
		/// Parses the payload.
		/// </summary>
		/// <returns>The payload.</returns>
		/// <param name="packetBytes">Packet bytes.</param>
		protected override IProvideBytes ParsePayload(byte[] packetBytes)
		{
			return null;
		}
	}
}