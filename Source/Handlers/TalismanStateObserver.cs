using System;
using System.Collections.Generic;
using System.Text;
using TalismanSwitch.HarmonyPatches;
using TalismanSwitchComponents;
using UnityEngine;

namespace TalismanSwitch {
    internal class TalismanStateObserver {
        public AbilityWrapper FooExplodeAllStyle => mainAbilityCollection.FooExplodeAllStyle;
        public AbilityWrapper FooExplodeConsecutiveStyle => mainAbilityCollection.FooExplodeConsecutiveStyle;
        public AbilityWrapper FooExplodeAutoStyle => mainAbilityCollection.FooExplodeAutoStyle;
        public AbilityWrapper FooExplodeAllStyleUpgrade => mainAbilityCollection.FooExplodeAllStyleUpgrade;
        public AbilityWrapper FooExplodeConsecutiveStyleUpgrade => mainAbilityCollection.FooExplodeConsecutiveStyleUpgrade;
        public AbilityWrapper FooExplodeAutoStyleUpgrade => mainAbilityCollection.FooExplodeAutoStyleUpgrade;

        private TalismanDisplayController _talismanDisplayController;
        private readonly PlayerMainAbilityCollection mainAbilityCollection;
        private bool _isHidden;
        public bool IsHidden {
            get => _isHidden;
            set {
                _isHidden = value;
                UpdateUIState();
            }
        }

        internal TalismanStateObserver(TalismanDisplayController talismanDisplayController, PlayerMainAbilityCollection mainAbilityCollection) {
            if(mainAbilityCollection == null) {
                throw new ArgumentNullException(nameof(mainAbilityCollection));
            }

            _talismanDisplayController = talismanDisplayController;
            this.mainAbilityCollection = mainAbilityCollection;

            PlayerInputBinderPatches.OnAwakePostfix += HandleInputBinderAwake;

            FooExplodeAllStyle.AbilityData.OnActivate.AddListener(HandleTalismanChange);
            FooExplodeConsecutiveStyle.AbilityData.OnActivate.AddListener(HandleTalismanChange);
            FooExplodeAutoStyle.AbilityData.OnActivate.AddListener(HandleTalismanChange);
            FooExplodeAllStyleUpgrade.AbilityData.OnActivate.AddListener(HandleTalismanChange);
            FooExplodeConsecutiveStyleUpgrade.AbilityData.OnActivate.AddListener(HandleTalismanChange);
            FooExplodeAutoStyleUpgrade.AbilityData.OnActivate.AddListener(HandleTalismanChange);
        }

        private void HandleTalismanChange(bool _) {
            UpdateUIState();
        }

        private void HandleInputBinderAwake() {
            if (SingletonBehaviour<PlayerInputBinder>.IsAvailable()) {
                var inputBinder = SingletonBehaviour<PlayerInputBinder>.Instance;
                inputBinder.fsm.Changed += HandlePlayerInputStateChange;
            }
        }

        private void HandlePlayerInputStateChange(PlayerInputStateType inputState) {
            if (inputState != PlayerInputStateType.Action) {
                IsHidden = true;
            } else {
                IsHidden = false;
            }
            UpdateUIState();
        }

        internal void UpdateUIState() {
            var enabledTalisman = IsHidden ? EnabledTalisman.None : ResolveEnabledTalisman();
            _talismanDisplayController.EnabledTalisman = enabledTalisman;
            _talismanDisplayController.RequestUpdate();
        }

        private EnabledTalisman ResolveEnabledTalisman() {
            try {
                if (Player.i == null) {
                    return EnabledTalisman.None;
                }

                

                if (FooExplodeAllStyle.AbilityData.IsEquipped) {
                    if (FooExplodeAllStyleUpgrade.AbilityData.IsEquipped) {
                        return EnabledTalisman.QiBlastUpgraded;
                    }

                    return EnabledTalisman.QiBlast;
                }

                

                if (FooExplodeAutoStyle.AbilityData.IsEquipped) {
                    if (FooExplodeAutoStyleUpgrade.AbilityData.IsEquipped) {
                        return EnabledTalisman.WaterFlowUpgraded;
                    }

                    return EnabledTalisman.WaterFlow;
                }

                

                if (FooExplodeConsecutiveStyle.AbilityData.IsEquipped) {
                    if (FooExplodeConsecutiveStyleUpgrade.AbilityData.IsEquipped) {
                        return EnabledTalisman.FullControlUpgraded;
                    }

                    return EnabledTalisman.FullControl;
                }
            } catch(Exception ex) {
                Log.Error($"Error resolving enabled talisman: {ex.Message}");
                return EnabledTalisman.None;
            }
            

            return EnabledTalisman.None;
        }

        internal void Release() {
            if (SingletonBehaviour<PlayerInputBinder>.IsAvailable()) {
                var inputBinder = SingletonBehaviour<PlayerInputBinder>.Instance;
                inputBinder.fsm.Changed -= HandlePlayerInputStateChange;
            }

            PlayerInputBinderPatches.OnAwakePostfix -= HandleInputBinderAwake;
        }
    }
}
