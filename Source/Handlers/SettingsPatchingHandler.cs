using HarmonyLib;
using I2.Loc;
using NineSolsAPI.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TalismanSwitch.HarmonyPatches;
using UnityEngine;

namespace TalismanSwitch.Handlers {
    internal class SettingsPatchingHandler {

        private FieldInfo _playerInputActionDataMyNameField = AccessTools.Field(typeof(PlayerInputActionData), "_myName");


        public SettingsPatchingHandler() {
            PlayerControlBindingPanelPatches.OnStartPostfix += HandleControlBindingPanelStart;
        }

        private void HandleControlBindingPanelStart(PlayerControlBindingPanel obj) {
            if(obj.name == ModConfig.KBM_CONTROLS_PANEL_NAME) {
                PatchKBMControlsPanel(obj);
            }

            if(obj.name == ModConfig.CONTROLLER_CONTROLS_PANEL_NAME) {
                PatchControllerControlsPanel(obj);
            }
        }

        private void PatchKBMControlsPanel(PlayerControlBindingPanel panel) {
            var baseButton = panel.transform.Find("Layout/Table/Col 2/Map");
            if (baseButton == null) {
                throw new InvalidOperationException();
            }

            CloneButton(baseButton.gameObject);
        }

        private void PatchControllerControlsPanel(PlayerControlBindingPanel panel) {
            var baseButton = panel.transform.Find("Layout/Table/Col 2/Arrow Previous Remap Control ActionBindingUI (8)");
            if (baseButton == null) {
                throw new InvalidOperationException();
            }

            CloneButton(baseButton.gameObject);
        }

        private ControlsButtonDescriptor CloneButton(GameObject baseButton) {
            var clonedButton = GameObject.Instantiate(baseButton, baseButton.transform.parent.transform);
            clonedButton.transform.localScale = new Vector3(1, 1, 1);

            ConfigureSelectables(clonedButton);

            var localize = clonedButton.GetComponentInChildren<Localize>(true);
            localize.Term = ModConfig.CONFIG_ENTRY_LOCALIZATION_KEY;

            var bindingProvider = clonedButton.GetComponent<ActionBindingProvider>();
            var actionData = bindingProvider.ActionData = ScriptableObject.CreateInstance<PlayerInputActionData>();
            actionData.actionSetType = ModConfig.MOD_INPUT_ACTION_SET_TYPE;
            _playerInputActionDataMyNameField.SetValue(actionData, $"{ModConfig.MOD_ACTIONS_PREFIX}{ModConfig.TALISMAN_SWITCH_ACTION_NAME}");

            return new ControlsButtonDescriptor(clonedButton);
        }

        private void ConfigureSelectables(GameObject buttonGameObject) {
            var playerControlBindingPanel = buttonGameObject.GetComponentInParent<PlayerControlBindingPanel>(true);
            var selectableNavigationProvider = buttonGameObject.GetComponentInParent<SelectableNavigationProvider>(true);
            
            AutoAttributeManager.AutoReferenceAllChildren(playerControlBindingPanel.gameObject);
            selectableNavigationProvider.AutoNavigateBindForAll();
        }
    }
}
