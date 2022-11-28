using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class Deck : ICascadable {
    [SerializeField] private List<HeroSlot> _heroSlotList;
    private List<ICascadable> _cascadables;

    public List<HeroSlot> HeroSlotList {
        get {
            return _heroSlotList;
        }
    }


    public List<ICascadable> Cascadables {
        get {
            return _cascadables;
        }

        set {
            _cascadables = value;
            _cascadables[0].Cascade(null);
        }
    }

    public Deck() {
        _heroSlotList = new List<HeroSlot>();
        _cascadables = new List<ICascadable>();

    }

    public void Cascade(ICascadable causedBy, PropertyInfo changedProperty = null, object changedValue = null) {
        if (causedBy == null) {
            causedBy = this;
        }
        foreach (ICascadable cascadable in Cascadables) {
            cascadable.Cascade(causedBy, changedProperty, changedValue);
        }
    }
}


