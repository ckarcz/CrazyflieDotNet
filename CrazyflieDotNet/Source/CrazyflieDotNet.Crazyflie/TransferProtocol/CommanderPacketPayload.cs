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
				throw new ArgumentNullException("payloadBytes");
			}

			if (payloadBytes.Length != _commanderPayloadSize)
			{
				throw new ArgumentException(string.Format("Commander packet payload size must be {0} bytes.", _commanderPayloadSize));
			}

			Roll = GetRoll(payloadBytes);
			Pitch = GetPitch(payloadBytes);
			Yaw = GetYaw(payloadBytes);
			Thrust = GetThurst(payloadBytes);
		}

		public CommanderPacketPayload(float roll, float pitch, float yaw, ushort thrust)
		{
			Roll = roll;
			Pitch = pitch;
			Yaw = yaw;
			Thrust = thrust;
		}

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

		private float GetRoll(byte[] payloadBytes)
		{
			if (payloadBytes == null)
			{
				throw new ArgumentNullException("payloadBytes");
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

		private float GetPitch(byte[] payloadBytes)
		{
			if (payloadBytes == null)
			{
				throw new ArgumentNullException("payloadBytes");
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

		private float GetYaw(byte[] payloadBytes)
		{
			if (payloadBytes == null)
			{
				throw new ArgumentNullException("payloadBytes");
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

		private ushort GetThurst(byte[] payloadBytes)
		{
			if (payloadBytes == null)
			{
				throw new ArgumentNullException("payloadBytes");
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

		private static readonly int _floatSize = sizeof (float);
		private static readonly int _shortSize = sizeof (ushort);
		private static readonly int _commanderPayloadSize = _floatSize * 3 + _shortSize;

		#region ICommanderPacketPayload Members

		public float Roll { get; }

		public float Pitch { get; }

		public float Yaw { get; }

		public ushort Thrust { get; }

		#endregion
	}
}