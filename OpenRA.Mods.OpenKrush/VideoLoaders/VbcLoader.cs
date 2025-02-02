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

using System.IO;
using OpenRA.Mods.OpenKrush.FileFormats;
using OpenRA.Video;

namespace OpenRA.Mods.OpenKrush.VideoLoaders
{
	public class VbcLoader : IVideoLoader
	{
		public bool TryParseVideo(Stream s, out IVideo video)
		{
			video = null;

			if (!IsVbc(s))
				return false;

			video = new Vbc(s);
			return true;
		}

		private static bool IsVbc(Stream s)
		{
			var start = s.Position;

			if (s.ReadASCII(4) != "SIFF")
			{
				s.Position = start;
				return false;
			}

			s.Position = start;
			return true;
		}
	}
}
