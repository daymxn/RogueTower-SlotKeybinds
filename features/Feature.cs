using BepInEx.Logging;

namespace SlotKeybindsPlugin.features;

public abstract class Feature
{
  protected static ManualLogSource Logger = Plugin.Logger;

  public virtual bool IsEnabled()
  {
    return Plugin.Instance.PluginEnabled.Value;
  }

  public virtual void OnUpdate()
  {
  }

  public virtual void OnStart()
  {
  }

  public virtual void OnDestroy()
  {
  }
}

public interface InternalFeature;