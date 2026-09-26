using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalismanSwitch.HarmonyPatches {
    [HarmonyPatch(typeof(Player))]
    internal static class PlayerPatches {

        internal static event Action<Player>? OnPlayerAwakePostfix;
        
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void Awake_Postfix(Player __instance) {
            try {
                OnPlayerAwakePostfix?.Invoke(__instance);
            } catch (Exception e) {
                Log.Exception(e);
            }
        }
    }
}
