using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using MoreCompany.Cosmetics;
using MoreCompany.Utils;
using UnityEngine;

namespace PandaCosmetics;

public static class PluginInformation
{
    public const string PluginName = "Panda Cosmetics";
    public const string PluginVersion = "2.0.2";
    public const string PluginGuid = "diyagi.PandaCosmetics";
}

[BepInPlugin(PluginInformation.PluginGuid, PluginInformation.PluginName, PluginInformation.PluginVersion)]
[BepInDependency("me.swipez.melonloader.morecompany")]
public class Plugin : BaseUnityPlugin
{
    private static Assembly _executingAssembly;
    private static ManualLogSource _staticLogger;
    
    private void Awake()
    {
        // Plugin startup logic
        _staticLogger = Logger;
        _staticLogger.LogInfo($"Loading {PluginInformation.PluginName}...");

        _executingAssembly = Assembly.GetExecutingAssembly();

        List<string> bundles = GetEmbeddedBundlesNames();
        bundles.ForEach(LoadBundle);
        
        _staticLogger.LogInfo($"{PluginInformation.PluginName} Loaded!!!");
    }

    private static List<string> GetEmbeddedBundlesNames()
    {
        string[] resourceNames = _executingAssembly.GetManifestResourceNames();
        return resourceNames.Where(x => x.Contains(".cosmetic")).ToList();
    }
    
    private static void LoadBundle(string bundleResourceName)
    {
        _staticLogger.LogInfo($"Found AssetBundle {bundleResourceName}...");
        AssetBundle bundle = BundleUtilities.LoadBundleFromInternalAssembly(bundleResourceName, _executingAssembly);
        LoadCosmeticsFromBundle(bundle);
    }

    private static void LoadCosmeticsFromBundle(AssetBundle bundle)
    {
        foreach (string potentialPrefab in bundle.GetAllAssetNames())
        {
            if (!potentialPrefab.EndsWith(".prefab")) continue;
                
            GameObject cosmeticInstance = bundle.LoadPersistentAsset<GameObject>(potentialPrefab);
            CosmeticInstance cosmeticInstanceBehavior = cosmeticInstance.GetComponent<CosmeticInstance>();
            if (cosmeticInstanceBehavior == null) continue;
            
            _staticLogger.LogInfo($"Loaded cosmetic: {cosmeticInstanceBehavior.cosmeticId} from bundle");
            CosmeticRegistry.cosmeticInstances.Add(cosmeticInstanceBehavior.cosmeticId, cosmeticInstanceBehavior);
        }
    }
}
