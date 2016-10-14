#region Imports

using System;
using System.Data;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Packet header.
	/// </summary>
	public abstract class PacketHeader
		: IPacketHeader
	{
		private static readonly byte[] EmptyPacketHeaderBytes = new byte[0];

		/// <summary>
		/// Gets the bytes.
		/// </summary>
		/// <returns>The bytes.</returns>
		public byte[] GetBytes()
		{
			try
			{
				var packetHeaderBytes = GetPacketHeaderBytes();

				// so we never return null
				return packetHeaderBytes ?? EmptyPacketHeaderBytes;
			}
			catch (Exception ex)
			{
				throw new DataException("Error obtaining packet header bytes.", ex);
			}
		}

		/// <summary>
		/// Gets the packet header bytes.
		/// </summary>
		/// <returns>The packet header bytes.</returns>
		protected abstract byte[] GetPacketHeaderBytes();

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