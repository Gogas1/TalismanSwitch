using InControl;
using System;
using System.Collections.Generic;
using System.Text;
using TalismanSwitch.HarmonyPatches;
using UnityEngine;

namespace TalismanSwitch.Handlers {
    internal class ModInputHandler : MonoBehaviour {
        internal TalismanSwitchActionSet modActionSet = null!;

        internal event Action? OnSwitchTalismanPressed;

        private void Awake() {
            PlayerInputBinderPatches.OnSetDeviceAwake += HandleDeviceSet;
            PlayerInputBinderPatches.OnBindingControlPostfix += HandleControlsBinding;
            SaveManagerPatches.OnSaveInputBindingPostfix += HandleSaveInputBinding;
        }

        private void HandleSaveInputBinding(SaveManager _) {
            SavePrefs();
        }

        private void Update() {
            if (modActionSet != null) {
                modActionSet.Update();
            }
        }

        private void HandleControlsBinding() {
            InitControls();
        }

        private void HandleDeviceSet(InputDevice device) {
            InitControls();
            modActionSet.Device = device;
        }


        private void InitControls() {
            if (modActionSet == null) {
                modActionSet = new TalismanSwitchActionSet();
                modActionSet.Initialize();
                modActionSet.OnSwitchTalismanPressed += OnSwitchTalismanPressed;
            }
            LoadPrefs();
        }

        private void LoadPrefs() {
            if(PlayerPrefs.HasKey(ModConfig.PLAYER_PREFS_KEY)) {
                var bindingData = PlayerPrefs.GetString(ModConfig.PLAYER_PREFS_KEY);
                modActionSet.Load(bindingData);
            }
        }

        private void SavePrefs() {
            var bindingData = modActionSet.Save();
            PlayerPrefs.SetString(ModConfig.PLAYER_PREFS_KEY, bindingData);
        }

        private void OnDestroy() {
            PlayerInputBinderPatches.OnSetDeviceAwake -= HandleDeviceSet;
            PlayerInputBinderPatches.OnBindingControlPostfix -= HandleControlsBinding;

            SaveManagerPatches.OnSaveInputBindingPostfix -= HandleSaveInputBinding;
        }
    }
}
