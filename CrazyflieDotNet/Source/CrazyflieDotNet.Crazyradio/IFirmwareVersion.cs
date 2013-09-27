#region Imports

using System;

#endregion Imports

namespace CrazyflieDotNet.Crazyradio
{
	public interface IFirmwareVersion
		: IEquatable<IFirmwareVersion>, IComparable<IFirmwareVersion>
	{
		int MajorVersion { get; }

		int MinorVersion { get; }

		int PatchVersion { get; }
	}
}