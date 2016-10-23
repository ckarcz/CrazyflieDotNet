#region Imports

using System;
using System.Collections.Generic;
using System.Linq;
using SlimDX.DirectInput;

#endregion

namespace CrazyflieDotNet.Input
{
	public static class MappedTo
	{
		public static readonly string Roll = "Roll";
		public static readonly string Pitch = "Pitch";
		public static readonly string Yaw = "Yaw";
		public static readonly string Thrust = "Thrust";
	}

	public class Tester
	{
		public void Test()
		{
			var joystick = GetFirstJoystickFound();

			var inputDevice = new InputDevice(joystick);
			var inputDeviceProfile = new InputDeviceProfile();

			var profiledInputDevice = new ProfiledInputDevice(inputDevice, inputDeviceProfile);
			profiledInputDevice.GetValueOrDefault(MappedTo.Thrust, 0);
		}

		private Joystick GetFirstJoystickFound()
		{
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
				joystick.GetObjectPropertiesById((int) doi.ObjectType).SetRange(-1 * 1000, 1000);
			}
			joystick.Properties.AxisMode = DeviceAxisMode.Absolute;
			joystick.Acquire();

			return joystick;
		}
	}

	public class InputDeviceProfile : IInputDeviceProfile
	{
		public string Name { get; set; }
		public IEnumerable<IValueMapping> Mappings { get; }

		public object GetMappedValue(IInputDevice inputDevice, string target)
		{
			var mapping = Mappings.FirstOrDefault(m => m.Target == target);
			if (mapping != null)
			{
				var value = mapping.GetValue(inputDevice);
				return value;
			}
			else
			{
				throw new KeyNotFoundException(target);
			}
		}

		public TValue GetMappedValue<TValue>(IInputDevice inputDevice, string target)
		{
			var mapping = Mappings.FirstOrDefault(m => m.Target == target);
			if (mapping != null)
			{
				var value = mapping.GetValue<TValue>(inputDevice);
				return value;
			}
			else
			{
				throw new KeyNotFoundException(target);
			}
		}

		public object GetMappedValueOrDefault(IInputDevice inputDevice, string target, object defaultValue)
		{
			var mapping = Mappings.FirstOrDefault(m => m.Target == target);
			if (mapping != null)
			{
				var value = mapping.GetValue(inputDevice);
				return value;
			}
			else
			{
				return defaultValue;
			}
		}

		public TValue GetMappedValueOrDefault<TValue>(IInputDevice inputDevice, string target, TValue defaultValue)
		{
			var mapping = Mappings.FirstOrDefault(m => m.Target == target);
			if (mapping != null)
			{
				var value = mapping.GetValue<TValue>(inputDevice);
				return value;
			}
			else
			{
				return defaultValue;
			}
		}
	}

	public interface IProfiledInputDevice
	{
		void Update();

		void Reset();

		TValue GetValue<TValue>(string target);

		TValue GetValueOrDefault<TValue>(string target, TValue defaultValue);
	}

	public class ProfiledInputDevice : IProfiledInputDevice
	{
		private readonly IInputDevice _inputDevice;
		private readonly IInputDeviceProfile _inputDeviceProfile;
		private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

		public ProfiledInputDevice(IInputDevice inputDevice, IInputDeviceProfile inputDeviceProfile)
		{
			if (inputDevice == null)
			{
				throw new ArgumentNullException(nameof(inputDevice));
			}
			if (inputDeviceProfile == null)
			{
				throw new ArgumentNullException(nameof(inputDeviceProfile));
			}

			_inputDevice = inputDevice;
			_inputDeviceProfile = inputDeviceProfile;

			Reset();
		}

		public void Update()
		{
			_inputDevice.Update();

			if (_inputDeviceProfile.Mappings != null)
			{
				foreach (var mapping in _inputDeviceProfile.Mappings)
				{
					_values[mapping.Target] = _inputDeviceProfile.GetMappedValue(_inputDevice, mapping.Target);
				}
			}
		}

		public void Reset()
		{
			if (_inputDeviceProfile.Mappings != null)
			{
				foreach (var mapping in _inputDeviceProfile.Mappings)
				{
					_values[mapping.Target] = mapping.InitialValue;
				}
			}
		}

		public TValue GetValue<TValue>(string target)
		{
			var value = _values[target];
			var convertedValue = ConvertValue<TValue>(value);
			return convertedValue;
		}

		public TValue GetValueOrDefault<TValue>(string target, TValue defaultValue)
		{
			if (_values.ContainsKey(target))
			{
				var value = _values[target];
				var convertedValue = ConvertValue<TValue>(value);
				return convertedValue;
			}
			else
			{
				return defaultValue;
			}
		}

		private static T ConvertValue<T>(object value)
		{
			return (T) Convert.ChangeType(value, typeof (T));
		}
	}

	public interface IInputDevice
	{
		object[] Values { get; }

		bool Update();
	}

	public class InputDevice : IInputDevice
	{
		private readonly Joystick _joystick;

		public InputDevice(Joystick joystick)
		{
			if (joystick == null)
			{
				throw new ArgumentNullException(nameof(joystick));
			}

			_joystick = joystick;
		}

		public bool Update()
		{
			throw new NotImplementedException();
		}

		public object[] Values { get; }
	}

	public interface IInputDeviceProfile
	{
		string Name { get; }
		IEnumerable<IValueMapping> Mappings { get; }

		object GetMappedValue(IInputDevice inputDevice, string target);

		TValue GetMappedValue<TValue>(IInputDevice inputDevice, string target);

		object GetMappedValueOrDefault(IInputDevice inputDevice, string target, object defaultValue);

		TValue GetMappedValueOrDefault<TValue>(IInputDevice inputDevice, string target, TValue defaultValue);
	}

	public interface IValueMapping
	{
		string Target { get; }
		object InitialValue { get; }

		object GetValue(IInputDevice inputDevice);

		TValue GetValue<TValue>(IInputDevice inputDevice);
	}

	public abstract class ValueMapping : IValueMapping, IEquatable<ValueMapping>
	{
		protected ValueMapping()
		{
		}

		protected ValueMapping(int inputIndex, string target, object initialValue, object initialValue1)
		{
			Target = target;
			CurrentValue = InitialValue = initialValue;
			InputIndex = inputIndex;
		}

		public int InputIndex { get; }
		public object CurrentValue { get; private set; }

		public bool Equals(ValueMapping other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return InputIndex == other.InputIndex && Equals(Target, other.Target);
		}

		public string Target { get; }
		public object InitialValue { get; }

		public object GetValue(IInputDevice inputDevice)
		{
			CurrentValue = ResolveValue(CurrentValue, inputDevice);
			return CurrentValue;
		}

		public TValue GetValue<TValue>(IInputDevice inputDevice)
		{
			GetValue(inputDevice);
			var convertedValue = ConvertValue<TValue>(CurrentValue);
			return convertedValue;
		}

		protected abstract object ResolveValue(object previouslyResolvedValue, IInputDevice inputDevice);

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((ValueMapping) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (InputIndex * 397) ^ (Target != null ? Target.GetHashCode() : 0);
			}
		}

		private static T ConvertValue<T>(object value)
		{
			return (T) Convert.ChangeType(value, typeof (T));
		}

		public override string ToString()
		{
			return $"[InputIndex: {InputIndex}, Target: {Target}, InitialValue: {InitialValue}, CurrentValue: {CurrentValue}]";
		}

		public static bool operator ==(ValueMapping left, ValueMapping right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(ValueMapping left, ValueMapping right)
		{
			return !Equals(left, right);
		}
	}
}