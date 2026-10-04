using HarmonyLib;
using I2.Loc;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalismanSwitch.HarmonyPatches {

    [HarmonyPatch(typeof(LocalizationManager))]
    internal static class LocalizationManagerPatches {

        internal static event Action<LanguageSourceData>? OnAddSource;

        [HarmonyPatch("AddSource"), HarmonyPostfix]
        private static void AddSourcePostfix(LanguageSourceData Source) {
            OnAddSource?.Invoke(Source);
        }
    }
}
