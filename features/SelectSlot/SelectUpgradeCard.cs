namespace SlotKeybindsPlugin.features.SelectSlot;

public class SelectUpgradeCard : SelectSlot, InternalFeature
{
  public override bool IsEnabled()
  {
    return base.IsEnabled() && Plugin.Instance.UpgradeCardSelect.Value;
  }

  public bool Select(int slot)
  {
    if (!IsEnabled()) return false;

    Game.GetCardManager()?.ActivateCard(slot);
    Logger.LogDebug($"Selected card slot {slot}");

    return true;
  }
}