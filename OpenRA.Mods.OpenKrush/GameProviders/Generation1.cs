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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenRA.Mods.OpenKrush.GameProviders
{
	public static class Generation1
	{
		public const int AppIdGog = 1207659107;
		public const int AppIdSteam = 1292170;

		public static bool TryRegister(string path)
    	{
   	 	var executablePath = path;

   	 	// Xtreme GoG and Steam
   	 	var executable = GameProvider.GetFile(executablePath, "kkndgame.exe");

   	 	if (executable == null)
   	 	{
  	 	 	// Xtreme CD
  	 	 	executablePath = GameProvider.GetDirectory(path, "game");

  	 	 	if (executablePath == null)
  	 	 	{
 	 	 	 	// Dos CD
 	 	 	 	executablePath = GameProvider.GetDirectory(path, "kknd");

 	 	 	 	if (executablePath == null)
	 	 	 	 	return false;
  	 	 	}

  	 	 	executable = GameProvider.GetFile(executablePath, "kknd.exe");

  	 	 	if (executable == null)
 	 	 	 	return false;
   	 	}

   	 	Log.Write("debug", $"Detected installation: {path}");

   	 	var release = CryptoUtil.SHA1Hash(File.OpenRead(executable));
   	 	var isXtreme = true;

   	 	switch (release)
   	 	{
  	 	 	case "d1f41d7129b6f377869f28b89f92c18f4977a48f":
 	 	 	 	Log.Write("debug", "=> Krush, Kill 'N' Destroy Xtreme (Steam/GoG, English)");
 	 	 	 	break;

  	 	 	case "6fb10d85739ef63b28831ada4cdfc159a950c5d2":
 	 	 	 	Log.Write("debug", "=> Krush, Kill 'N' Destroy Xtreme (Disc, English)");
 	 	 	 	break;

  	 	 	case "024e96860c504b462b24b9237d49bfe8de6eb8e0":
 	 	 	 	Log.Write("debug", "=> Krush, Kill 'N' Destroy (Disc, English)");
 	 	 	 	isXtreme = false;
 	 	 	 	path = executablePath;
 	 	 	 	break;

  	 	 	default:
 	 	 	 	Log.Write("debug", "=> Unsupported game version");
 	 	 	 	return false;
   	 	}

   	 	var levelsFolder = GameProvider.GetDirectory(path, "levels");

   	 	if (levelsFolder == null)
   	 	{
  	 	 	Log.Write("debug", "=> Missing folder: levels");
  	 	 	return false;
   	 	}

   	 	var fmvFolder = GameProvider.GetDirectory(path, "fmv");

   	 	if (fmvFolder == null)
   	 	{
  	 	 	Log.Write("debug", "=> Missing folder: fmv");
  	 	 	return false;
   	 	}

   	 	var graphicsFolder = GameProvider.GetDirectory(levelsFolder, "640");

   	 	if (graphicsFolder == null)
   	 	{
  	 	 	Log.Write("debug", "=> Missing folder: 640");
  	 	 	return false;
   	 	}

   	 	// Required files.
   	 	var files = new Dictionary<string, string>
   	 	{
  	 	 	{ "sprites.lvl", graphicsFolder },
  	 	 	{ "surv.slv", levelsFolder },
  	 	 	{ "mute.slv", levelsFolder },
  	 	 	{ "mh_fmv.vbc", fmvFolder },
  	 	 	{ "intro.vbc", fmvFolder }
   	 	}.Concat(isXtreme
  	 	 	? new Dictionary<string, string>
  	 	 	{
 	 	 	 	{ "surv1.wav", levelsFolder },
 	 	 	 	{ "surv2.wav", levelsFolder },
 	 	 	 	{ "surv3.wav", levelsFolder },
 	 	 	 	{ "surv4.wav", levelsFolder },
 	 	 	 	{ "mute1.wav", levelsFolder },
 	 	 	 	{ "mute2.wav", levelsFolder },
 	 	 	 	{ "mute3.wav", levelsFolder },
 	 	 	 	{ "mute4.wav", levelsFolder }
  	 	 	}
  	 	 	: new Dictionary<string, string>
  	 	 	{
 	 	 	 	{ "surv1.son", levelsFolder },
 	 	 	 	{ "surv2.son", levelsFolder },
 	 	 	 	{ "surv3.son", levelsFolder },
 	 	 	 	{ "mute1.son", levelsFolder },
 	 	 	 	{ "mute2.son", levelsFolder },
 	 	 	 	{ "mute3.son", levelsFolder },
  	 	 	}).ToDictionary(e => e.Key, e => GameProvider.GetFile(e.Value, e.Key));

   	 	var missingFiles = files.Where(e => e.Value == null).Select(e => e.Key).ToArray();

   	 	if (missingFiles.Any())
   	 	{
  	 	 	foreach (var missingFile in missingFiles)
 	 	 	 	Log.Write("debug", $"=> Missing file: {missingFile}");

  	 	 	return false;
   	 	}

   	 	GameProvider.Installation = path;
   	 	GameProvider.Packages.Add(files["sprites.lvl"], "sprites.lvl");
   	 	GameProvider.Packages.Add(files["mute.slv"], "mute.slv");
   	 	GameProvider.Packages.Add(files["surv.slv"], "surv.slv");

   	 	GameProvider.Music.Add("Survivors 1", files[$"surv1.{(isXtreme ? "wav" : "son")}"]);
   	 	GameProvider.Music.Add("Survivors 2", files[$"surv2.{(isXtreme ? "wav" : "son")}"]);
   	 	GameProvider.Music.Add("Survivors 3", files[$"surv3.{(isXtreme ? "wav" : "son")}"]);
   	 	GameProvider.Music.Add("Evolved 1", files[$"mute1.{(isXtreme ? "wav" : "son")}"]);
   	 	GameProvider.Music.Add("Evolved 2", files[$"mute2.{(isXtreme ? "wav" : "son")}"]);
   	 	GameProvider.Music.Add("Evolved 3", files[$"mute3.{(isXtreme ? "wav" : "son")}"]);

   	 	if (isXtreme)
   	 	{
  	 	 	GameProvider.Music.Add("Survivors 4", files["surv4.wav"]);
  	 	 	GameProvider.Music.Add("Evolved 4", files["mute4.wav"]);
   	 	}

   	 	GameProvider.Movies.Add("mh.vbc", files["mh_fmv.vbc"]);
   	 	GameProvider.Movies.Add("intro.vbc", files["intro.vbc"]);

   	 	// Any other container for asset browser purpose
   	 	GameProvider.Packages.Add(levelsFolder, null);
   	 	GameProvider.Packages.Add(fmvFolder, null);

   	 	foreach (var file in Directory.GetFiles(levelsFolder).Concat(Directory.GetFiles(graphicsFolder)).Where(f =>
  	 	 	f.EndsWith(".slv", StringComparison.OrdinalIgnoreCase) ||
  	 	 	f.EndsWith(".lvl", StringComparison.OrdinalIgnoreCase)))
   	 	{
  	 	 	if (!GameProvider.Packages.ContainsKey(file))
 	 	 	 	GameProvider.Packages.Add(file, Path.GetFileName(file).ToLower());
   	 	}

   	 	return true;
    	}
	}
}
