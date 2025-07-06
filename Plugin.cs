using System;
using System.Linq;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SlotKeybindsPlugin.features.BanishColors;
using SlotKeybindsPlugin.features.KeyOverlay;
using SlotKeybindsPlugin.features.SelectSlot;
using UnityEngine;

namespace SlotKeybindsPlugin;

[BepInProcess("Rogue Tower.exe")]
[BepInProcess("RogueTower.exe")]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("AgusBut.BanishCards", BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
  public static Plugin Instance { get; private set; }
  internal new static ManualLogSource Logger;

  public ConfigEntry<bool> PluginEnabled;
  public ConfigEntry<bool> UpgradeCardSelect;
  public ConfigEntry<bool> MonsterCardSelect;
  public ConfigEntry<bool> TowerSelect;
  public ConfigEntry<bool> BuildingSelect;
  public ConfigEntry<bool> BanishEnabled;
  public ConfigEntry<bool> BanishColorsEnabled;
  public ConfigEntry<bool> TowerKeyOverlay;
  public ConfigEntry<bool> UpgradeCardKeyOverlay;
  public ConfigEntry<bool> MonsterCardKeyOverlay;
  public ConfigEntry<Color> BanishColor;
  public ConfigEntry<KeyCode> BanishKey;
  public ConfigEntry<KeyCode> BuildingKey;
  public readonly ConfigEntry<KeyCode>[] SelectKeys = new ConfigEntry<KeyCode>[11];

  public bool BanishPluginPresent;

  public TowerKeyOverlay TowerKeyOverlayFeature;
  public BuildingKeyOverlay BuildingKeyOverlayFeature;
  public UpgradeCardKeyOverlay UpgradeCardKeyOverlayFeature;
  public MonsterCardKeyOverlay MonsterCardKeyOverlayFeature;
  public BanishColors BanishColorsFeature;
  public SelectSlot SelectSlotFeature;

  public Harmony Patches;

  private static KeyCode DefaultKeyCodeForSlot(int slot) => slot switch
  {
    < 10 => (KeyCode)((int)KeyCode.Alpha1 + (slot - 1)),
    10 => KeyCode.Alpha0,
    _ => KeyCode.None
  };

  private void Awake()
  {
    Instance = this;
    Logger = base.Logger;
    Logger.LogInfo($"Loading plugin {MyPluginInfo.PLUGIN_GUID}");

    PluginEnabled = Config.Bind(
      section: "General",
      key: "Enabled",
      defaultValue: true,
      description: "Enable the plugin."
    );
    UpgradeCardSelect = Config.Bind(
      section: "General",
      key: "UpgradeCardSelect",
      defaultValue: true,
      description: "Use the slot keys for selecting upgrade cards."
    );
    MonsterCardSelect = Config.Bind(
      section: "General",
      key: "MonsterCardSelect",
      defaultValue: true,
      description: "Use the slot keys for monster buff cards drawn after special game modes."
    );
    TowerSelect = Config.Bind(
      section: "General",
      key: "TowerSelect",
      defaultValue: true,
      description: "Use the slot keys for selecting towers to build."
    );
    BuildingSelect = Config.Bind(
      section: "General",
      key: "BuildingSelect",
      defaultValue: true,
      description: "Toggle the key behavior for selecting building cards."
    );

    TowerKeyOverlay = Config.Bind(
      section: "Overlay",
      key: "Towers",
      defaultValue: true,
      description: "Displays text on tower and building cards showcasing the key said slot is bound to."
    );
    UpgradeCardKeyOverlay = Config.Bind(
      section: "Overlay",
      key: "UpgradeCards",
      defaultValue: true,
      description: "Displays text on upgrade cards showcasing the key said slot is bound to."
    );
    MonsterCardKeyOverlay = Config.Bind(
      section: "Overlay",
      key: "MonsterCards",
      defaultValue: true,
      description: "Displays text on monster cards showcasing the key said slot is bound to."
    );

    BanishEnabled = Config.Bind(
      section: "Banish",
      key: "Enabled",
      defaultValue: true,
      description: "Toggle the banish feature; requires the Banish plugin."
    );
    BanishColorsEnabled = Config.Bind(
      section: "Banish",
      key: "CardColors",
      defaultValue: true,
      description: "Change the colors of upgrade cards when the banish key is held."
    );
    BanishColor = Config.Bind(
      section: "Banish",
      key: "Color",
      defaultValue: new Color(1, 0.0f, 0.0f, 1),
      description: "Color to change upgrade cards to when the banish key is held."
    );
    BanishKey = Config.Bind(
      section: "Banish",
      key: "Key",
      defaultValue: KeyCode.LeftControl,
      description: "While this key is held, using a slot key will banish a card instead of selecting it."
    );

    BuildingKey = Config.Bind(
      section: "Keybinds",
      key: "BuildingKey",
      defaultValue: KeyCode.LeftShift,
      description: "While this key is held, using a slot key will select a building card instead of a tower card."
    );

    for (var i = 1; i < SelectKeys.Length; i++)
    {
      SelectKeys[i] = Config.Bind(
        section: "Keybinds",
        key: $"Slot{i}",
        defaultValue: DefaultKeyCodeForSlot(i),
        description: $"Key to quick select the item at slot {i}"
      );
    }

    BanishPluginPresent = Chainloader.PluginInfos.ContainsKey("AgusBut.BanishCards");
    Logger.LogInfo($"Banish plugin present: {BanishPluginPresent}");

    TowerKeyOverlayFeature = new TowerKeyOverlay();
    BuildingKeyOverlayFeature = new BuildingKeyOverlay();
    UpgradeCardKeyOverlayFeature = new UpgradeCardKeyOverlay();
    MonsterCardKeyOverlayFeature = new MonsterCardKeyOverlay();
    BanishColorsFeature = new BanishColors();
    SelectSlotFeature = new SelectSlot();
    SelectSlotFeature.OnStart();

    Patches = new Harmony("daymxn.RogueTower.SlotKeybinds");
    Patches.PatchAll();

    Logger.LogInfo($"Loaded plugin {MyPluginInfo.PLUGIN_GUID}!");
  }

  public void Update()
  {
    BanishColorsFeature.OnUpdate();
    SelectSlotFeature.OnUpdate();
  }

  public void OnDestroy()
  {
    Patches.UnpatchSelf();

    TowerKeyOverlayFeature?.OnDestroy();
    BuildingKeyOverlayFeature?.OnDestroy();
    UpgradeCardKeyOverlayFeature?.OnDestroy();
    MonsterCardKeyOverlayFeature?.OnDestroy();
    BanishColorsFeature?.OnDestroy();
  }
  
  public KeyCode GetKeyForSlot(int slot) => SelectKeys.ElementAtOrDefault(slot + 1)?.Value ?? KeyCode.None;
}