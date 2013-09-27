using System.Collections.Generic;

namespace CrazyflieDotNet.Crazyradio.Driver
{
	public interface IScanChannelsResult
	{
		RadioDataRate DataRate { get; }

		IEnumerable<RadioChannel> Channels { get; }
	}
}