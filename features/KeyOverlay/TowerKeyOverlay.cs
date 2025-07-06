using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SlotKeybindsPlugin.features.KeyOverlay;

public class TowerKeyOverlay : KeyOverlay
{
  public override bool IsEnabled()
  {
    return base.IsEnabled() && Plugin.Instance.TowerKeyOverlay.Value && Plugin.Instance.TowerSelect.Value;
  }

  public void Refresh()
  {
    var towers = Game.GetUnlockedTowers();
    if (towers == null) return;

    for (var slot = 0; slot < towers.Count; slot++)
    {
      CheckSlot(towers[slot], Plugin.Instance.GetKeyForSlot(slot), slot);
    }
  }

  public override void OnDestroy()
  {
    var towers = Game.GetUnlockedTowers();
    if (towers == null) return;

    foreach (var tower in towers)
    {
      var maybeOverlay = tower.transform.FindChildByName("KeyOverlay", true);
      if (maybeOverlay)
      {
        Object.Destroy(maybeOverlay);
      }
    }
  }

  private void CheckSlot(GameObject tower, KeyCode code, int slot)
  {
    var buildButtonUI = tower.GetComponentInChildren<BuildButtonUI>();
    if (!buildButtonUI)
    {
      Logger.LogWarning($"Failed to find BuildButtonUI (slot: {slot}).");
      return;
    }

    var parent = buildButtonUI.transform.parent;
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

    var priceTag = AccessTools.Field(typeof(BuildButtonUI), "priceTag")?.GetValue(buildButtonUI) as Text;
    if (!priceTag)
    {
      Plugin.Logger.LogWarning($"Failed to find price tag (slot: {slot}).");
      return;
    }

    ConfigureTextObject(
      Object.Instantiate(priceTag.transform, parent),
      KeyCodeToString(code),
      Color.black
    );

    Logger.LogDebug($"Tower key overlay created for slot {slot}");
  }
}