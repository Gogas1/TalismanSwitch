using NineSolsAPI.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TalismanSwitch {
    internal static class AssetLoader {
        internal static Dictionary<string, UnityEngine.Object> Assets { get; private set; } = new();

        internal static void LoadAssets() {
            LoadAssetsFromAssetBundles();
        }

        private static void LoadAssetsFromAssetBundles() {
            foreach (var assetBundlePath in ModConfig.AssetBundlesPaths) {
                var assetBundle = AssemblyUtils.GetEmbeddedAssetBundle(assetBundlePath);

                if (assetBundle == null) continue;

                foreach (var assetName in assetBundle.GetAllAssetNames()) {
                    var asset = assetBundle.LoadAsset(assetName);
                    if(asset == null) continue;

                    Assets[assetName] = asset;
                }
            }
        }
    }
}
