using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalismanSwitch.HarmonyPatches {

    [HarmonyPatch(typeof(SaveManager))]
    internal static class SaveManagerPatches {
        internal static event Action<SaveManager>? OnSaveInputBindingPostfix;

        [HarmonyPatch(nameof(SaveManager.SaveInputBinding))]
        [HarmonyPostfix]
        private static void SaveInputBinding_Postfix(SaveManager __instance) {
            try {
                OnSaveInputBindingPostfix?.Invoke(__instance);
            } catch(Exception e) {
                Log.Exception(e);
            }
        }
    }
}
