using HarmonyLib;

namespace SlotKeybindsPlugin.patches;

[HarmonyPatch(typeof(CardManager), "DisplayMonsterCards")]
public class MonsterCardPatch
{
  static void Postfix(CardManager __instance, int count)
  {
    Plugin.Instance.MonsterCardKeyOverlayFeature.Refresh();
  }
}