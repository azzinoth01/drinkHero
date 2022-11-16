using System;
using UnityEngine;

[Serializable]
public class HeroSlot {
    [SerializeField] private Hero _hero;
    [SerializeField] private uint _slotID;

    public Hero Hero {
        get {
            return _hero;
        }


    }
}
