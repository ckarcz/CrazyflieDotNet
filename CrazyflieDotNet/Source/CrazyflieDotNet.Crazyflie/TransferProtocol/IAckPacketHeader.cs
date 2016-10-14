namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Ack packet header.
	/// </summary>
	public interface IAckPacketHeader
		: IPacketHeader
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.IAckPacketHeader"/>
		/// ack recieved.
		/// </summary>
		/// <value><c>true</c> if ack recieved; otherwise, <c>false</c>.</value>
		bool AckRecieved { get; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.IAckPacketHeader"/>
		/// power detector.
		/// </summary>
		/// <value><c>true</c> if power detector; otherwise, <c>false</c>.</value>
		bool PowerDetector { get; }

		/// <summary>
		/// Gets the retry count.
		/// </summary>
		/// <value>The retry count.</value>
		int RetryCount { get; }
	}
}