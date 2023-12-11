using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;

namespace FileMover;


public static class PluginInformation
{
    public const string PluginName = "FileMover";
    public const string PluginVersion = "1.0.0";
    public const string PluginGuid = "diyagi.morecompanycosmetics.filemover";
}

[BepInPlugin(PluginInformation.PluginGuid, PluginInformation.PluginName, PluginInformation.PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private static ManualLogSource _staticLogger;
    private static readonly string DynamicCosmeticsPath = Paths.PluginPath + "/MoreCompanyCosmetics";
    private const string CosmeticsDirectory = "Bundles";

    private void Awake()
    {
        _staticLogger = Logger;
        _staticLogger.LogInfo($"Running FileMover...");
        
        _staticLogger.LogInfo("Checking: "+DynamicCosmeticsPath);
            
        if (!Directory.Exists(DynamicCosmeticsPath))
        {
            _staticLogger.LogInfo("Creating cosmetics directory");
            Directory.CreateDirectory(DynamicCosmeticsPath);
        }

        List<string> cosmeticEmbeddedNames = GetEmbeddedCosmeticsNames();
        cosmeticEmbeddedNames.ForEach(x => _staticLogger.LogInfo(x));
        cosmeticEmbeddedNames.ForEach(x => MoveEmbeddedCosmeticsToFolder(x, DynamicCosmeticsPath));
    }

    private static List<string> GetEmbeddedCosmeticsNames()
    {
        string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        List<string> cosmeticEmbeddedNames = resourceNames.Where(x => x.Contains(".cosmetic")).ToList();

        return cosmeticEmbeddedNames;
    }

    private void MoveEmbeddedCosmeticsToFolder(string cosmeticEmbeddedName, string folderPath)
    {
        string cosmeticName = FormatCosmeticFileNameFromEmbed(cosmeticEmbeddedName);
        string filePath = $"{folderPath}\\{cosmeticName}";
        
        using var stream = Assembly
            .GetExecutingAssembly()  
            .GetManifestResourceStream(cosmeticEmbeddedName)!;
        using Stream s = File.Create(filePath);
        stream.CopyTo(s);
    }

    private string FormatCosmeticFileNameFromEmbed(string cosmeticEmbeddedName)
    {
        string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        return cosmeticEmbeddedName.Replace($"{assemblyName}.{CosmeticsDirectory}.", "");
    }
}
