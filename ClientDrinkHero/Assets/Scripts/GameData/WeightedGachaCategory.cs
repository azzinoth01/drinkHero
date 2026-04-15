using System;
using UnityEngine;


[Serializable]
public class WeightedGachaCategory
{
    [SerializeField] private int _weight;
    [SerializeField] private GachaCategoryData _category;

    public WeightedGachaCategory() {

    }

    public int Weight {
        get {
            return _weight;
        }
    }

    public GachaCategoryData Category {
        get {
            return _category;
        }
    }
}