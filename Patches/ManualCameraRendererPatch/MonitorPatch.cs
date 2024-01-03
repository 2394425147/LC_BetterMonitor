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
            var radarTarget = __instance.radarTargets[__instance.targetTransformIndex];

            if (__instance.cam        != __instance.mapCamera ||
                radarTarget.transform == null)
                return;

            AlignCameraToPlayer(__instance.cam.transform, radarTarget.transform);
            UpdateCompass(__instance, radarTarget.transform);
        }

        private static void UpdateCompass(ManualCameraRenderer __instance, Transform radarTarget)
        {
            if (!__instance.shipArrowUI.activeSelf)
                return;

            var playerDistance = StartOfRound.Instance.elevatorTransform.position - radarTarget.position;
            playerDistance.y = 0;

            if (Plugin.Configuration.AlignMonitorToPlayer.Value)
            {
                var rotation = radarTarget.rotation;
                playerDistance = Quaternion.AngleAxis(-rotation.y, Vector3.up) * playerDistance;
            }

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

        private static void AlignCameraToPlayer(Transform camera, Transform player)
        {
            if (!Plugin.Configuration.AlignMonitorToPlayer.Value)
                return;

            camera.rotation = Quaternion.Euler(90, player.rotation.eulerAngles.y, 0);
        }
    }
}
