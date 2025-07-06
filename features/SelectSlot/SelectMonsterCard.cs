namespace SlotKeybindsPlugin.features.SelectSlot;

public class SelectMonsterCard : SelectSlot, InternalFeature
{
  public override bool IsEnabled()
  {
    return base.IsEnabled() && Plugin.Instance.MonsterCardSelect.Value;
  }

  public bool Select(int slot)
  {
    if(!IsEnabled()) return false;
    
    Game.GetCardManager()?.ActivateMonsterCard(slot);
    Logger.LogDebug($"Selected monster card slot {slot}");
    
    return true;
  }
}