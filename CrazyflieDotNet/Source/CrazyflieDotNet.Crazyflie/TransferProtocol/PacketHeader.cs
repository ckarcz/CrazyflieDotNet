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

		public byte? GetByte()
		{
			try
			{
				var packetHeaderByte = GetPacketHeaderByte();
				return packetHeaderByte;
			}
			catch (Exception ex)
			{
				throw new DataException("Error obtaining packet header byte.", ex);
			}
		}

		#endregion

		protected abstract byte? GetPacketHeaderByte();
	}
}