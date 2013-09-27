#region Imports

using LibUsbDotNet.Main;

#endregion Imports

namespace CrazyflieDotNet.Crazyradio
{
	public static class CrazyradioDataEndpointId
	{
		public static readonly ReadEndpointID DataReadEndpointId = ReadEndpointID.Ep01;

		public static readonly WriteEndpointID DataWriteEndpointId = WriteEndpointID.Ep01;
	}
}