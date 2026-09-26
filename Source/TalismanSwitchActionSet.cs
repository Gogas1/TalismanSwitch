using InControl;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalismanSwitch {
    internal class TalismanSwitchActionSet : PlayerActionSet {
        internal Action? OnSwitchTalismanPressed;
        internal PlayerAction SwitchTalismanAction = null!;

        private new PlayerAction CreatePlayerAction(string actionName) {
            return base.CreatePlayerAction(ModConfig.MOD_ACTIONS_PREFIX + actionName);
        }

        internal void Initialize() {
            SwitchTalismanAction = CreatePlayerAction(ModConfig.TALISMAN_SWITCH_ACTION_NAME);

            BindControls();
        }

        internal void Update() {
            if (SwitchTalismanAction.WasPressed) {
                OnSwitchTalismanPressed?.Invoke();
            }
        }

        protected void BindControls() {
            SwitchTalismanAction.AddDefaultBinding(ModConfig.DEFAULT_CONTROLLER_INPUT);
            SwitchTalismanAction.AddDefaultBinding(ModConfig.DEFAULT_KBM_INPUT);
        }
    }
}
