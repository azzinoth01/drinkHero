using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData
{
    [ReadOnly]
    [SerializeField] private string _id;

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _shield;
    [SerializeField] private string _spritePath;
    [SerializeField] private bool _isBoss;
    [SerializeField] private int _moneyDrop;

    public EnemyData() {
        _id = Guid.NewGuid().ToString("N");
    }

    public string Id {
        get {
            return _id;
        }
    }

    public int MaxHealth {
        get {
            return _maxHealth;
        }
    }

    public int Shield {
        get {
            return _shield;
        }
    }

    public string SpritePath {
        get {
            return _spritePath;
        }
    }

    public bool IsBoss {
        get {
            return _isBoss;
        }
    }

    public int MoneyDrop {
        get {
            return _moneyDrop;
        }
    }
}