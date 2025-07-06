using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SlotKeybindsPlugin.features.KeyOverlay;

public class BuildingKeyOverlay : KeyOverlay
{
  public override bool IsEnabled()
  {
    return base.IsEnabled() &&
           Plugin.Instance.TowerKeyOverlay.Value &&
           Plugin.Instance.BuildingSelect.Value &&
           Plugin.Instance.BuildingKey.Value != KeyCode.None;
  }

  public void Refresh()
  {
    var buildings = Game.GetUnlockedBuildings();
    if (buildings == null) return;

    for (var slot = 0; slot < buildings.Count; slot++)
    {
      CheckSlot(buildings[slot], Plugin.Instance.GetKeyForSlot(slot), slot);
    }
  }

  public override void OnDestroy()
  {
    var buildings = Game.GetUnlockedBuildings();
    if (buildings == null) return;

    foreach (var building in buildings)
    {
      var maybeOverlay = building.transform.FindChildByName("KeyOverlay", true);
      if (maybeOverlay)
      {
        Object.Destroy(maybeOverlay);
      }
    }
  }

  private void CheckSlot(GameObject building, KeyCode code, int slot)
  {
    var buildButtonUI = building.GetComponentInChildren<BuildButtonUI>();
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
        keybindText.text = $"[ {KeyCodeToString(Plugin.Instance.BuildingKey.Value)} + {KeyCodeToString(code)} ]";
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
      $"{KeyCodeToString(Plugin.Instance.BuildingKey.Value)} + {KeyCodeToString(code)}",
      Color.black
    );

    Logger.LogDebug($"Building key overlay created for slot {slot}");
  }
}