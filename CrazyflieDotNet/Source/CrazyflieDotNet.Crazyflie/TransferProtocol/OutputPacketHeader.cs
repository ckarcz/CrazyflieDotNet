#region Imports

using System;
using System.Data;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	///     Header Format (1 byte):
	///     8  7  6  5  4  3  2  1
	///     [  Port#  ][Res.][Ch.]
	///     Res. = reserved for transfer layer.
	///     Ch. = Channel
	/// </summary>
	public abstract class OutputPacketHeader
		: PacketHeader, IOutputPacketHeader
	{
		public const CommunicationChannel DefaultChannel = CommunicationChannel.Channel0;

		/// <summary>
		///     Header Format (1 byte):
		///     8  7  6  5  4  3  2  1
		///     [  Port#  ][Res.][Ch.]
		///     Res. = reserved for transfer layer.
		///     Ch. = Channel
		/// </summary>
		/// <param name="headerByte"> </param>
		protected OutputPacketHeader(byte headerByte)
		{
			Port = GetPort(headerByte);
			Channel = GetChannel(headerByte);
		}

		protected OutputPacketHeader(CommunicationPort port, CommunicationChannel channel = DefaultChannel)
		{
			Port = port;
			Channel = channel;
		}

		protected override byte? GetPacketHeaderByte()
		{
			try
			{
				var portByte = (byte) Port;
				var portByteAnd15 = (byte) (portByte & 0x0F);
				var portByteAnd15LeftShifted4 = (byte) (portByteAnd15 << 4);

				var reservedLeftShifted2 = (byte) (0x03 << 2);

				var channelByte = (byte) Channel;
				var channelByteAnd3 = (byte) (channelByte & 0x03);

				return (byte) (portByteAnd15LeftShifted4 | reservedLeftShifted2 | channelByteAnd3);
			}
			catch (Exception ex)
			{
				throw new DataException("Error converting output packet header to byte.", ex);
			}
		}

		protected virtual CommunicationPort GetPort(byte headerByte)
		{
			try
			{
				var headerByteRightShiftedFour = (byte) (headerByte >> 4);
				var portByte = (byte) (headerByteRightShiftedFour & 0x0F);
				var port = (CommunicationPort) Enum.ToObject(typeof (CommunicationPort), portByte);
				return port;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting output packet header port value from header byte", ex);
			}
		}

		protected virtual CommunicationChannel GetChannel(byte headerByte)
		{
			try
			{
				var channelByte = (byte) (headerByte & 0x03);
				var channel = (CommunicationChannel) Enum.ToObject(typeof (CommunicationChannel), channelByte);
				return channel;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting output packet header channel value from header byte", ex);
			}
		}

		#region IOutputPacketHeader Members

		public CommunicationPort Port { get; }

		public CommunicationChannel Channel { get; }

		#endregion
	}
}