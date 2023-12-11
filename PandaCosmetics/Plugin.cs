using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using MoreCompany.Cosmetics;
using MoreCompany.Utils;

namespace PandaCosmetics;

public static class PluginInformation
{
    public const string PluginName = "Panda Cosmetics";
    public const string PluginVersion = "1.0.0";
    public const string PluginGuid = "diyagi.PandaCosmetics";
}

[BepInPlugin(PluginInformation.PluginGuid, PluginInformation.PluginName, PluginInformation.PluginVersion)]
[BepInDependency("me.swipez.melonloader.morecompany")]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Loading {PluginInformation.PluginName}...");

        string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        List<string> cosmeticEmbeddedNames = resourceNames.Where(x => x.Contains(".cosmetic")).ToList();
        
        cosmeticEmbeddedNames.ForEach(x =>
        {
            Logger.LogInfo($"Loading cosmetic {x}...");
            var bundle = BundleUtilities.LoadBundleFromInternalAssembly(x, Assembly.GetExecutingAssembly());
            CosmeticRegistry.LoadCosmeticsFromBundle(bundle);
        });
        
        Logger.LogInfo($"{PluginInformation.PluginName} Loaded!!!");
    }
}