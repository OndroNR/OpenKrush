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
using OpenRA.Mods.Common.Traits.Render;
using OpenRA.Traits;

namespace OpenRA.Mods.OpenKrush.Traits.Render
{
	public class WithAimAttackAnimationInfo : TraitInfo, Requires<WithSpriteBodyInfo>
	{
		[Desc("Displayed while attacking.")]
		[SequenceReference]
		public readonly string SequenceFire = null;

		[Desc("Displayed while attacking.")]
		[SequenceReference]
		public readonly string SequenceAim = null;

		public override object Create(ActorInitializer init) { return new WithAimAttackAnimation(init, this); }
	}

	public class WithAimAttackAnimation : ITick, INotifyAttack, INotifyAiming
	{
		WithAimAttackAnimationInfo info;
		WithSpriteBody wsb;
		bool aiming;

		public WithAimAttackAnimation(ActorInitializer init, WithAimAttackAnimationInfo info)
		{
			this.info = info;
			wsb = init.Self.Trait<WithSpriteBody>();
		}

		void INotifyAttack.Attacking(Actor self, in Target target, Armament a, Barrel barrel)
		{
			wsb.PlayCustomAnimation(self, info.SequenceFire);
		}

		void INotifyAttack.PreparingAttack(Actor self, in Target target, Armament a, Barrel barrel) { }

		void ITick.Tick(Actor self)
		{
			if (!string.IsNullOrEmpty(info.SequenceAim) && aiming && wsb.DefaultAnimation.CurrentSequence.Name == "idle")
				wsb.PlayCustomAnimation(self, info.SequenceAim);
		}

		void INotifyAiming.StartedAiming(Actor self, AttackBase attack)
		{
			aiming = true;
		}

		void INotifyAiming.StoppedAiming(Actor self, AttackBase attack)
		{
			aiming = false;
		}
	}
}
