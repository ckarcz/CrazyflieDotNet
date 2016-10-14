#region Imports

using System;
using System.Data;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public abstract class PacketPayload
		: IPacketPayload
	{
		private static readonly byte[] EmptyPacketPayloadBytes = new byte[0];

		/// <summary>
		/// Gets the bytes.
		/// </summary>
		/// <returns>The bytes.</returns>
		public byte[] GetBytes()
		{
			try
			{
				var packetBytes = GetPacketPayloadBytes();

				// so we never return null
				return packetBytes ?? EmptyPacketPayloadBytes;
			}
			catch (Exception ex)
			{
				throw new DataException("Error obtaining packet payload bytes.", ex);
			}
		}

		protected abstract byte[] GetPacketPayloadBytes();

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