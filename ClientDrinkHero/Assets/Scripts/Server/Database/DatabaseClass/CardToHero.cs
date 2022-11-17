
using System;
using System.Collections.Generic;
using UnityEngine;


[Table("CardToHero"), Serializable]
public class CardToHero : DatabaseItem {


    [SerializeField] private long _id;
    [SerializeField] private string _refCard;
    [SerializeField] private string _refHero;
    [NonSerialized] private CardDatabase _card;
    [NonSerialized] private HeroDatabase _hero;

    private DataRequestStatusEnum _cardRequested;

    [Column("ID"), PrimaryKey]
    public long Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }
    [Column("RefCard")]
    public string RefCard {
        get {
            return _refCard;
        }

        set {
            _refCard = value;
        }
    }
    [Column("RefHero")]
    public string RefHero {
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
            _card = DatabaseManager.GetDatabaseItem<CardDatabase>(long.Parse(_refCard));

            return _card;
        }

        set {
            if (_card == null) {
                _refCard = null;
            }
            else {
                _refCard = value.Id.ToString();
            }
            _card = value;
        }
    }
#if SERVER
    public HeroDatabase Hero {
        get {
            if (_refHero == null) {
                return null;
            }
            _hero = DatabaseManager.GetDatabaseItem<HeroDatabase>(long.Parse(_refHero));
            return _hero;
        }

        set {
            if (_hero == null) {
                _refHero = null;
            }
            else {
                _refHero = value.Id.ToString();
            }
            _hero = value;
        }
    }

#endif

#if CLIENT
    public CardDatabase GetCard(out bool waitOnData) {
        if (_card == null && _cardRequested == DataRequestStatusEnum.NotRequested) {
            _cardRequested = DataRequestStatusEnum.Requested;
            ClientFunctions.GetCardDatabaseByKeyPair("ID\"" + _refHero + "\"");
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
