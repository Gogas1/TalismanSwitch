using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalismanSwitch.HarmonyPatches {

    [HarmonyPatch(typeof(PlayerControlBindingPanel))]
    internal static class PlayerControlBindingPanelPatches {

        internal static event Action<PlayerControlBindingPanel>? OnStartPostfix;
        
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void Start_Postfix(PlayerControlBindingPanel __instance) {
            try {
                OnStartPostfix?.Invoke(__instance);
            } catch (Exception e) {
                Log.Exception(e);
            }
        }
    }
}
