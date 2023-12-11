using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using MoreCompany.Cosmetics;
using MoreCompany.Utils;
using UnityEngine;

namespace PandaCosmetics;

public static class PluginInformation
{
    public const string PluginName = "Panda Cosmetics";
    public const string PluginVersion = "2.0.1";
    public const string PluginGuid = "diyagi.PandaCosmetics";
}

[BepInPlugin(PluginInformation.PluginGuid, PluginInformation.PluginName, PluginInformation.PluginVersion)]
[BepInDependency("me.swipez.melonloader.morecompany")]
public class Plugin : BaseUnityPlugin
{
    private Assembly _executingAssembly;
    
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Loading {PluginInformation.PluginName}...");

        _executingAssembly = Assembly.GetExecutingAssembly();

        List<string> bundles = GetEmbeddedBundlesNames();
        bundles.ForEach(LoadBundle);
        
        Logger.LogInfo($"{PluginInformation.PluginName} Loaded!!!");
    }

    private void LoadBundle(string bundleResourceName)
    {
        Logger.LogInfo($"Loading AssetBundle {bundleResourceName}...");
        AssetBundle bundle = BundleUtilities.LoadBundleFromInternalAssembly(bundleResourceName, _executingAssembly);
        CosmeticRegistry.LoadCosmeticsFromBundle(bundle);
    }

    private List<string> GetEmbeddedBundlesNames()
    {
        string[] resourceNames = _executingAssembly.GetManifestResourceNames();
        return resourceNames.Where(x => x.Contains(".cosmetic")).ToList();
    }
}
