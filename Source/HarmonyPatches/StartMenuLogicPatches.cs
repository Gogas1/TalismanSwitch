using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalismanSwitch.HarmonyPatches {

    [HarmonyPatch(typeof(StartMenuLogic))]
    internal static class StartMenuLogicPatches {

        internal static event Action<StartMenuLogic>? OnStartMenuAwake;

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void Awake_Postfix(StartMenuLogic __instance) {
            try {
                OnStartMenuAwake?.Invoke(__instance);
            } catch (Exception e) {
                Log.Exception(e);
            }
        }
    }
}
