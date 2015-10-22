#region Imports

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrazyflieDotNet.Crazyflie.TransferProtocol;
using SlimDX;
using SlimDX.DirectInput;

#endregion

namespace CrazyflieDotNet
{
	public interface ICrazyflieInputDeviceController
	{

	}

	public class CrazyflieInputDeviceController
		: ICrazyflieInputDeviceController
	{
		private float pitch = 0;
		// need another class that contains input device and mappings of buttons/sticks to commander packet params. also contain settings.
		private float roll = 0;
		private ushort thrust = 0;
		private float yaw = 0;
		private readonly ICrazyflieMessenger _crazyflieMessenger;
		private readonly IController _controller;
		private readonly float pitchRange = 50;
		private readonly float rollRange = 50;
		private readonly int stickRange = 1000;
		private readonly ushort thrustRange = 50000;
		private readonly float yawRange = 100;

		public CrazyflieInputDeviceController(IController controller, ICrazyflieMessenger crazyflieMessenger)
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			if (crazyflieMessenger == null)
			{
				throw new ArgumentNullException("crazyflieMessenger");
			}

			this._controller = controller;
			this._crazyflieMessenger = crazyflieMessenger;

			// TODO - figure out the protection level of these properties and methods for IController. Controller class should be initializing inputdevice and calling aquire.
			this._controller.SetJoystickRanges(-1000, 1000);
			if (!this._controller.IsAquired)
			{
				this._controller.AquireDevice();
			}

			this._controller.OnInputDeviceStateChanged += OnControllerStateChanged;

			// test
			var controllerProfile = new ControllerProfile();
			controllerProfile.Map(ControllerInput.Button3, CrazyflieAviation.Thrust);
		}

