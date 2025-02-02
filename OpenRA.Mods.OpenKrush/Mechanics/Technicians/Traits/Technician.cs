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

using System.Collections.Generic;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.OpenKrush.Mechanics.Bunkers.Traits;
using OpenRA.Mods.OpenKrush.Mechanics.Technicians.Activities;
using OpenRA.Mods.OpenKrush.Mechanics.Technicians.Orders;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenKrush.Mechanics.Technicians.Traits
{
	[Desc("Technician mechanism, attach to the unit.")]
	public class TechnicianInfo : TraitInfo
	{
		[Desc("Cursor used for order.")]
		public readonly string Cursor = "repair";

		[Desc("Cursor used for order if building does not need repair.")]
		public readonly string BlockedCursor = "repair-blocked";

		[Desc("Target line color.")]
		public readonly Color TargetLineColor = Color.Yellow;

		[Desc("Voice used when ordering to repair.")]
		[VoiceReference]
		public readonly string VoiceOrder = "Repair";

		[Desc("Voice used when entered and starting repair.")]
		[VoiceReference]
		public readonly string VoiceEnter = "Repairing";

		[Desc("Voice used when entered a bunker.")]
		[VoiceReference]
		public readonly string VoiceEnterBunker = null;

		public override object Create(ActorInitializer init)
		{
			return new Technician(this);
		}
	}

	public class Technician : IIssueOrder, IResolveOrder, IOrderVoice
	{
		private readonly TechnicianInfo info;

		public Technician(TechnicianInfo info)
		{
			this.info = info;
		}

		IEnumerable<IOrderTargeter> IIssueOrder.Orders
		{
			get { yield return new TechnicianEnterOrderTargeter(info.Cursor, info.BlockedCursor); }
		}

		Order IIssueOrder.IssueOrder(Actor self, IOrderTargeter order, in Target target, bool queued)
		{
			return order.OrderID == TechnicianEnterOrderTargeter.Id ? new Order(order.OrderID, self, target, queued) : null;
		}

		void IResolveOrder.ResolveOrder(Actor self, Order order)
		{
			if (order.OrderString != TechnicianEnterOrderTargeter.Id)
				return;

			if (order.Target.Type != TargetType.Actor || order.Target.Actor == null)
				return;

			if (!TechnicianUtils.CanEnter(self, order.Target.Actor))
				return;

			self.QueueActivity(order.Queued, new TechnicianEnter(self, order.Target, info.TargetLineColor));
		}

		string IOrderVoice.VoicePhraseForOrder(Actor self, Order order)
		{
			return order.OrderString == TechnicianEnterOrderTargeter.Id ? info.VoiceOrder : null;
		}

		public void Enter(Actor self, Actor targetActor)
		{
			if (self.Owner != self.World.LocalPlayer)
				return;

			if (self.World.WorldActor.Info.TraitInfo<TechBunkerAmountInfo>().ActorType != targetActor.Info.Name)
				self.PlayVoice(info.VoiceEnter);
			else if (info.VoiceEnterBunker != null)
				self.PlayVoice(info.VoiceEnterBunker);
		}
	}
}
