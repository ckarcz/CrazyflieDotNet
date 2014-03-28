/*
 *						 _ _  _
 *		       ____ ___  ___  __________(_|_)(_)____
 *		      / __ `__ \/ _ \/ ___/ ___/ / _ \/ ___/
 *		     / / / / / /  __(__  |__  ) /  __/ /
 *		    /_/ /_/ /_/\___/____/____/_/\___/_/
 *
 *	     Copyright 2013 - Messier/Chris Karcz - ckarcz@gmail.com
 *
 *	This Source Code Form is subject to the terms of the Mozilla Public
 *	License, v. 2.0. If a copy of the MPL was not distributed with this
 *	file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public sealed class CommanderPacket
		: OutputPacket<ICommanderPacketHeader, ICommanderPacketPayload>, ICommanderPacket
	{
		public CommanderPacket(byte[] packetBytes)
			: base(packetBytes)
		{
		}

		public CommanderPacket(ICommanderPacketHeader header, ICommanderPacketPayload payload)
			: base(header, payload)
		{
		}

		public CommanderPacket(float roll, float pitch, float yaw, short thrust, Channel channel = OutputPacketHeader.DefaultChannel)
			: this(new CommanderPacketHeader(channel), new CommanderPacketPayload(roll, pitch, yaw, thrust))
		{
		}

		protected override ICommanderPacketHeader ParseHeader(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetHeader = new CommanderPacketHeader(packetBytes[0]);
				return packetHeader;
			}

			return null;
		}

		protected override ICommanderPacketPayload ParsePayload(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetPayload = new CommanderPacketPayload(packetBytes);
				return packetPayload;
			}

			return null;
		}
	}
}