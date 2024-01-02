using BetterMonitor.Models;
using HarmonyLib;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace BetterMonitor.Patches.ManualCameraRendererPatch
{
    [HarmonyPatch(typeof(ManualCameraRenderer))]
    public static class MonitorPatch
    {
        // Using postfix instead of overriding the entire method to avoid future update compatibility issues
        [HarmonyPostfix]
        [HarmonyPatch(nameof(ManualCameraRenderer.Update))]
        public static void UpdatePostfix(ManualCameraRenderer __instance)
        {
            if (!__instance.shipArrowUI.activeSelf)
                return;

            var radarTarget = __instance.radarTargets[__instance.targetTransformIndex];

            if (__instance.cam        != __instance.mapCamera ||
                radarTarget.transform == null)
                return;

            var playerDistance = StartOfRound.Instance.elevatorTransform.position - radarTarget.transform.position;
            playerDistance.y = 0;

            if (Plugin.Configuration.CompassLocation.Value == CompassLocationType.AroundPlayer)
            {
                __instance.shipArrowPointer.localPosition = playerDistance.normalized * 50;
            }
            else
            {
                var pointerDistance = Mathf.Lerp(40, 90, playerDistance.sqrMagnitude / 196);
                __instance.shipArrowPointer.localPosition = playerDistance.normalized * pointerDistance;
            }

            __instance.shipArrowPointer.forward = playerDistance;
        }
    }
}
