using System;
using UnityEngine;


[Serializable]
public class GachaItem
{
    [SerializeField] private int _weight;
    [SerializeField] private ScriptableObject _item;

    public GachaItem() {

    }

    public int Weight {
        get {
            return _weight;
        }
    }

    public ScriptableObject Item {
        get {
            return _item;
        }
    }
}