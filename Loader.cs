using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace IgnizMelange
{
    [BepInPlugin("org.IgnizHerz.mod.IgnizMelange", "IgnizMelange", "0.1.0.0")]
    class Loader : BaseUnityPlugin
    {
		public static bool setup = false;
		public static TMP_FontAsset roboto;

        void Awake()
        {
			Harmony harmony = new Harmony("IgnizMelange");
			harmony.PatchAll();
			LoadConfig();
		}

		public void LoadConfig()
		{
			Main.EndlessWar = Config.Bind("SETTINGS",   // The section under which the option is shown
			"EndlessWar",  // The key of the configuration option in the configuration file
			true, // The default value
			"Makes every war an endless war."); // Description of the option to show in the config file

			Main.GoldChanges = Config.Bind("SETTINGS",
			"GoldChanges",
			true,
			"Makes wars not force peace at 0 gold, and adjusts gold production and combat effeciency in wars to help prevent stalemates.");
		}

		[HarmonyPatch(typeof(MainMenu), "Start")]
		public class StartupPatch
		{

			public static void Postfix()
			{
				if (!setup)
				{
					var go = new GameObject("IgnizMelange");
					UnityEngine.Object.DontDestroyOnLoad(go);
					go.AddComponent<Main>();
					Debug.LogError("IgnizMelange Loaded Successfully!");
				}
				setup = true;
			}

		}
	}
}
