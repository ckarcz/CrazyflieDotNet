#region Imports

using System.Collections.Generic;

#endregion Imports

namespace CrazyflieDotNet.Crazyradio
{
	public class ScanChannelsResult
		: IScanChannelsResult
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