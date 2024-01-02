using BepInEx.Configuration;
using BetterMonitor.Models;

namespace BetterMonitor
{
    /// <summary>
    /// Typed wrapper around the <see cref="BepInEx.Configuration.ConfigFile"/> class
    /// </summary>
    public class Config
    {
        public ConfigEntry<bool>                DisplayScrapByValue { get; private set; }
        public ConfigEntry<CompassLocationType> CompassLocation     { get; private set; }

        public Config(ConfigFile config)
        {
            DisplayScrapByValue = config.Bind("Scraps",
                                              "Show value",
                                              false,
                                              "Scrap icon changes shape by their value (Customize the icons in the 'ScrapIcons' folder)");

            CompassLocation = config.Bind("Compass",
                                          "Location",
                                          CompassLocationType.AroundPlayerWithProximity,
                                          "Where the compass should be placed on the monitor");
        }
    }
}
