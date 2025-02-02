#region Copyright & License Information
/*
 * Copyright 2007-2021 The OpenKrush Developers (see AUTHORS)
 * This file is part of OpenKrush, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

namespace OpenRA.Mods.OpenKrush.Mechanics.Docking.Traits.Dockables
{
	public class RepairableVehicleInfo : DockableInfo
	{
		public override object Create(ActorInitializer init) { return new RepairableVehicle(this); }
	}

	public class RepairableVehicle : Dockable
	{
		public RepairableVehicle(RepairableVehicleInfo info)
			: base(info) { }
	}
}
