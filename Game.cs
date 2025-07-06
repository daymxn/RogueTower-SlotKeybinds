#nullable enable
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SlotKeybindsPlugin;

/**
 * Static class for providing common functionality specific to RogueTower.
 */
public static class Game
{
  public static CardManager? GetCardManager()
  {
    return CardManager.instance;
  }

  public static TowerUnlockManager? GetTowerUnlockManager()
  {
    return TowerUnlockManager.instance;
  }

  public static SelectMode DetermineSelectMode()
  {
    if (!GetCardManager()) return SelectMode.None;
    if (!DrawingCards()) return SelectMode.TowerSelect;
    return MonsterCardUIActive() ? SelectMode.MonsterCardSelect : SelectMode.CardSelect;
  }

  public static GameObject? GetCardHolder(int slot)
  {
    var cardManager = GetCardManager();
    if (!cardManager) return null;

    var cardHolders = AccessTools.Field(typeof(CardManager), "cardHolders")?.GetValue(cardManager) as GameObject[];
    if (cardHolders == null)
    {
      Plugin.Logger.LogWarning("cardHolders field not found.");
      return null;
    }

    if (cardHolders.Length >= slot) return cardHolders[slot];

    Plugin.Logger.LogWarning(
      $"cardHolders array is smaller than slot number (length is {cardHolders.Length}, but slot was {slot}).");
    return null;
  }

  public static Text[] GetCardTitles()
  {
    var cardManager = GetCardManager();
    if (!cardManager) return [];

    var cardHolders = AccessTools.Field(typeof(CardManager), "titles")?.GetValue(cardManager) as Text[];
    return cardHolders ?? [];
  }
  
  public static Text[] GetMonsterCardTitles()
  {
    var cardManager = GetCardManager();
    if (!cardManager) return [];

    var cardHolders = AccessTools.Field(typeof(CardManager), "monsterTitles")?.GetValue(cardManager) as Text[];
    return cardHolders ?? [];
  }

  public static Image[] GetCardImages()
  {
    var cardManager = GetCardManager();
    if (!cardManager) return [];

    var images = AccessTools.Field(typeof(CardManager), "images")?.GetValue(cardManager) as Image[];
    return images ?? [];
  }

  public static BuildButtonUI? GetBuildingButton(int slot) =>
    GetUnlockedBuildings()?.ElementAtOrDefault(slot)?.GetComponentInChildren<BuildButtonUI>();

  public static BuildButtonUI? GetTowerButton(int slot) =>
    GetUnlockedTowers()?.ElementAtOrDefault(slot)?.GetComponentInChildren<BuildButtonUI>();

  private static bool MonsterCardUIActive() => MonsterCardUI()?.activeInHierarchy ?? false;

  private static bool DrawingCards() => GetCardManager()?.drawingCards ?? false;

  private static GameObject? MonsterCardUI()
  {
    var cardManager = GetCardManager();
    if (!cardManager) return null;

    return AccessTools.Field(typeof(CardManager), "monsterCardUI")?.GetValue(cardManager) as GameObject;
  }

  public static List<GameObject>? GetUnlockedTowers()
  {
    Plugin.Logger.LogDebug("Getting unlocked towers.");
    var towerManager = GetTowerUnlockManager();
    if (!towerManager) return null;

    Plugin.Logger.LogDebug("Getting unlocked towers field.");
    var unlockedTowers =
      AccessTools.Field(typeof(TowerUnlockManager), "unlockedTowers")?.GetValue(towerManager) as List<GameObject>;
    if (unlockedTowers != null) return unlockedTowers;

    Plugin.Logger.LogWarning("unlockedTowers field not found.");
    return null;
  }

  public static List<GameObject>? GetUnlockedBuildings()
  {
    var towerManager = GetTowerUnlockManager();
    if (!towerManager) return null;

    var unlockedBuildings =
      AccessTools.Field(typeof(TowerUnlockManager), "unlockedBuildings")?.GetValue(towerManager) as List<GameObject>;
    if (unlockedBuildings != null) return unlockedBuildings;

    Plugin.Logger.LogWarning("unlockedBuildings field not found.");
    return null;
  }
}

public enum SelectMode
{
  None,
  TowerSelect,
  CardSelect,
  MonsterCardSelect
}