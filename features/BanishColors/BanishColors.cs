using UnityEngine;

namespace SlotKeybindsPlugin.features.BanishColors;

public class BanishColors : Feature
{
  private static readonly Color DefaultCardColor = new(1, 1, 1, 1);

  public override bool IsEnabled()
  {
    return base.IsEnabled() && Plugin.Instance.BanishEnabled.Value && Plugin.Instance.BanishColorsEnabled.Value;
  }

  public override void OnUpdate()
  {
    var key = Plugin.Instance.BanishKey.Value;
    if (key == KeyCode.None) return;

    if (Input.GetKeyDown(key))
    {
      TriggerBanishColors(1);
    }

    if (Input.GetKeyUp(key))
    {
      RestoreCardColors(1);
    }
  }

  public void SetBanishColors()
  {
    if(!IsEnabled()) return;
    
    var key = Plugin.Instance.BanishKey.Value;
    if (key == KeyCode.None) return;
    if (!Input.GetKey(key)) return;
    
    TriggerBanishColors(0);
  }

  public override void OnDestroy()
  {
    RestoreCardColors(0);
  }

  private void TriggerBanishColors(int duration)
  {
    if(!IsEnabled()) return;
    var images = Game.GetCardImages();

    foreach (var image in images)
    {
      image.CrossFadeColor(Plugin.Instance.BanishColor.Value, duration, false, true);
    }
  }

  private void RestoreCardColors(int duration)
  {
    var images = Game.GetCardImages();

    foreach (var image in images)
    {
      image.CrossFadeColor(DefaultCardColor, duration, false, true);
    }
  }
}