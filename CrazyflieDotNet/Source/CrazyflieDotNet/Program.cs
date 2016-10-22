#region Imports

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CrazyflieDotNet.Crazyflie.TransferProtocol;
using CrazyflieDotNet.Crazyradio.Driver;
using log4net;
using log4net.Config;
using SlimDX.DirectInput;

#endregion

namespace CrazyflieDotNet
{
	/// <summary>
	///     Currently, this Program is only a small Test like executable for testing during development.
	/// </summary>
	internal class Program
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

		private static void Main(string[] args)
		{
			SetUpLogging();

			try
			{
				var crazyradioDriver = SetupCrazyflieDriver();

				// TESTS:
				try
				{
					//TestCRTP(crazyradioDriver);

					TestPS3Controller(crazyradioDriver);
				}
				catch (Exception ex)
				{
					Log.Error("Error testing Crazyradio.", ex);
					crazyradioDriver?.Close();
					throw;
				}
				finally
				{
					crazyradioDriver?.Close();
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error setting up Crazyradio.", ex);
			}

			Log.Info("Sleepy time...Hit ESC to quit.");

			var sleep = true;
			while (sleep)
			{
				if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape)
				{
					sleep = false;
				}
			}
		}

		private static ICrazyradioDriver SetupCrazyflieDriver()
		{
			IEnumerable<ICrazyradioDriver> crazyradioDrivers = null;

			try
			{
				// Scan for connected Crazyradio USB dongles
				crazyradioDrivers = CrazyradioDriver.GetCrazyradios();
			}
			catch (Exception ex)
			{
				var msg = "Error getting Crazyradio USB dongle devices connected to computer.";
				Log.Error(msg, ex);
				throw new ApplicationException(msg, ex);
			}

			// If we found any
			if (crazyradioDrivers != null && crazyradioDrivers.Any())
			{
				// Use first available Crazyradio dongle
				var crazyradioDriver = crazyradioDrivers.First();

				try
				{
					// Initialize driver
					crazyradioDriver.Open();

					// Scan for any Crazyflie quadcopters ready for communication
					var scanResults = crazyradioDriver.ScanChannels();
					if (scanResults.Any())
					{
						// Use first online Crazyflie quadcopter found
						var firstScanResult = scanResults.First();

						// Set CrazyradioDriver's DataRate and Channel to that of online Crazyflie
						var dataRateWithCrazyflie = firstScanResult.DataRate;
						var channelWithCrazyflie = firstScanResult.Channels.First();
						crazyradioDriver.DataRate = dataRateWithCrazyflie;
						crazyradioDriver.Channel = channelWithCrazyflie;

						return crazyradioDriver;
					}
					else
					{
						Log.Warn("No Crazyflie quadcopters available for communication.");
						return null;
					}
				}
				catch (Exception ex)
				{
					var msg = "Error initializing Crazyradio USB dongle for communication with a Crazyflie quadcopter.";
					Log.Error(msg, ex);
					throw new ApplicationException(msg, ex);
				}
			}
			else
			{
				Log.Warn("No Crazyradio USB dongles found!");
				return null;
			}
		}

