#region Imports

using System;

#endregion Imports

namespace CrazyflieDotNet.Crazyradio.Driver
{
	/// <summary>
	///   Exception occuring within the CrazyradioDriver.
	/// </summary>
	public class CrazyradioDriverException
		: Exception
	{
		/// <summary>
		///   Initializes a new instance of CrazyradioDriverException.
		/// </summary>
		/// <param name="message"> The exception message. </param>
		/// <param name="innerException"> The inner exception. </param>
		public CrazyradioDriverException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		///   Initializes a new instance of CrazyradioDriverException.
		/// </summary>
		/// <param name="message"> The exception message. </param>
		public CrazyradioDriverException(string message)
			: base(message)
		{
		}
	}
}