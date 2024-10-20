using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.HID.HID;


namespace IgnizMelange
{
	public class Main : MonoBehaviour
    {
		public static ConfigEntry<bool> EndlessWar;
		public static ConfigEntry<bool> GoldChanges;

		public void Awake()
        {
            Debug.Log("Loaded IgnizMelange Successfully");
        }

		// This method ends wars when both sides are broke, we dont want that.
		[HarmonyPatch(typeof(Events), "BothAreBroke")]
		public class BrokePatch
		{
			public static void Postfix(ref bool __result)
			{
				if (GoldChanges.Value == true)
				{
					__result = false;
				}
			}

		}

		// Forces all wars to death
		[HarmonyPatch(typeof(Events), "TryWar")]
		public class TryWarPatch
		{

			public static void Prefix(ref bool toDeath)
			{
				if (EndlessWar.Value == true)
				{
					toDeath = true;
				}
			}

		}

		// You can technically do this as a transpiler for more compatibility, but bah I will deal with this later
		// This method adjusts gold logic for wars that never end
		[HarmonyPatch(typeof(NationLogic), "UpdateGoldAndRevoltPercentage")]
		public static class GoldPatch
		{
			static bool Prefix(NationLogic __instance)
			{
				//Use normal method if no gold changes
				if (GoldChanges.Value == false)
				{
					return true;
				}

				Nation me = __instance.me;

				float deltaTime = UnityEngine.Time.deltaTime;
				float landValueGoldModifier = me.GetLandValueGoldModifier();

				if (me.wars.Count < 1)
				{
					me.goldf += deltaTime * landValueGoldModifier;
					me.revoltPercent = Mathf.Max(me.revoltPercent - deltaTime * 0.1f, 0f);
				}
				else
				{
					float gold = me.goldf;
					float enemyGold = 0f;

					// Aggregate enemy gold from all war enemies
					foreach (var v in me.GetWarEnemies())
					{
						enemyGold += Events.allNations[v].goldf;
					}

					// Adjust combat efficiency based on gold comparison
					if (gold > enemyGold * 2)
					{
						me.combatEfficiency = Mathf.Clamp(me.combatEfficiency, 0, 6);
					}
					if (gold > enemyGold * 4)
					{
						me.combatEfficiency = Mathf.Clamp(me.combatEfficiency, 0, 2);
					}

					// Increase gold but at a slower rate during war
					me.goldf += deltaTime * landValueGoldModifier / 3f;

					// Make losing gold during war 
					me.goldf -= deltaTime * me.GetWarGoldModifier() / 5f;

					// Adjust combat efficiency when gold is low
					if (gold < 50f)
					{
						me.combatEfficiency = Mathf.Clamp(me.combatEfficiency, 8, 12);
					}
					// Make defenseless when low gold
					if (gold < 10f)
					{
						me.combatEfficiency = 12;
					}
				}

				// Ensure gold doesn't fall below 0
				me.goldf = Mathf.Max(me.goldf, 0f);

				return false;
			}
		}
	}
}
