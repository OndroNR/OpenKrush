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

using OpenRA.Mods.Common.Traits;

namespace OpenRA.Mods.OpenKrush.Mechanics.Docking.Traits
{
	public abstract class DockActionInfo : ConditionalTraitInfo
	{
		[Desc("Cursor to use when docking is possible.")]
		public readonly string Cursor = "dock";

		[Desc("Name of the dock this action is assigned to..")]
		public readonly string Name = "Dock";
	}

	public abstract class DockAction : ConditionalTrait<DockActionInfo>
	{
		protected DockAction(DockActionInfo info)
			: base(info) { }

		public abstract bool CanDock(Actor actor);

		public virtual void OnDock() { }
		public abstract bool Process(Actor actor);
		public virtual void OnUndock() { }
	}
}
