#nullable enable
using UnityEngine.UI;
using static SlotKeybindsPlugin.Plugin;

namespace SlotKeybindsPlugin.features.Banish;

/**
 * Static class for interoping with the plugin `AgusBut.BanishCards`
 */
public static class BanishInterop
{
  public static void BanishCard(int slot)
  {
    var button = GetBanishButton(slot);
    if (!button)
    {
      Logger.LogWarning($"No banish button found for slot {slot}!");
      return;
    }

    button?.onClick.Invoke();
    Logger.LogWarning($"Banished card slot {slot}");
  }

  private static Button? GetBanishButton(int slot) => Game.GetCardHolder(slot)?
    .transform
    .FindChildByName("BanishButton", true)?
    .GetComponent<Button>();
}