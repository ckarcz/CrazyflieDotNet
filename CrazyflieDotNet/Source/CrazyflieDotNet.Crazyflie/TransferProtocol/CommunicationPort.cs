namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Communication port.
	/// </summary>
	public enum CommunicationPort
	{
		/// <summary>
		/// The console.
		/// </summary>
		Console = 0x00,

		/// <summary>
		/// The parameters.
		/// </summary>
		Parameters = 0x02,

		/// <summary>
		/// The commander.
		/// </summary>
		Commander = 0x03,

		/// <summary>
		/// The logging.
		/// </summary>
		Logging = 0x05,

		/// <summary>
		/// The debugging.
		/// </summary>
		Debugging = 0x0E,

		/// <summary>
		/// The link control.
		/// </summary>
		LinkControl = 0x0F,

		/// <summary>
		/// All.
		/// </summary>
		All = 0xFF
	}
}