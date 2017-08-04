using System;
using System.Collections.Generic;
using AquaServer.Service.Models.Enums;

namespace AquaServer.PresentationServices.ZoneFormatters
{
	public abstract class ZoneFormatBase
	{
		private static readonly Dictionary<Devices, ZoneFormatBase> _formats = new Dictionary<Devices, ZoneFormatBase>();

		public virtual string GetColor(DeviceStates deviceState)
		{
			return null;
		}

		public virtual string GetDashStyle(DeviceStates deviceState)
		{
			return null;
		}

		public static ZoneFormatBase GetFormat(Devices device)
		{
			ZoneFormatBase format;
			if (_formats.TryGetValue(device, out format))
			{
				return format;
			}

			format = createFormat(device);
			_formats[device] = format;

			return format;
		}

		private static ZoneFormatBase createFormat(Devices device)
		{
			switch (device)
			{
				case Devices.Cooler:
					return new CoolerZoneFormat();
				case Devices.Heater:
					return new HeaterZoneFormat();
			}

			throw new NotImplementedException();
		}
	}
}