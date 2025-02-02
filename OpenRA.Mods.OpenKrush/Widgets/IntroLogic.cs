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

using OpenRA.GameRules;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.OpenKrush.FileFormats;
using OpenRA.Mods.OpenKrush.GameProviders;
using OpenRA.Primitives;
using OpenRA.Widgets;

namespace OpenRA.Mods.OpenKrush.Widgets
{
	public class IntroLogic : ChromeLogic
	{
		private static int state;

		private readonly Widget widget;
		private readonly ModData modData;
		private readonly World world;
		private readonly VbcPlayerWidget player;
		private MusicInfo song;

		[ObjectCreator.UseCtorAttribute]
		public IntroLogic(Widget widget, World world, ModData modData)
		{
			if (state != 0)
				return;

			this.widget = widget;
			this.world = world;
			this.modData = modData;

			widget.AddChild(player = new VbcPlayerWidget());

			player.Bounds = new Rectangle(0, 0, Game.Renderer.Resolution.Width, Game.Renderer.Resolution.Height);

			var musicPlayList = world.WorldActor.Trait<MusicPlaylist>();
			song = musicPlayList.CurrentSong();
			musicPlayList.Stop();
		}

		public override void Tick()
		{
			if (state == 0)
				PlayVideo(GameProvider.Movies.ContainsKey("mh_fmv.vbc") ? GameProvider.Movies["mh_fmv.vbc"] : GameProvider.Movies["mh.vbc"]);
			else if (state == 2)
				PlayVideo(GameProvider.Movies["intro.vbc"]);
			else if (state == 4)
			{
				widget.RemoveChild(player);
				state++;

				if (song != null)
					world.WorldActor.Trait<MusicPlaylist>().Play(song);
			}
		}

		private void PlayVideo(string video)
		{
			state++;

			if (!modData.ModFiles.TryOpen(video, out var stream))
			{
				state++;
				return;
			}

			player.Video = new Vbc(stream);
			player.Play(() => state++);
		}
	}
}
