using HarmonyLib;
using InControl;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalismanSwitch.HarmonyPatches {

    [HarmonyPatch(typeof(PlayerInputBinder))]
    internal static class PlayerInputBinderPatches {

        internal static Action<InputDevice>? OnSetDeviceAwake;
        internal static Action? OnBindingControlPostfix;
        internal static Action? OnAwakePostfix;

        [HarmonyPatch(nameof(PlayerInputBinder.SetDevice))]
        [HarmonyPostfix]
        private static void SetDevice_Postfix(InputDevice device) {
            try {
                OnSetDeviceAwake?.Invoke(device);
            } catch (Exception e) {
                Log.Exception(e);
            }
        }

        [HarmonyPatch("BindingControl")]
        [HarmonyPostfix]
        private static void BindingControl_Postfix() {
            try {
                OnBindingControlPostfix?.Invoke();
            } catch (Exception e) {
                Log.Exception(e);
            }
        }

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void Awake_Postfix() {
            try {
                OnAwakePostfix?.Invoke();
            } catch (Exception e) {
                Log.Exception(e);
            }
        }

        [HarmonyPatch("FindAction")]
        [HarmonyPostfix]
        private static void FindAction_Postfix(ActionSetType actionSetType, string actionName, ref PlayerAction __result) {
            if(actionSetType == ModConfig.MOD_INPUT_ACTION_SET_TYPE) {
                __result = TalismanSwitch.Instance.modInputHandler.modActionSet.GetPlayerActionByName(actionName);
            }
        }
    }
}
