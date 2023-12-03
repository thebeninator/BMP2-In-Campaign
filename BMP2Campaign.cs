using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using HarmonyLib;
using BMP2Campaign;
using UnityEngine;
using System.Reflection;

[assembly: MelonInfo(typeof(BMP2CampaignMod), "BMP-2 Campaign", "1.0.0", "ATLAS")]
[assembly: MelonGame("Radian Simulations LLC", "GHPC")]

namespace BMP2Campaign
{
    public class BMP2CampaignMod : MelonMod
    {
        public static MelonPreferences_Category cfg;
        public static MelonPreferences_Entry<int> chance;

        public override void OnInitializeMelon()
        {
            cfg = MelonPreferences.CreateCategory("BMP-2 Campaign");
            chance = cfg.CreateEntry("Replacement Chance", 100);
            chance.Description = "% chance (integer) a BMP-1 will be replaced by a BMP-2";
        }

        /*
        [HarmonyPatch(typeof(GHPC.Mission.DynamicMissionComposer), "ResolveFlexSpawn")]
        public static class fghb
        {
            private static void Prefix(GHPC.Mission.DynamicMissionComposer __instance, object[] __args)
            {
                MethodInfo get_key = typeof(GHPC.Mission.DynamicMissionComposer).GetMethod("GetFirstRegisteredKeyForSpawnPoint", BindingFlags.Instance | BindingFlags.NonPublic);
                object[] args = new object[] { __args[0], __args[1] };
                string firstRegisteredKeyForSpawnPoint = (string)get_key.Invoke(__instance, args);
                MelonLogger.Msg(firstRegisteredKeyForSpawnPoint);
            }
        }
        */

        [HarmonyPatch(typeof(GHPC.Mission.DynamicMissionComposer), "GetFirstRegisteredKeyForSpawnPoint")]
        public static class ReplaceBMP1
        {
            private static void Postfix(GHPC.Mission.DynamicMissionComposer __instance, ref string __result)
            {
                bool is_BMP = __result == "BMP1" || __result == "BMP1P";
                if (is_BMP && UnityEngine.Random.Range(1, 100) <= chance.Value) { 
                    __result = "BMP2"; 
                }
            }
        }
    }
}
