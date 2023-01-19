#if CLIENT
using System;
using UnityEngine;
#endif

[Serializable, Table("UserHeroToCard")]
public class UserHeroToCardDatabase : DatabaseItem {
#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int? _refUserHero;
    [SerializeField] private int? _refCard;

#endif
#if SERVER
    private int _id;
    private int? _refUserHero;
    private int? _refCard;

    private CardDatabase _card;

#endif

    [Column("ID"), PrimaryKey]
    public int Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }
    [Column("RefUserHero")]
    public int? RefUserHero {
        get {
            return _refUserHero;
        }

        set {
            _refUserHero = value;
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

    public CardDatabase Card {
        get {
            if (_refCard == null) {
                return null;
            }
            _card = DatabaseManager.GetDatabaseItem<CardDatabase>(_refCard);
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

}