		private static void TestCRTP(ICrazyradioDriver crazyradioDriver)
		{
			if (crazyradioDriver != null)
			{
				var crazyRadioMessenger = new CrazyflieMessenger(crazyradioDriver);

				try
				{
					IAckPacket ackPacket = null;

					Log.InfoFormat("Ping Packet Request: {0}", PingPacket.Instance);
					ackPacket = crazyRadioMessenger.SendMessage(PingPacket.Instance);
					Log.InfoFormat("ACK Response: {0}", ackPacket);


					ushort thrustIncrements = 1000;
					float pitchIncrements = 5;
					float yawIncrements = 2;
					float rollIncrements = 5;
					ushort thrust = 10000;
					float pitch = 0;
					float yaw = 0;
					float roll = 0;

					var loop = true;
					while (loop)
					{
						Log.InfoFormat("Thrust: {0}, Pitch: {1}, Roll: {2}, Yaw: {3}.", thrust, pitch, roll, yaw);

						if (Console.KeyAvailable)
						{
							switch (Console.ReadKey().Key)
							{
							// end
							case ConsoleKey.Escape:
								loop = false;
								break;
							// pause
							case ConsoleKey.Spacebar:
								var commanderPacket = new CommanderPacket(roll, pitch, yaw, thrust = 10000);
								Log.InfoFormat("Commander Packet Request: {0}", commanderPacket);
								ackPacket = crazyRadioMessenger.SendMessage(commanderPacket);
								Log.InfoFormat("ACK Response: {0}", ackPacket);

								Log.InfoFormat("Paused...Hit SPACE to resume, ESC to quit.");

								var pauseLoop = true;
								while (pauseLoop)
								{
									if (Console.KeyAvailable)
									{
										switch (Console.ReadKey().Key)
										{
										// resume
										case ConsoleKey.Spacebar:
											pauseLoop = false;
											break;
										// end
										case ConsoleKey.Escape:
											pauseLoop = loop = false;
											break;
										}
									}
								}
								break;
							// thrust up
							case ConsoleKey.UpArrow:
								thrust += thrustIncrements;
								break;
							// thrust down
							case ConsoleKey.DownArrow:
								thrust -= thrustIncrements;
								break;
							// yaw right
							case ConsoleKey.RightArrow:
								yaw += yawIncrements;
								break;
							// yaw left
							case ConsoleKey.LeftArrow:
								yaw -= yawIncrements;
								break;
							// pitch backward
							case ConsoleKey.S:
								pitch += pitchIncrements;
								break;
							// pitch forward
							case ConsoleKey.W:
								pitch -= pitchIncrements;
								break;
							// roll right
							case ConsoleKey.D:
								roll += rollIncrements;
								break;
							// roll left
							case ConsoleKey.A:
								roll -= rollIncrements;
								break;
							default:
								Log.InfoFormat("Invalid key for action.");
								break;
							}
						}

						{
							var commanderPacket = new CommanderPacket(roll, pitch, yaw, thrust);
							Log.InfoFormat("Commander Packet Request: {0}", commanderPacket);
							ackPacket = crazyRadioMessenger.SendMessage(commanderPacket);
							Log.InfoFormat("ACK Response: {0}", ackPacket);
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error("Error testing Crazyradio.", ex);
				}
				finally
				{
					crazyRadioMessenger.SendMessage(new CommanderPacket(0, 0, 0, 0));
				}
			}
		}

		private static void TestPS3Controller(ICrazyradioDriver crazyradioDriver)
		{
			if (crazyradioDriver != null)
			{
				var crazyRadioMessenger = new CrazyflieMessenger(crazyradioDriver);

				var stopMotorsCommanderPacket = new CommanderPacket(roll: 0, pitch: 0, yaw: 0, thrust: 0);

				try
				{
					// Init
					float roll = 0;
					float pitch = 0;
					float yaw = 0;
					ushort thrust = 0;

					// Max/min values
					float rollRange = 50;
					float pitchRange = 50;
					float yawRange = 100;
					ushort thrustRange = 50000;

					// Stick ranges
					int stickRange = 1000;

					// Get first attached game controller found
					var directInput = new DirectInput();
					var attahcedGameControllerDevices = directInput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly);
					if (!attahcedGameControllerDevices.Any())
					{
						throw new ApplicationException("No available game controllers found.");
					}
					var attachedDeviceInstance = attahcedGameControllerDevices.First();
					var joystick = new Joystick(directInput, attachedDeviceInstance.InstanceGuid);

					foreach (DeviceObjectInstance doi in joystick.GetObjects(ObjectDeviceType.Axis))
					{
						joystick.GetObjectPropertiesById((int)doi.ObjectType).SetRange(-1 * stickRange, stickRange);
					}

					joystick.Properties.AxisMode = DeviceAxisMode.Absolute;
					joystick.Acquire();
					var joystickState = new JoystickState();

					var loop = true;
					while (loop)
					{
						if (Console.KeyAvailable)
						{
							switch (Console.ReadKey().Key)
							{
							// end
							case ConsoleKey.Escape:
								loop = false;
								break;
							// pause
							case ConsoleKey.Spacebar:
								Log.InfoFormat("Paused...Hit SPACE to resume, ESC to quit.");

								thrust = 0;
								pitch = 0;
								yaw = 0;
								roll = 0;
								crazyRadioMessenger.SendMessage(stopMotorsCommanderPacket);

								var pauseLoop = true;
								while (pauseLoop)
								{
									if (Console.KeyAvailable)
									{
										switch (Console.ReadKey().Key)
										{
										// resume
										case ConsoleKey.Spacebar:
											pauseLoop = false;
											break;
										// end
										case ConsoleKey.Escape:
											pauseLoop = loop = false;
											break;
										}
									}
								}
								break;
							default:
								Log.InfoFormat("Invalid key for action.");
								break;
							}
						}

						// Poll the device and get state
						joystick.Poll();
						joystick.GetCurrentState(ref joystickState);

						// Get buttons pressed info
						var stringWriter = new StringWriter();
						var buttons = joystickState.GetButtons();
						var anyButtonsPressed = buttons.Any(b => b == true);
						if (anyButtonsPressed)
						{
							for (int buttonNumber = 0; buttonNumber < buttons.Length; buttonNumber++)
							{
								if (buttons[buttonNumber] == true)
								{
									stringWriter.Write(string.Format("{0}", buttonNumber));
								}
							}
						}
						var buttonsPressedString = stringWriter.ToString().Trim();

						// Joystick info
						var leftStickX = joystickState.X;
						var leftStickY = joystickState.Y;
						var rightStickX = joystickState.RotationX;
						var rightStickY = joystickState.RotationY;

						roll = rollRange * rightStickX / stickRange;
						pitch = pitchRange * rightStickY / stickRange;
						yaw = yawRange * leftStickX / stickRange;
						thrust = (ushort)(leftStickY > 0 ? 0 : thrustRange * -1 * leftStickY / stickRange);

						var infoString = String.Format("LX:{0,7}, LY:{1,7}, RX:{2,7}, RY:{3,7}, Buttons:{4,7}.\tRoll:{5, 7}, Pitch:{6, 7}, Yaw:{7, 7}, Thrust:{8, 7}.", leftStickX, leftStickY, rightStickX, rightStickY, buttonsPressedString, roll, pitch, yaw, thrust);
						Console.WriteLine(infoString);

						var commanderPacket = new CommanderPacket(roll, pitch, yaw, thrust);
						crazyRadioMessenger.SendMessage(commanderPacket);
					}
				}
				catch (Exception ex)
				{
					Log.Error("Error testing Crazyradio.", ex);
				}
				finally
				{
					crazyRadioMessenger.SendMessage(stopMotorsCommanderPacket);
				}
			}
		}

		private static void SetUpLogging()
		{
			XmlConfigurator.Configure();
		}
	}
}
