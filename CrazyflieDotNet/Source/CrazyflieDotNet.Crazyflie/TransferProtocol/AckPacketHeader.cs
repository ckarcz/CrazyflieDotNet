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
			RetryCount = GetRetryCount(headerByte);
			PowerDetector = GetPowerDetector(headerByte);
			AckRecieved = GetAckRecieved(headerByte);
		}

		public AckPacketHeader(int retryCount, bool powerDetector, bool ackRecieved)
		{
			RetryCount = retryCount;
			PowerDetector = powerDetector;
			AckRecieved = ackRecieved;
		}

		public int RetryCount { get; }
		public bool PowerDetector { get; }
		public bool AckRecieved { get; }

		protected override byte? GetPacketHeaderByte()
		{
			try
			{
				var retryCountByte = (byte) RetryCount;
				var retryCountByteAnd15 = (byte) (retryCountByte & 0x0F);
				var retryCountByteAnd15LeftShifted4 = (byte) (retryCountByteAnd15 << 4);
				var reservedLeftShifted2 = (byte) (0x03 << 2);

				var powerDetectorByte = Convert.ToByte(PowerDetector);
				var powerDetectorByteAnd1 = (byte) (powerDetectorByte & 0x01);
				var powerDetectorByteLeftShifted1 = (byte) (powerDetectorByteAnd1 << 1);

				var ackRecievedByte = Convert.ToByte(AckRecieved);
				var ackRecievedByteAnd1 = (byte) (ackRecievedByte & 0x01);

				return (byte) (retryCountByteAnd15LeftShifted4 | reservedLeftShifted2 | powerDetectorByteLeftShifted1 | ackRecievedByteAnd1);
			}
			catch (Exception ex)
			{
				throw new DataException("Error converting ack packet header to byte.", ex);
			}
		}

		private int GetRetryCount(byte headerByte)
		{
			try
			{
                var retryCountByte = (byte)(headerByte >> 4);
				var retryCount = (int) Convert.ToInt32(retryCountByte);
				return retryCount;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting ack packet header retry count value from header byte", ex);
			}
		}

		private bool GetPowerDetector(byte headerByte)
		{
			try
			{
				var powerDetectorByte = (byte) (headerByte & 0x02);
				var powerDetector = Convert.ToBoolean(powerDetectorByte);
				return powerDetector;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting ack packet header power detected value from header byte", ex);
			}
		}

		private bool GetAckRecieved(byte headerByte)
		{
			try
			{
				var ackReceivedByte = (byte) (headerByte & 0x01);
				var ackReceived = Convert.ToBoolean(ackReceivedByte);
				return ackReceived;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting ack packet header ack received value from header byte", ex);
			}
		}
	}
}