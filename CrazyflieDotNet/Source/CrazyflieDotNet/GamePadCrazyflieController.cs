#region Imports

using System;
using System.IO;
using System.Linq;
using SlimDX.DirectInput;

#endregion

namespace CrazyflieDotNet
{
	// Initial testing for PS3 controller support.
	// You need drivers for PS3 controller
	// Search motionjoy application for windows.
	// Here is Windows 32 bit and 64 bit drivers:
	// http://www.hardcoreware.net/how-to-playstation-3-controller-64-bit-windows-7-vista/
	public class GamePadCrazyflieController
	{
		private readonly DirectInput directInput;

		public GamePadCrazyflieController()
		{
			directInput = new DirectInput();
		}

		public void Test()
		{
			var directInput = new DirectInput();
			var ps3DeviceInstance = directInput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly).First();
			var ps3Joystick = new Joystick(directInput, ps3DeviceInstance.InstanceGuid);

			ps3Joystick.RunControlPanel();

			ps3Joystick.Properties.AxisMode = DeviceAxisMode.Absolute;
			var acquireResult = ps3Joystick.Acquire();

			var state = new JoystickState();

			var loop = true;
			while (loop)
			{
				if (Console.KeyAvailable)
				{
					switch (Console.ReadKey().Key)
					{
						// end
						case ConsoleKey.Escape:
							Console.WriteLine("Quitting PS3 controller test.");
							loop = false;
							break;
						// pause
						case ConsoleKey.Spacebar:
							Console.WriteLine("Paused. Hit ESC to quit, SPACE to resume.");
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
					}
				}

				ps3Joystick.Poll();

				var stringWriter = new StringWriter();

				ps3Joystick.GetCurrentState(ref state);

				var buttons = state.GetButtons();
				var anyButtonsPressed = buttons.Any(b => b == true);
				if (anyButtonsPressed)
				{
					for (int buttonNumber = 0; buttonNumber < buttons.Length; buttonNumber++)
					{
						if (buttons[buttonNumber] == true)
						{
							stringWriter.Write(string.Format("{0} ", buttonNumber));
						}
					}
				}

				var buttonsPressedString = stringWriter.ToString().Trim();
				var infoString = String.Format("LX:{0}, LY:{1}, RX:{2}, RY:{3}, Buttons:{4}.", state.X, state.Y, state.RotationX, state.RotationY, buttonsPressedString);
				var infoString2 = String.Format("LX:{0}, LY:{1}, RX:{2}, RY:{3}, Buttons:{4}.", state.X- 32767, state.Y - 32767, state.RotationX - 32767, state.RotationY - 32767, buttonsPressedString);
				Console.WriteLine(infoString);
				
            }
		}
	}
}