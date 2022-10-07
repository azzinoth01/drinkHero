using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Deck {
    [SerializeField] private List<HeroSlot> _heroSlotList;

    public List<HeroSlot> HeroSlotList {
        get {
            return _heroSlotList;
        }


    }

    public Deck() {
        _heroSlotList = new List<HeroSlot>();
    }
}
