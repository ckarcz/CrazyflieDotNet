#region Imports

using LibUsbDotNet.Main;

#endregion

namespace CrazyflieDotNet.Crazyradio.Driver
{
	internal static class CrazyradioDataEndpointId
	{
		public static readonly ReadEndpointID DataReadEndpointId = ReadEndpointID.Ep01;

		public static readonly WriteEndpointID DataWriteEndpointId = WriteEndpointID.Ep01;
	}
}