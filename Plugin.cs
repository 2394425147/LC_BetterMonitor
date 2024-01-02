using BepInEx;
using BetterMonitor.Patches.ScrapShapePatch;
using HarmonyLib;

namespace BetterMonitor
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance      { get; private set; }
        public static Config Configuration { get; private set; }

        private void Awake()
        {
            Instance      = this;
            Configuration = new Config(Config);

            var harmony = new Harmony("com.fumiko.bettermonitor");

            harmony.PatchAll(typeof(ScrapObjectPatch));
            ScrapObjectPatch.Initialize();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        public static void LogError(string s) => Instance.Logger.LogError(s);

        public static void LogInfo(string s) => Instance.Logger.LogInfo(s);
    }
}
