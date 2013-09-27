namespace CrazyflieDotNet.Crazyradio
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