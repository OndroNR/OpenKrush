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

namespace OpenRA.Mods.OpenKrush.Mechanics.Docking
{
	public enum DockingState
	{
    	None,
    	Approaching,
    	Waiting,
    	PrepareDocking,
    	Docking,
    	Docked,
    	Undocking
	}
}
