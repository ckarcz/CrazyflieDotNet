using System.Collections.Generic;

namespace CrazyflieDotNet.Crazyradio
{
	public interface IScanChannelsResult
	{
		RadioDataRate DataRate { get; }

		IEnumerable<RadioChannel> Channels { get; }
	}
}