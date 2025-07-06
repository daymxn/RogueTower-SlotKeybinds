namespace SlotKeybindsPlugin.features.Banish;

public class BanishUpgradeCard : Feature
{
  public override bool IsEnabled()
  {
    return base.IsEnabled() &&
           Plugin.Instance.UpgradeCardSelect.Value &&
           Plugin.Instance.BanishEnabled.Value &&
           Plugin.Instance.BanishPluginPresent;
  }

  public bool Banish(int slot)
  {
    if (!IsEnabled()) return false;

    BanishInterop.BanishCard(slot);
    Plugin.Instance.BanishColorsFeature.SetBanishColors();

    return true;
  }
}