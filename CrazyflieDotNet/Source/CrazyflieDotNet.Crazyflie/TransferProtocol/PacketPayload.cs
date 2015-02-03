#region Imports

using System;
using System.Data;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public abstract class PacketPayload
		: IPacketPayload
	{
		public static readonly byte[] EmptyPacketPayloadBytes = new byte[0];

		#region IPacketPayload Members

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

		#endregion

		protected abstract byte[] GetPacketPayloadBytes();
	}
}