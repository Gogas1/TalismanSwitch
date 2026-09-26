using System;
using System.Collections.Generic;
using System.Text;
using TalismanSwitch.HarmonyPatches;

namespace TalismanSwitch.Handlers {
    internal class TalismanSwitchHandler {
        private bool _styleChangeQueued = false;
        private PlayerAbilitySingleChoiceCollection? talismanSwitchCollection;

        public TalismanSwitchHandler() {
            talismanSwitchCollection = (PlayerAbilitySingleChoiceCollection)AssetLoader.Assets[ModConfig.TALISMAN_SWITCH_COLLECTION_ASSET_NAME];

            PlayerPatches.OnPlayerAwakePostfix += HandlePlayerAwake;
        }

        private void HandlePlayerAwake(Player player) {
            player.fsm.Changed += HandlePlayerFsmChange;
        }

        private void HandlePlayerFsmChange(PlayerStateType state) {
            if (_styleChangeQueued && !IsUsingTalisman(state)) {
                _styleChangeQueued = false;
                ChangeTalisman();
            }
        }

        internal void ChangeTalisman() {
            var playerState = Player.i.CurrentStateType;
            if (IsUsingTalisman(playerState)) {
                _styleChangeQueued = true;
                return;
            }

            talismanSwitchCollection?.Next();
        }

        private static bool IsUsingTalisman(PlayerStateType playerState) {
            return playerState == PlayerStateType.FooAttack ||
                            playerState == PlayerStateType.FooAttackCharge ||
                            playerState == PlayerStateType.FooExplode;
        }

        internal void Release() {
            PlayerPatches.OnPlayerAwakePostfix -= HandlePlayerAwake;
        }
    }
}
