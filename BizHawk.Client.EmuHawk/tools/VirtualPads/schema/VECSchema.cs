﻿using System.Collections.Generic;
using System.Drawing;

using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Consoles.Vectrex;

namespace BizHawk.Client.EmuHawk
{
	[Schema("VEC")]
	// ReSharper disable once UnusedMember.Global
	public class VecSchema : IVirtualPadSchema
	{
		public IEnumerable<PadSchema> GetPadSchemas(IEmulator core)
		{
			var vecSyncSettings = ((VectrexHawk)core).GetSyncSettings().Clone();
			var port1 = vecSyncSettings.Port1;
			var port2 = vecSyncSettings.Port2;

			switch (port1)
			{
				case "Vectrex Digital Controller":
					yield return StandardController(1);
					break;
				case "Vectrex Analog Controller":
					yield return AnalogController(1);
					break;
			}

			switch (port2)
			{
				case "Vectrex Digital Controller":
					yield return StandardController(2);
					break;
				case "Vectrex Analog Controller":
					yield return AnalogController(2);
					break;
			}

			yield return ConsoleButtons();
		}

		private static PadSchema StandardController(int controller)
		{
			return new PadSchema
			{
				Size = new Size(200, 100),
				Buttons = new[]
				{
					ButtonSchema.Up(14, 12, controller),
					ButtonSchema.Down(14, 56, controller),
					ButtonSchema.Left(2, 34, controller),
					ButtonSchema.Right(24, 34, controller),
					Button(74, 34, controller, 1),
					Button(98, 34, controller, 2),
					Button(122, 34, controller, 3),
					Button(146, 34, controller, 4)
				}
			};
		}

		private static PadSchema AnalogController(int controller)
		{
			var controllerDefRanges = new AnalogControls(controller).Definition.AxisRanges;
			return new PadSchema
			{
				Size = new Size(280, 300),
				Buttons = new[]
				{
					Button(74, 34, controller, 1),
					Button(98, 34, controller, 2),
					Button(122, 34, controller, 3),
					Button(146, 34, controller, 4),
					new AnalogSchema(2, 80, $"P{controller} Stick X")
					{
						AxisRange = controllerDefRanges[0],
						SecondaryAxisRange = controllerDefRanges[1]
					}
				}
			};
		}

		private static ButtonSchema Button(int x, int y, int controller, int button)
		{
			return new ButtonSchema(x, y, controller, $"Button {button}")
			{
				DisplayName = button.ToString()
			};
		}

		private static PadSchema ConsoleButtons()
		{
			return new ConsoleSchema
			{
				Size = new Size(150, 50),
				Buttons = new[]
				{
					new ButtonSchema(10, 15, "Reset"),
					new ButtonSchema(58, 15, "Power")
				}
			};
		}
	}
}
