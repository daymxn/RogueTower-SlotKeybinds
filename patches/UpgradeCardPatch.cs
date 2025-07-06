using HarmonyLib;

namespace SlotKeybindsPlugin.patches;

[HarmonyPatch(typeof(CardManager), "DisplayCards")]
public class UpgradeCardPatch
{
  static void Postfix(CardManager __instance, int count)
  {
    Plugin.Instance.UpgradeCardKeyOverlayFeature.Refresh();
  }
}