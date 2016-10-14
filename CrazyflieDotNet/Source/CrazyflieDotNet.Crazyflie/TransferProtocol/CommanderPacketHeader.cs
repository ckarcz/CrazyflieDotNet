namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Commander packet header.
	/// </summary>
	public class CommanderPacketHeader
		: OutputPacketHeader, ICommanderPacketHeader
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.CommanderPacketHeader"/> class.
		/// </summary>
		/// <param name="headerByte">Header byte.</param>
		public CommanderPacketHeader(byte headerByte)
			: base(headerByte)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.CommanderPacketHeader"/> class.
		/// </summary>
		/// <param name="channel">Channel.</param>
		public CommanderPacketHeader(CommunicationChannel channel = DefaultChannel)
			: base(CommunicationPort.Commander, channel)
		{
		}
	}
}