		private void OnControllerStateChanged(object sender, InputDeviceStateArgs inputDeviceStateArgs)
		{
			if (inputDeviceStateArgs == null)
			{
				throw new ArgumentNullException("inputDeviceStateArgs");
			}

			var newInputDeviceState = inputDeviceStateArgs.ControllerState;

			var roll = rollRange * newInputDeviceState.RotationX / stickRange;
			var pitch = pitchRange * newInputDeviceState.RotationY / stickRange;
			var yaw = yawRange * newInputDeviceState.X / stickRange;
			var thrust = (ushort) (newInputDeviceState.Y > 0 ? 0 : thrustRange * -1 * newInputDeviceState.Y / stickRange);

			var commanderPacket = new CommanderPacket(roll, pitch, yaw, thrust);

			var ackPacket = this._crazyflieMessenger.SendMessage(commanderPacket);
		}
	}

	public enum ControllerInput
	{
		Button0,
		Button1,
		Button2,
		Button3,
		// ...
		X,
		Y,
		Z
	}

	public enum CrazyflieAviation
	{
		Thrust,
		Pitch,
		Roll,
		Yaw
	}

	public class ControllerProfile
	{
		public void Map(ControllerInput controllerInput, CrazyflieAviation crazyflieAviation)
		{
			
		}
	}

	public interface IInputDeviceManager
	{
		IEnumerable<IController> GetAttachedControllers();
	}

	public class SlimDxDeviceManager
		: IInputDeviceManager
	{
		private static readonly DirectInput DirectInput;

		static SlimDxDeviceManager()
		{
			DirectInput = new DirectInput();
		}

		public IEnumerable<IController> GetAttachedControllers()
		{
			var attahcedGameControllerDevices = DirectInput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly);
			if (!attahcedGameControllerDevices.Any())
			{
				return Enumerable.Empty<IController>();
			}

			var inputDevices = new List<IController>();

			foreach (var attahcedGameControllerDevice in attahcedGameControllerDevices)
			{
				var slimDxJoystick = new Joystick(DirectInput, attahcedGameControllerDevice.InstanceGuid);
				var slimDxInputDevice = new SlimDxController(slimDxJoystick);
				inputDevices.Add(slimDxInputDevice);
			}

			return inputDevices;
		}
	}

	public interface IController
		: IDisposable
	{
		bool IsPolling { get; }
		bool IsAquired { get; }
		int LowerJoystickRange { get; }
		int UpperJoystickRange { get; }

		void SetJoystickRanges(int lowerRange, int upperRange);

		IAquireInputDeviceResult AquireDevice();

		void UnAquireDevice();

		void StartPollingDeviceAsync();

		void StopPollingDevice();

		event EventHandler<InputDeviceStateArgs> OnInputDeviceStateChanged;
	}

	public class InputDeviceStateArgs
		: EventArgs
	{
		public InputDeviceStateArgs(IControllerState controllerState)
		{
			if (controllerState == null)
			{
				throw new ArgumentNullException("controllerState");
			}

			ControllerState = controllerState;
		}

		public IControllerState ControllerState { get; }
	}

	public interface IControllerState
	{
		int X { get; }
		int Y { get; }
		int Z { get; }
		int RotationX { get; }
		int RotationY { get; }
		int RotationZ { get; }
		IEnumerable<bool> Buttons { get; }
	}

	public class ControllerState
		: IControllerState
	{
		public ControllerState(int x, int y, int z, int rotationX, int rotationY, int rotationZ, IEnumerable<bool> buttons)
		{
			X = x;
			Y = y;
			Z = z;
			RotationX = rotationX;
			RotationY = rotationY;
			RotationZ = rotationZ;
			Buttons = buttons ?? Enumerable.Empty<bool>();
		}

		public int X { get; }
		public int Y { get; }
		public int Z { get; }
		public int RotationX { get; }
		public int RotationY { get; }
		public int RotationZ { get; }
		public IEnumerable<bool> Buttons { get; }
	}

	public interface IAquireInputDeviceResult
	{
		string Name { get; }
		string Description { get; }
		bool IsSuccess { get; }
		bool IsFailed { get; }
	}

	public class AquireInputDeviceResult
		: IAquireInputDeviceResult
	{
		public AquireInputDeviceResult(Result slimDxResult)
		{
			if (slimDxResult == null)
			{
				throw new ArgumentNullException("slimDxResult");
			}

			Name = slimDxResult.Name;
			Description = slimDxResult.Description;
			IsSuccess = slimDxResult.IsSuccess;
			IsFailed = slimDxResult.IsFailure;
		}

		public string Name { get; }
		public string Description { get; }
		public bool IsSuccess { get; }
		public bool IsFailed { get; }
	}

	public class SlimDxController
		: IController
	{
		private static readonly object _pollingLOCK = new object();
		private bool _isPolling;
		private readonly Joystick _slimDxJoystick;

		public SlimDxController(Joystick slimDxJoystick)
		{
			if (slimDxJoystick == null)
			{
				throw new ArgumentNullException("slimDxJoystick");
			}

			this._slimDxJoystick = slimDxJoystick;
		}

		public bool IsPolling
		{
			get
			{
				lock (_pollingLOCK)
				{
					return _isPolling;
				}
			}
			private set
			{
				lock (_pollingLOCK)
				{
					_isPolling = value;
				}
			}
		}

		public bool IsAquired { get; private set; }
		public int LowerJoystickRange { get; private set; }
		public int UpperJoystickRange { get; private set; }

		public void SetJoystickRanges(int lowerRange, int upperRange)
		{
			foreach (DeviceObjectInstance doi in _slimDxJoystick.GetObjects(ObjectDeviceType.Axis))
			{
				_slimDxJoystick.GetObjectPropertiesById((int) doi.ObjectType).SetRange(lowerRange, upperRange);
			}

			LowerJoystickRange = lowerRange;
			UpperJoystickRange = upperRange;
		}

		public IAquireInputDeviceResult AquireDevice()
		{
			var aquireResult = _slimDxJoystick.Acquire();
			var resultWrapper = new AquireInputDeviceResult(aquireResult);
			IsAquired = resultWrapper.IsSuccess;
			return resultWrapper;
		}

		public void UnAquireDevice()
		{
			_slimDxJoystick.Unacquire();
			IsAquired = false;
		}

		public void StopPollingDevice()
		{
			IsPolling = false;
		}

		public void Dispose()
		{
			_slimDxJoystick.Dispose();
			IsAquired = false;
		}

		public void StartPollingDeviceAsync()
		{
			IsPolling = true;

			var priorSlimDxJoystickState = new JoystickState();
			var currentSlimDxJoystickState = new JoystickState();

			Task.Factory.StartNew(() =>
			{
				while (IsPolling)
				{
					priorSlimDxJoystickState = currentSlimDxJoystickState;
					_slimDxJoystick.Poll();
					_slimDxJoystick.GetCurrentState(ref currentSlimDxJoystickState);

					if (currentSlimDxJoystickState != null && priorSlimDxJoystickState != null)
					{
						if (!currentSlimDxJoystickState.Equals(priorSlimDxJoystickState))
						{
							var newInputDeviceState = new ControllerState(currentSlimDxJoystickState.X, currentSlimDxJoystickState.Y, currentSlimDxJoystickState.Z, currentSlimDxJoystickState.RotationX, currentSlimDxJoystickState.RotationY, currentSlimDxJoystickState.RotationZ, currentSlimDxJoystickState.GetButtons());
							NotifyInputDeviceStateChanged(newInputDeviceState);
						}
					}
				}
			}, TaskCreationOptions.LongRunning);
		}

		public event EventHandler<InputDeviceStateArgs> OnInputDeviceStateChanged;

		private void NotifyInputDeviceStateChanged(IControllerState newControllerState)
		{
			if (newControllerState == null)
			{
				throw new ArgumentNullException("newControllerState");
			}

			if (OnInputDeviceStateChanged != null)
			{
				OnInputDeviceStateChanged(this, new InputDeviceStateArgs(newControllerState));
			}
		}
	}
}