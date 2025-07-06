namespace SlotKeybindsPlugin.features.SelectSlot;

public class SelectTower : SelectSlot, InternalFeature
{
  public override bool IsEnabled()
  {
    return base.IsEnabled() && Plugin.Instance.TowerSelect.Value;
  }

  public bool Select(int slot)
  {
    if (!IsEnabled()) return false;

    var button = Game.GetTowerButton(slot);
    if (!button)
    {
      Logger.LogWarning($"Tower button not found for slot {slot}");
      return true;
    }

    button.Build();
    Logger.LogDebug($"Pressed tower slot {slot}");

    return true;
  }
}