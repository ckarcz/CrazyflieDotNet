#region Imports

using System;
using System.Data;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public abstract class PacketHeader
		: IPacketHeader
	{
		#region IPacketHeader Members

		public byte[] GetBytes()
		{
			try
			{
				var packetHeaderBytes = GetPacketHeaderBytes();
				return packetHeaderBytes;
			}
			catch (Exception ex)
			{
				throw new DataException("Error obtaining packet header bytes.", ex);
			}
		}

		#endregion

		protected abstract byte[] GetPacketHeaderBytes();
	}
}