namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Provide bytes.
	/// </summary>
	public interface IProvideBytes
	{
		/// <summary>
		/// Gets the bytes.
		/// </summary>
		/// <returns>The bytes.</returns>
		byte[] GetBytes();
	}
}