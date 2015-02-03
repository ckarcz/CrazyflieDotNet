#region Imports

using System.Collections.Generic;

#endregion

namespace CrazyflieDotNet.Crazyradio.Driver
{
	public sealed class ScanChannelsResult
	{
		public ScanChannelsResult(RadioDataRate dataRate, IEnumerable<RadioChannel> channels)
		{
			DataRate = dataRate;
			Channels = channels;
		}

		public RadioDataRate DataRate { get; private set; }

		public IEnumerable<RadioChannel> Channels { get; private set; }
	}
}