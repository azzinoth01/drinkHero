using System;
using UnityEngine;

[Serializable]
public class SavedItem
{
    [SerializeField] private string _id;
    [SerializeField] private int _amount;

    public string Id {
        get {
            return _id;
        }
    }

    public int Amount {
        get {
            return _amount;
        }
        set {
            _amount = value;
        }
    }

    private SavedItem() {

    }

    public SavedItem(UpgradeItemData data) {
        _id = data.Id;
        _amount = 0;
    }
}