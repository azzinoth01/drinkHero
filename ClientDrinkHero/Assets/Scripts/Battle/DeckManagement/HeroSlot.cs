using System;
using UnityEngine;

[Serializable]
public class HeroSlot {
    [SerializeField] private HeroDatabase _hero;
    [SerializeField] private int _slotID;


    public HeroDatabase Hero {
        get {
            return _hero;
        }
        set {

            _hero = value;

        }

    }

    public int SlotID {
        get {
            return _slotID;
        }

        set {
            _slotID = value;
        }
    }

    public HeroSlot() {

    }


}
