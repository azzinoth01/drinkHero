
#if CLIENT
using System;
using System.Collections.Generic;
using UnityEngine;
#endif

[Table("CardToHero"), Serializable]
public class CardToHero : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int? _refCard;
    [SerializeField] private int? _refHero;
    [NonSerialized] private CardDatabase _card;
    [NonSerialized] private HeroDatabase _hero;
    private DataRequestStatusEnum _cardRequested;
#endif


#if SERVER
    private int _id;
    private int? _refCard;
    private int? _refHero;
    private CardDatabase _card;
    private HeroDatabase _hero;
    private DataRequestStatusEnum _cardRequested;
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
#if SERVER
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

    public HeroDatabase Hero {
        get {
            if (_refHero == null) {
                return null;
            }
            _hero = DatabaseManager.GetDatabaseItem<HeroDatabase>(_refHero);
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

#endif

#if CLIENT
    public CardDatabase GetCard(out bool waitOnData) {
        if (_card == null && _cardRequested == DataRequestStatusEnum.NotRequested) {
            _cardRequested = DataRequestStatusEnum.Requested;
            ClientFunctions.GetCardDatabaseByKeyPair("ID\"" + _refCard + "\"");
            WriteBackData writeBackData = new WriteBackData(this, GetType().GetMethod(nameof(SetCard)), typeof(CardDatabase));
            GlobalGameInfos.writeServerDataTo.Enqueue(writeBackData);

        }
        else if (_cardRequested == DataRequestStatusEnum.Recieved) {
            waitOnData = false;
            return _card;
        }
        waitOnData = true;
        return null;
    }

    public void SetCard(List<CardDatabase> card) {
        _card = card[0];
        _cardRequested = DataRequestStatusEnum.Recieved;

    }

#endif
    public CardToHero() {
        _cardRequested = DataRequestStatusEnum.NotRequested;
    }


}
