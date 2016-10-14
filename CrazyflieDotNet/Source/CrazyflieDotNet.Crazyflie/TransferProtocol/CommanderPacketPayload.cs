#region Imports

using System;
using System.Data;
using System.Linq;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	///     Commander Payload Format:
	///     Name   |  Index  |  Type  |  Size (bytes)
	///     roll        0       float      4
	///     pitch       4       float      4
	///     yaw         8       float      4
	///     thurst      12      ushort     2
	///     .............................total: 14 bytes
	/// </summary>
	public class CommanderPacketPayload
		: PacketPayload, ICommanderPacketPayload
	{
		private static readonly int _floatSize = sizeof(float);
		private static readonly int _shortSize = sizeof(ushort);
		private static readonly int _commanderPayloadSize = _floatSize * 3 + _shortSize;

		/// <summary>
		///     Commander Payload Format:
		///     Name   |  Index  |  Type  |  Size (bytes)
		///     roll        0       float      4
		///     pitch       4       float      4
		///     yaw         8       float      4
		///     thurst      12      ushort     2
		///     .............................total: 14 bytes
		/// </summary>
		/// <param name="payloadBytes"> </param>
		public CommanderPacketPayload(byte[] payloadBytes)
		{
			if (payloadBytes == null)
			{
				throw new ArgumentNullException(nameof(payloadBytes));
			}

			if (payloadBytes.Length != _commanderPayloadSize)
			{
				throw new ArgumentException(string.Format("Commander packet payload size must be {0} bytes.", _commanderPayloadSize));
			}

			Roll = ParseRoll(payloadBytes);
			Pitch = ParsePitch(payloadBytes);
			Yaw = ParseYaw(payloadBytes);
			Thrust = ParseThrust(payloadBytes);
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.CommanderPacketPayload"/> class.
		/// </summary>
		/// <param name="roll">Roll.</param>
		/// <param name="pitch">Pitch.</param>
		/// <param name="yaw">Yaw.</param>
		/// <param name="thrust">Thrust.</param>
		public CommanderPacketPayload(float roll, float pitch, float yaw, ushort thrust)
		{
			Roll = roll;
			Pitch = pitch;
			Yaw = yaw;
			Thrust = thrust;
		}

		/// <summary>
		/// Gets the roll.
		/// </summary>
		/// <value>The roll.</value>
		public float Roll { get; }

		/// <summary>
		/// Gets the pitch.
		/// </summary>
		/// <value>The pitch.</value>
		public float Pitch { get; }

		/// <summary>
		/// Gets the yaw.
		/// </summary>
		/// <value>The yaw.</value>
		public float Yaw { get; }

		/// <summary>
		/// Gets the thrust.
		/// </summary>
		/// <value>The thrust.</value>
		public ushort Thrust { get; }

		/// <summary>
		/// Gets the packet payload bytes.
		/// </summary>
		/// <returns>The packet payload bytes.</returns>
		protected override byte[] GetPacketPayloadBytes()
		{
			try
			{
				var rollBytes = BitConverter.GetBytes(Roll);
				var pitchBytes = BitConverter.GetBytes(Pitch);
				var yawBytes = BitConverter.GetBytes(Yaw);
				var thrustBytes = BitConverter.GetBytes(Thrust);

				var commanderPayloadBytes = new byte[_commanderPayloadSize];

				Array.Copy(rollBytes, 0, commanderPayloadBytes, 0, rollBytes.Length);
				Array.Copy(pitchBytes, 0, commanderPayloadBytes, rollBytes.Length, pitchBytes.Length);
				Array.Copy(yawBytes, 0, commanderPayloadBytes, rollBytes.Length + pitchBytes.Length, yawBytes.Length);
				Array.Copy(thrustBytes, 0, commanderPayloadBytes, rollBytes.Length + pitchBytes.Length + yawBytes.Length, thrustBytes.Length);

				return commanderPayloadBytes;
			}
			catch (Exception ex)
			{
				throw new DataException("Error converting commander packet payload to bytes.", ex);
			}
		}

		/// <summary>
		/// Parses the roll.
		/// </summary>
		/// <returns>The roll.</returns>
		/// <param name="payloadBytes">Payload bytes.</param>
		private float ParseRoll(byte[] payloadBytes)
		{
			if (payloadBytes == null)
			{
				throw new ArgumentNullException(nameof(payloadBytes));
			}

			try
			{
				var rollBytes = payloadBytes.Take(_floatSize).ToArray();
				var roll = BitConverter.ToSingle(rollBytes, 0);
				return roll;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting commander packet header roll value from header byte", ex);
			}
		}

		/// <summary>
		/// Parses the pitch.
		/// </summary>
		/// <returns>The pitch.</returns>
		/// <param name="payloadBytes">Payload bytes.</param>
		private float ParsePitch(byte[] payloadBytes)
		{
			if (payloadBytes == null)
			{
				throw new ArgumentNullException(nameof(payloadBytes));
			}

			try
			{
				var pitchBytes = payloadBytes.Skip(_floatSize).Take(_floatSize).ToArray();
				var pitch = BitConverter.ToSingle(pitchBytes, 0);
				return pitch;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting commander packet header pitch value from header byte", ex);
			}
		}

		/// <summary>
		/// Parses the yaw.
		/// </summary>
		/// <returns>The yaw.</returns>
		/// <param name="payloadBytes">Payload bytes.</param>
		private float ParseYaw(byte[] payloadBytes)
		{
			if (payloadBytes == null)
			{
				throw new ArgumentNullException(nameof(payloadBytes));
			}

			try
			{
				var yawBytes = payloadBytes.Skip(_floatSize * 2).Take(_floatSize).ToArray();
				var yaw = BitConverter.ToSingle(yawBytes, 0);
				return yaw;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting commander packet header yaw value from header byte", ex);
			}
		}

		/// <summary>
		/// Parses the thrust.
		/// </summary>
		/// <returns>The thrust.</returns>
		/// <param name="payloadBytes">Payload bytes.</param>
		private ushort ParseThrust(byte[] payloadBytes)
		{
			if (payloadBytes == null)
			{
				throw new ArgumentNullException(nameof(payloadBytes));
			}

			try
			{
				var thrustBytes = payloadBytes.Skip(_floatSize * 3).Take(_shortSize).ToArray();
				var thrust = BitConverter.ToUInt16(thrustBytes, 0);
				return thrust;
			}
			catch (Exception ex)
			{
				throw new DataException("Error getting commander packet header thrust value from header byte", ex);
			}
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.CommanderPacketPayload"/>.
		/// </summary>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.CommanderPacketPayload"/>.</returns>
		public override string ToString()
		{
			return string.Format("[Roll={0}, Pitch={1}, Yaw={2}, Thrust={3}]", Roll, Pitch, Yaw, Thrust);
		}
	}
}