using InControl;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalismanSwitch {
    internal static class ModConfig {

        internal static IReadOnlyList<string> AssetBundlesPaths => _assetBundlesPaths;
        private static List<string> _assetBundlesPaths = [
            "TalismanSwitch.Resources.main_bundle.bundle",
            "TalismanSwitch.Resources.modbundle.bundle"];

        internal const string MAIN_ABILITY_COLLECTION_ASSET_NAME = "extraobjects/playermainabilitycollection/-0_playermainabilitycollection.prefab";
        internal const string TALISMAN_DISPLAY_CANVAS_ASSET_NAME = "assets/canvas.prefab";
        internal const string TALISMAN_SWITCH_COLLECTION_ASSET_NAME = "extraobjects/playerabilitysinglechoicecollection/foostyle collection 所有流派.prefab";

        internal const InputControlType DEFAULT_CONTROLLER_INPUT = InputControlType.DPadDown;
        internal const Key DEFAULT_KBM_INPUT = Key.Key2;

        internal const string KBM_CONTROLS_PANEL_NAME = "Keyboard Mouse Binding Panel";
        internal const string CONTROLLER_CONTROLS_PANEL_NAME = "Controller Panel";
        internal const ActionSetType MOD_INPUT_ACTION_SET_TYPE = (ActionSetType)4;

        internal const string MOD_ACTIONS_PREFIX = "TalismanSwitch";
        internal const string TALISMAN_SWITCH_ACTION_NAME = "TalismanSwitch";
        internal const string PLAYER_PREFS_KEY = "TALISMAN_SWITCH_PLAYER_PREFS";

        internal const string CONFIG_ENTRY_LOCALIZATION_KEY = "NextTalismanKey";
        internal const string TRANSLATIONS_ASSET_PATH = "TalismanSwitch.Resources.translations.json";
    }
}
