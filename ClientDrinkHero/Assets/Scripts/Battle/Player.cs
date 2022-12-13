using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : Character, IHandCards, IPlayer {
    [SerializeField] private string _name;

    [SerializeField] private int _attack;
    [SerializeField] private int _maxRessource;
    [SerializeField] private int _ressource;

    [SerializeField] private List<CardDatabase> _handCards;
    public List<CardDatabase> HandCards => _handCards;

    [SerializeField] private GameDeck _gameDeck;

    public int PlayerEnergy => _ressource;
    public int PlayerMaxEnergy => _maxRessource;

    public int PlayerShield => _shield;

    //public static Action onPlayerDeath;



    //public static event Action<float, float> updatePlayerHealthUI;
    //public static event Action<float, float> updatePlayerEnergyUI;
    //public static event Action<int> updatePlayerShieldUI;
    //public static event Action updateHandCardUI;

    public static event Action playerDamageReceived, playerDamageBlocked, playerHealed, playerShieldUp;

    public event Action<int> RessourceChange;
    public event Action UpdateHandCards;
    public event Action GameOverEvent;



    public GameDeck GameDeck {
        get {
            return _gameDeck;
        }

        set {

            _gameDeck = value;
            ResetPlayer();

        }
    }
    public Player(GameDeck gameDeck) : base() {
        GameDeck = gameDeck;
        ResetPlayer();
        UIDataContainer.Instance.Player = this;
    }

    public Player() : base() {
        UIDataContainer.Instance.Player = this;
    }

    private void ResetPlayer() {
        if (_gameDeck != null) {
            _handCards = new List<CardDatabase>();
            _gameDeck.RecreateDeck();

            //define max handcards globaly
            for (int i = 0; i < 4;) {

                _handCards.Add(_gameDeck.DrawCard());

                i = i + 1;
            }

            foreach (HeroSlot heroSlot in _gameDeck.Deck.HeroSlotList) {
                _maxHealth = _maxHealth + heroSlot.Hero.Health;
            }
            _health = _maxHealth;
            //define max base ressources globaly

            _maxRessource = 10;
            _ressource = 10;
        }

    }

    public override void Clear() {

        base.Clear();
        _name = null;
        _attack = 0;
        _maxRessource = 10;
        _ressource = 0;
        _gameDeck = null;
        _handCards = new List<CardDatabase>();

    }


    public void ResetRessource() {
        _ressource = _maxRessource;
        RessourceChange?.Invoke(_ressource);
    }



    public void PlayHandCard(int index) {

        CardDatabase card = _handCards[index];

        if (card.Cost > _ressource) {
            return;
        }

        _ressource = _ressource - card.Cost;
        RessourceChange?.Invoke(-card.Cost);


        // Action Start
        CheckDebuffsAndBuffs(ActivationTimeEnum.actionStart);


        // Action
        for (int i = 0; i < _buffMultihit;) {
            Action(card);

            i = i + 1;
        }

        // Action End

        _buffMultihit = 1;

        CheckDebuffsAndBuffs(ActivationTimeEnum.actionFinished, _dmgCausedThisAction);
        _dmgCausedThisAction = 0;

        _gameDeck.ScrapCard(card);
        _handCards.RemoveAt(index);

    }


    public void Action(CardDatabase card) {
        if (card.CardEffectList != null && card.CardEffectList.Count > 0) {
            foreach (CardToEffect cardToEffect in card.CardEffectList) {
                Effect effect = EffectConverter.ConvertEffectIntoEffectType(cardToEffect.Effect);
                if (effect is IBuff) {
                    IBuff b = (IBuff)effect;
                    if (b.ActivateEffect(this, ActivationTimeEnum.onCast) == true) {
                        _buffList.Add(b);
                    }
                }
                else if (effect is IDebuff) {
                    IDebuff b = (IDebuff)effect;
                    if (b.ActivateEffect(GlobalGameInfos.Instance.EnemyObject.Enemy, ActivationTimeEnum.onCast) == true) {
                        GlobalGameInfos.Instance.EnemyObject.Enemy.DebuffList.Add(b);
                    }
                }
                else if (effect is ISkill) {
                    ISkill b = (ISkill)effect;
                    b.ActivateEffect(this, ActivationTimeEnum.onCast);
                }
            }

        }
    }





    protected override void Death() {
        GameOverEvent?.Invoke();
    }


    protected void UpdateUI(int deltaHealth = 0, int deltaShield = 0, int deltaEnergy = 0) {
        base.UpdateUI(deltaHealth, deltaShield);
        RessourceChange?.Invoke(deltaEnergy);
    }




    public int HandCardCount() {
        return _handCards.Count;
    }

    public ICardDisplay GetHandCard(int index) {
        return HandCards[index];
    }

    public int MaxRessource() {
        return 10;
    }

    public int CurrentRessource() {
        return _ressource;
    }

    public IHandCards GetHandCards() {
        return this;
    }

    public override void EndTurn() {
        Debug.Log("player turn end");

        CheckDebuffsAndBuffs(ActivationTimeEnum.turnEnd);


    }

    public override void StartTurn() {


        CheckDebuffsAndBuffs(ActivationTimeEnum.turnStart);

        //draw until 5 cards
        //Debug.Log(_handCards.Count);
        for (int i = _handCards.Count; i < 4;) {
            _handCards.Add(_gameDeck.DrawCard());
            i = i + 1;
        }
        UpdateHandCards?.Invoke();
        ResetRessource();

        UpdateUI();

        if (_skipTurn == true) {
            _skipTurn = false;
            EndTurn();
        }
    }

    public override void SwapShieldWithEnemy() {
        int tempShield = GlobalGameInfos.Instance.EnemyObject.Enemy.shield;
        GlobalGameInfos.Instance.EnemyObject.Enemy.shield = Shield;
        Shield = tempShield;
        UpdateUI();
        GlobalGameInfos.Instance.EnemyObject.Enemy.UpdateUI();
    }

    public override void AttackEnemy(int value) {
        GlobalGameInfos.Instance.EnemyObject.Enemy.TakeDmg(value);
        _dmgCausedThisAction = _dmgCausedThisAction + value;
    }
}
