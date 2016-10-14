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
			Port = ParsePort(headerByte);
			Channel = ParseChannel(headerByte);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.OutputPacketHeader"/> class.
		/// </summary>
		/// <param name="port">Port.</param>
		/// <param name="channel">Channel.</param>
		protected OutputPacketHeader(CommunicationPort port, CommunicationChannel channel = DefaultChannel)
		{
			Port = port;
			Channel = channel;
		}

		/// <summary>
		/// Gets the port.
		/// </summary>
		/// <value>The port.</value>
		public CommunicationPort Port { get; }

		/// <summary>
		/// Gets the channel.
		/// </summary>
		/// <value>The channel.</value>
		public CommunicationChannel Channel { get; }

		/// <summary>
		/// Gets the packet header bytes.
		/// </summary>
		/// <returns>The packet header bytes.</returns>
		protected override byte[] GetPacketHeaderBytes()
		{
			try
			{
				var portByte = (byte)Port;
				var portByteAnd15 = (byte)(portByte & 0x0F);
				var portByteAnd15LeftShifted4 = (byte)(portByteAnd15 << 4);

				var reservedLeftShifted2 = (byte)(0x03 << 2);

				var channelByte = (byte)Channel;
				var channelByteAnd3 = (byte)(channelByte & 0x03);

				return new[] { (byte)(portByteAnd15LeftShifted4 | reservedLeftShifted2 | channelByteAnd3) };
			}
			catch (Exception ex)
			{
				throw new DataException("Error converting output packet header to byte.", ex);
			}
		}

		/// <summary>
		/// Parses the port.
		/// </summary>
		/// <returns>The port.</returns>
		/// <param name="headerByte">Header byte.</param>
		protected virtual CommunicationPort ParsePort(byte headerByte)
		{
			try
			{
				var headerByteRightShiftedFour = (byte)(headerByte >> 4);
				var portByte = (byte)(headerByteRightShiftedFour & 0x0F);
				var port = (CommunicationPort)Enum.ToObject(typeof(CommunicationPort), portByte);
				return port;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting output packet header port value from header byte", ex);
			}
		}

		/// <summary>
		/// Parses the channel.
		/// </summary>
		/// <returns>The channel.</returns>
		/// <param name="headerByte">Header byte.</param>
		protected virtual CommunicationChannel ParseChannel(byte headerByte)
		{
			try
			{
				var channelByte = (byte)(headerByte & 0x03);
				var channel = (CommunicationChannel)Enum.ToObject(typeof(CommunicationChannel), channelByte);
				return channel;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting output packet header channel value from header byte", ex);
			}
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.OutputPacketHeader"/>.
		/// </summary>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.OutputPacketHeader"/>.</returns>
		public override string ToString()
		{
			return string.Format("[Port={0}, Channel={1}]", Port, Channel);
		}
	}
}