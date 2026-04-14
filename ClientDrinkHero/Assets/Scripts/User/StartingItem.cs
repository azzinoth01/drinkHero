using System;
using UnityEngine;

[Serializable]
public class StartingItem
{
    [SerializeField] private int _amount;
    [SerializeField] private UpgradeItemData _upgradeItem;

    public int Amount {
        get {
            return _amount;
        }
    }

    public UpgradeItemData UpgradeItem {
        get {
            return _upgradeItem;
        }
    }
}