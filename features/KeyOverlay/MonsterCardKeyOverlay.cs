using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SlotKeybindsPlugin.features.KeyOverlay;

public class MonsterCardKeyOverlay : KeyOverlay
{
  public override bool IsEnabled()
  {
    return base.IsEnabled() &&
           Plugin.Instance.UpgradeCardSelect.Value &&
           Plugin.Instance.MonsterCardKeyOverlay.Value;
  }

  public void Refresh()
  {
    var titles = Game.GetMonsterCardTitles();

    for (var slot = 0; slot < titles.Length; slot++)
    {
      CheckSlot(titles[slot], Plugin.Instance.GetKeyForSlot(slot), slot);
    }
  }

  public override void OnDestroy()
  {
    var titles = Game.GetMonsterCardTitles();

    foreach (var title in titles)
    {
      var maybeOverlay = title.transform.parent.Find("KeybindOverlay");
      if (maybeOverlay)
      {
        Object.Destroy(maybeOverlay);
      }
    }
  }

  private void CheckSlot(Text title, KeyCode code, int slot)
  {
    var parent = title.transform.parent;
    var maybeOverlay = parent.Find("KeybindOverlay");
    var shouldExist = IsEnabled() && code != KeyCode.None;

    if (maybeOverlay)
    {
      if (shouldExist)
      {
        // make sure text is updated, in case config was changed
        var keybindText = maybeOverlay.GetComponent<Text>();
        keybindText.text = $"[ {KeyCodeToString(code)} ]";
        return;
      }

      // config was changed, and now this is disabled; so it shouldn't exist
      Object.Destroy(maybeOverlay);
      return;
    }

    if (!shouldExist) return;

    ConfigureTextObject(
      Object.Instantiate(title.transform, parent),
      KeyCodeToString(code),
      fontSize: 240
    );

    Logger.LogDebug($"Monster card key overlay created for slot {slot}");
  }
}