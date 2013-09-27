using System.Collections.Generic;

namespace CrazyflieDotNet.Crazyradio.Driver
{
	/// <summary>
	/// The radio address of the Crazyradio.
	/// </summary>
	public interface IRadioAddress
	{
		/// <summary>
		/// The address bytes.
		/// </summary>
		byte[] Bytes { get; }
	}
}