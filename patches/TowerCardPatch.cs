using HarmonyLib;

namespace SlotKeybindsPlugin.patches;

[HarmonyPatch(typeof(TowerUnlockManager), "DisplayButtons")]
public class TowerCardPatch
{
  static void Postfix(TowerUnlockManager __instance)
  {
    Plugin.Instance.TowerKeyOverlayFeature.Refresh();
    Plugin.Instance.BuildingKeyOverlayFeature.Refresh();
  }
}