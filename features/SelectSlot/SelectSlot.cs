using System;
using SlotKeybindsPlugin.features.Banish;
using UnityEngine;

namespace SlotKeybindsPlugin.features.SelectSlot;

public class SelectSlot : Feature
{
  private SelectTower _selectTower;
  private SelectBuilding _selectBuilding;
  private SelectUpgradeCard _selectUpgradeCard;
  private SelectMonsterCard _selectMonsterCard;
  private BanishUpgradeCard _banishUpgradeCard;

  public override void OnStart()
  {
    _selectTower = new SelectTower();
    _selectBuilding = new SelectBuilding();
    _selectUpgradeCard = new SelectUpgradeCard();
    _selectMonsterCard = new SelectMonsterCard();
    _banishUpgradeCard = new BanishUpgradeCard();
  }

  public override void OnUpdate()
  {
    for (var slot = 1; slot < Plugin.Instance.SelectKeys.Length; slot++)
    {
      if (Input.GetKeyUp(Plugin.Instance.SelectKeys[slot].Value) && !OnSlotSelected(slot - 1))
      {
        Logger.LogWarning($"Failed to select slot {slot}");
      }
    }
  }

  private bool OnSlotSelected(int slot)
  {
    var mode = Game.DetermineSelectMode();

    return mode switch
    {
      SelectMode.None => true,
      SelectMode.CardSelect => BanishKeyHeld()
        ? _banishUpgradeCard.Banish(slot) || _selectUpgradeCard.Select(slot)
        : _selectUpgradeCard.Select(slot),
      SelectMode.MonsterCardSelect => _selectMonsterCard.Select(slot),
      SelectMode.TowerSelect => BuildingKeyHeld()
        ? _selectBuilding.Select(slot) || _selectTower.Select(slot)
        : _selectTower.Select(slot),
      _ => throw new ArgumentOutOfRangeException()
    };
  }

  private bool BanishKeyHeld() => Input.GetKey(Plugin.Instance.BanishKey.Value);
  private bool BuildingKeyHeld() => Input.GetKey(Plugin.Instance.BuildingKey.Value);
}