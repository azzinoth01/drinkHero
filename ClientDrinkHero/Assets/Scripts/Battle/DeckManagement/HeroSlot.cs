using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class HeroSlot : ICascadable {
    [SerializeField] private Hero _hero;
    [SerializeField] private uint _slotID;
    private List<ICascadable> _cascadables;

    public Hero Hero {
        get {
            return _hero;
        }
        set {
            if (_hero != null) {
                _hero.Cascadables.Remove(this);
            }

            _hero = value;
            if (_hero != null) {
                _hero.Cascadables.Add(this);
            }
        }

    }

    public List<ICascadable> Cascadables {
        get {
            return _cascadables;
        }

        set {
            _cascadables = value;
        }
    }

    public HeroSlot() {
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
