using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GachaCategorieData
{
    [SerializeField] private string _id;
    [SerializeField] private string _name;
    [SerializeField] private int _weightedValue;
    [SerializeField] private List<string> _gachaitemList;

    public GachaCategorieData() {
        _id = Guid.NewGuid().ToString("N");
        _gachaitemList = new List<string>();
    }

    public string Id {
        get {
            return _id;
        }
    }

    public string Name {
        get {
            return _name;
        }
    }

    public int WeightedValue {
        get {
            return _weightedValue;
        }
    }

    public List<string> GachaitemList {
        get {
            return _gachaitemList;
        }
    }
}