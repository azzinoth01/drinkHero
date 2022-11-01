using System;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;


[Table("Card"), Serializable]
public class CardToHero : DatabaseItem {


    [SerializeField] private int _id;
    [SerializeField] private int? _refCard;
    [SerializeField] private int? _refHero;
    [SerializeField] CardDatabase _card;
    [SerializeField] HeroDatabase _hero;

    [Column("ID"), PrimaryKey, AutoIncrement]
    public int Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }
    [Column("RefCard")]
    public int? RefCard {
        get {
            return _refCard;
        }

        set {
            _refCard = value;
        }
    }
    [Column("RefHero")]
    public int? RefHero {
        get {
            return _refHero;
        }

        set {
            _refHero = value;
        }
    }

    public CardDatabase Card {
        get {
            if (_refCard == null) {
                return null;
            }
            _card = GetDatabaseItem<CardDatabase>((int)_refCard);

            return _card;
        }

        set {
            if (_card == null) {
                _refCard = null;
            }
            else {
                _refCard = value.Id;
            }
            _card = value;
        }
    }

    public HeroDatabase Hero {
        get {
            if (_refHero == null) {
                return null;
            }
            _hero = GetDatabaseItem<HeroDatabase>((int)_refHero);
            return _hero;
        }

        set {
            if (_hero == null) {
                _refHero = null;
            }
            else {
                _refHero = value.Id;
            }
            _hero = value;
        }
    }

    public CardToHero() {

    }


}
