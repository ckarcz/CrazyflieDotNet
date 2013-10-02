namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public sealed class PacketHeader
	{
		private byte? _bytesCached;

		public PacketHeader(Port port)
			: this(port, DefaultChannel)
		{
		}

		public PacketHeader(Port port, Channel channel)
		{
			Port = port;
			Channel = channel;
		}

		public Channel Channel { get; private set; }

		public Port Port { get; private set; }

		public static Channel DefaultChannel = Channel.Channel0;

		internal byte HeaderByte
		{
			get { return (_bytesCached ?? (_bytesCached = GetByte(this))).Value; }
		}

		public static byte GetByte(PacketHeader packetHeader)
		{
			var port = (byte)packetHeader.Port;
			var channel = (byte)packetHeader.Channel;

			// Header Format (1 byte):
			//  7  6  5  4  3  2  1  0
			// [   Port   ][Res. ][Ch.]
			// Res. = reserved for transfer layer.

			byte portByte = (byte)port;
			byte portByteAnd15 = (byte)(portByte & 0x0F);
			byte portByteAnd15LeftShifted4 = (byte)(portByteAnd15 << 4);
			byte reservedLeftShifted2 = (byte)(0x03 << 2);
			byte channelByte = (byte)channel;
			byte channelByteAnd3 = (byte)(channel & 0x03);

			return (byte)(portByteAnd15LeftShifted4 | reservedLeftShifted2 | channelByteAnd3);
		}
	}
}