using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using I2.Loc;
using InControl;
using NineSolsAPI;
using NineSolsAPI.Utils;
using TalismanSwitch.Handlers;
using TalismanSwitch.HarmonyPatches;
using TalismanSwitchComponents;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TalismanSwitch;

[BepInDependency(NineSolsAPICore.PluginGUID)]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class TalismanSwitch : BaseUnityPlugin {

    private Harmony harmony = null!;

    internal static TalismanSwitch Instance = null!;

    internal TalismanDisplayController talismanDisplayController = null!;
    internal TalismanStateObserver talismanStateObserver = null!;
    internal TalismanSwitchHandler talismanSwitchHandler = null!;
    internal ModInputHandler modInputHandler = null!;
    internal SettingsPatchingHandler settingsPatchingHandler = null!;

    internal ConfigEntry<KeyboardShortcut> switchControl = null!;


    private void Awake() {
        Instance = this;
        Log.Init(Logger);
        RCGLifeCycle.DontDestroyForever(gameObject);

        try {
            LoadAssets();

            talismanStateObserver = new TalismanStateObserver(talismanDisplayController, (PlayerMainAbilityCollection)AssetLoader.Assets[ModConfig.MAIN_ABILITY_COLLECTION_ASSET_NAME]);
            InitInputHandler();

            talismanSwitchHandler = new();
            modInputHandler.OnSwitchTalismanPressed += talismanSwitchHandler.ChangeTalisman;
            settingsPatchingHandler = new();

            AddLocalizations();
        } catch (Exception ex) {
            Log.Exception(ex);
            Destroy(this);
            return;
        }

        harmony = Harmony.CreateAndPatchAll(typeof(TalismanSwitch).Assembly);

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void InitInputHandler() {
        modInputHandler = new GameObject("ModInputHandler").AddComponent<ModInputHandler>();
        RCGLifeCycle.DontDestroyForever(modInputHandler.gameObject);
        modInputHandler.gameObject.hideFlags = HideFlags.HideAndDontSave;
    }

    private void AddLocalizations() {
        var targetSource = LocalizationManager.Sources.FirstOrDefault(s => s.Google_SpreadsheetName == ModConfig.TARGET_SOURCE_SPREADSHEET_NAME);

        var translations = AssemblyUtils.GetEmbeddedJson<Dictionary<string, string>>(ModConfig.TRANSLATIONS_ASSET_PATH);

        if(translations == null) {
            Log.Error("Failed to load translations");
            return;
        }

        var termData = targetSource.AddTerm(ModConfig.CONFIG_ENTRY_LOCALIZATION_KEY);
        List<string> termTranslationsList = new();
        for (int i = 0; i < targetSource.mLanguages.Count; i++) {
            var language = targetSource.mLanguages[i];

            if(!translations.TryGetValue(language.Code, out var value)) {
                termTranslationsList.Add("");
            }
            else {
                termTranslationsList.Add(value);
            }
        }

        termData.Languages = termTranslationsList.ToArray();
    }

    private void LoadAssets() {
        AssetLoader.LoadAssets();

        var canvas = (GameObject)AssetLoader.Assets[ModConfig.TALISMAN_DISPLAY_CANVAS_ASSET_NAME];
        var talismanDisplayCanvasInstance = Instantiate(canvas);
        RCGLifeCycle.DontDestroyForever(talismanDisplayCanvasInstance);
        talismanDisplayCanvasInstance.hideFlags = HideFlags.HideAndDontSave;
        talismanDisplayController = talismanDisplayCanvasInstance.GetComponentInChildren<TalismanDisplayController>();
    }

    private void OnDestroy() {
        modInputHandler.OnSwitchTalismanPressed -= talismanSwitchHandler.ChangeTalisman;

        talismanStateObserver.Release();
        talismanSwitchHandler.Release();

        Destroy(modInputHandler.gameObject);
        Destroy(talismanDisplayController.GetComponentInParent<Canvas>().gameObject);

        harmony.UnpatchSelf();
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} has been unloaded");
    }    
}