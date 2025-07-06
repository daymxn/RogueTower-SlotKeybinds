namespace SlotKeybindsPlugin.features.SelectSlot;

public class SelectBuilding : SelectSlot, InternalFeature
{
  public override bool IsEnabled()
  {
    return base.IsEnabled() && Plugin.Instance.BuildingSelect.Value;
  }

  public bool Select(int slot)
  {
    if(!IsEnabled()) return false;
    
    var button = Game.GetBuildingButton(slot);
    if (!button)
    {
      Logger.LogWarning($"Building button not found for slot {slot}");
      return true;
    }

    button.Build();
    Logger.LogDebug($"Pressed building slot {slot}");
    
    return true;
  }
}