using System.Collections.Generic;
using System.IO;
using BepInEx;
using BetterMonitor.Resources;
using BetterMonitor.Utilities;
using HarmonyLib;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace BetterMonitor.Patches.ScrapShapePatch
{
    [HarmonyPatch(typeof(GrabbableObject))]
    public static class ScrapObjectPatch
    {
        private static readonly int                      MainTexture    = Shader.PropertyToID("_UnlitColorMap");
        private static readonly SortedList<int, Texture> ScrapIcons     = new();
        private static readonly string                   ScrapIconsPath = Path.Combine(Paths.PluginPath, "ScrapIcons");

        public static void Initialize()
        {
            if (!Directory.Exists(ScrapIconsPath))
                InitializeDirectory();

            LoadScrapIcons();
        }

        private static void LoadScrapIcons()
        {
            Plugin.LogInfo($"Loading scrap icons from {ScrapIconsPath}");
            foreach (var file in Directory.EnumerateFiles(ScrapIconsPath))
            {
                var name      = Path.GetFileNameWithoutExtension(file);
                var extension = Path.GetExtension(file);

                if (extension.ToLowerInvariant() != ".png")
                    continue;

                if (!int.TryParse(name, out var scrapValue))
                    continue;

                AddScrapIcon(scrapValue, File.ReadAllBytes(file).ToTexture2D());
            }

            if (ScrapIcons.Count > 0)
                return;

            using var fallbackIconStream = ModResources.Get("ScrapIcons.0.png");
            var bytes                     = new byte[fallbackIconStream.Length];

            if (fallbackIconStream.Read(bytes, 0, bytes.Length) != 0)
                return;

            AddScrapIcon(0, bytes.ToTexture2D());
        }

        private static void InitializeDirectory()
        {
            Directory.CreateDirectory(ScrapIconsPath);

            var defaultScrapIcons = new[]
            {
                "0.png",
                "59.png",
                "89.png"
            };

            foreach (var scrapIcon in defaultScrapIcons)
            {
                var path = Path.Combine(ScrapIconsPath, scrapIcon);

                using var file = File.Create(path);
                ModResources.Get($"ScrapIcons.{scrapIcon}").CopyTo(file);

                Plugin.LogInfo($"Created default scrap icon at {path}");
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(GrabbableObject.Start))]
        public static void Postfix(GrabbableObject __instance)
        {
            if (!__instance.itemProperties.isScrap)
                return;

            if (!__instance.radarIcon.TryGetComponent(out MeshRenderer meshRenderer))
                return;

            var materialProperties = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(materialProperties);
            materialProperties.SetTexture(MainTexture, FindScrapIcon(__instance.scrapValue));
            meshRenderer.SetPropertyBlock(materialProperties);
        }

        private static Texture FindScrapIcon(int value)
        {
            for (var i = 0; i < ScrapIcons.Count; i++)
            {
                var scrapValue = ScrapIcons.Keys[i];

                if (scrapValue >= value)
                    return ScrapIcons.Values[Mathf.Max(0, i - 1)];
            }

            return ScrapIcons.Values[^1];
        }

        private static void AddScrapIcon(int minValue, Texture texture)
        {
            ScrapIcons.Add(minValue, texture);
        }
    }
}
