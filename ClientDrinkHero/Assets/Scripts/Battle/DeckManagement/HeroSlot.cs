using System;
using UnityEngine;

[Serializable]
public class HeroSlot {
    [SerializeField] private HeroDatabase _hero;
    [SerializeField] private uint _slotID;


    public HeroDatabase Hero {
        get {
            return _hero;
        }
        set {

            _hero = value;

        }

    }


    public HeroSlot() {

    }


}
