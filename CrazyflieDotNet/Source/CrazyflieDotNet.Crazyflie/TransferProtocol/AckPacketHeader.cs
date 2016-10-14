#region Imports

using System;
using System.Data;
using CrazyflieDotNet.Crazyradio.Driver;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	///     Header Format (1 byte):
	///     8  7  6  5  4  3  2  1
	///     [# Retries ][Res.][P][A]
	///     Res. = reserved for transfer layer.
	///     P = Power detector triggered
	///     A = Ack received
	/// </summary>
	public class AckPacketHeader
		: PacketHeader, IAckPacketHeader
	{
		/// <summary>
		///     Header Format (1 byte):
		///     8  7  6  5  4  3  2  1
		///     [# Retries ][Res.][P][A]
		///     Res. = reserved for transfer layer.
		///     P = Power detector triggered
		///     A = Ack received
		/// </summary>
		/// <param name="headerByte"> </param>
		public AckPacketHeader(byte headerByte)
		{
			RetryCount = ParseRetryCount(headerByte);
			PowerDetector = ParsePowerDetector(headerByte);
			AckRecieved = ParseAckReceived(headerByte);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacketHeader"/> class.
		/// </summary>
		/// <param name="retryCount">Retry count.</param>
		/// <param name="powerDetector">If set to <c>true</c> power detector.</param>
		/// <param name="ackRecieved">If set to <c>true</c> ack recieved.</param>
		public AckPacketHeader(int retryCount, bool powerDetector, bool ackRecieved)
		{
			RetryCount = retryCount;
			PowerDetector = powerDetector;
			AckRecieved = ackRecieved;
		}

		/// <summary>
		/// Gets the retry count.
		/// </summary>
		/// <value>The retry count.</value>
		public int RetryCount { get; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacketHeader"/>
		/// power detector.
		/// </summary>
		/// <value><c>true</c> if power detector; otherwise, <c>false</c>.</value>
		public bool PowerDetector { get; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacketHeader"/>
		/// ack recieved.
		/// </summary>
		/// <value><c>true</c> if ack recieved; otherwise, <c>false</c>.</value>
		public bool AckRecieved { get; }

		/// <summary>
		/// Gets the packet header bytes.
		/// </summary>
		/// <returns>The packet header bytes.</returns>
		protected override byte[] GetPacketHeaderBytes()
		{
			try
			{
				var retryCountByte = (byte)RetryCount;
				var retryCountByteAnd15 = (byte)(retryCountByte & 0x0F);
				var retryCountByteAnd15LeftShifted4 = (byte)(retryCountByteAnd15 << 4);
				var reservedLeftShifted2 = (byte)(0x03 << 2);

				var powerDetectorByte = Convert.ToByte(PowerDetector);
				var powerDetectorByteAnd1 = (byte)(powerDetectorByte & 0x01);
				var powerDetectorByteLeftShifted1 = (byte)(powerDetectorByteAnd1 << 1);

				var ackRecievedByte = Convert.ToByte(AckRecieved);
				var ackRecievedByteAnd1 = (byte)(ackRecievedByte & 0x01);

				return new[] { (byte)(retryCountByteAnd15LeftShifted4 | reservedLeftShifted2 | powerDetectorByteLeftShifted1 | ackRecievedByteAnd1) };
			}
			catch (Exception ex)
			{
				throw new DataException("Error converting ack packet header to byte.", ex);
			}
		}

		/// <summary>
		/// Parses the retry count.
		/// </summary>
		/// <returns>The retry count.</returns>
		/// <param name="headerByte">Header byte.</param>
		private int ParseRetryCount(byte headerByte)
		{
			try
			{
				var retryCountByte = (byte)(headerByte >> 4);
				var retryCount = (int)Convert.ToInt32(retryCountByte);
				return retryCount;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting ack packet header retry count value from header byte", ex);
			}
		}

		/// <summary>
		/// Parses the power detector.
		/// </summary>
		/// <returns><c>true</c>, if power detector was parsed, <c>false</c> otherwise.</returns>
		/// <param name="headerByte">Header byte.</param>
		private bool ParsePowerDetector(byte headerByte)
		{
			try
			{
				var powerDetectorByte = (byte)(headerByte & 0x02);
				var powerDetector = Convert.ToBoolean(powerDetectorByte);
				return powerDetector;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting ack packet header power detected value from header byte", ex);
			}
		}

		/// <summary>
		/// Parses the ack received.
		/// </summary>
		/// <returns><c>true</c>, if ack received was parsed, <c>false</c> otherwise.</returns>
		/// <param name="headerByte">Header byte.</param>
		private bool ParseAckReceived(byte headerByte)
		{
			try
			{
				var ackReceivedByte = (byte)(headerByte & 0x01);
				var ackReceived = Convert.ToBoolean(ackReceivedByte);
				return ackReceived;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting ack packet header ack received value from header byte", ex);
			}
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacketHeader"/>.
		/// </summary>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacketHeader"/>.</returns>
		public override string ToString()
		{
			return string.Format("[RetryCount={0}, PowerDetector={1}, AckRecieved={2}]", RetryCount, PowerDetector, AckRecieved);
		}
	}
}