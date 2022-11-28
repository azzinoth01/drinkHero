using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class Player : Character, IHandCards, IPlayer {
    [SerializeField] private string _name;

    [SerializeField] private int _attack;
    [SerializeField] private int _maxRessource;
    [SerializeField] private int _ressource;

    [SerializeField] private List<Card> _handCards;
    public List<Card> HandCards => _handCards;

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
    public event Action<int> HealthChange;
    public event Action<int> ShieldChange;
    public event Action TurnEnded;


    public GameDeck GameDeck {
        get {
            return _gameDeck;
        }

        set {
            if (_gameDeck != null) {
                _gameDeck.Cascadables.Remove(this);
            }
            _gameDeck = value;
            if (_gameDeck != null) {
                _gameDeck.Cascadables.Add(this);
                ResetPlayer();
            }
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
            _handCards = new List<Card>();
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
        _handCards = new List<Card>();

    }


    public void ResetRessource() {
        _ressource = _maxRessource;
        UpdateEnergyUI(_ressource);
    }

    public void StartTurn() {
        for (int i = 0; i < _buffList.Count;) {
            _buffList[i].ActivateStatusEffect(this, ActivationTimeEnum.turnStartActive);
            if (_buffList[i].CheckIfEffectIsOver()) {
                _buffList.RemoveAt(i);
            }
            else {
                i = i + 1;
            }
        }


        //draw until 5 cards
        //Debug.Log(_handCards.Count);
        for (int i = _handCards.Count; i < 4;) {
            _handCards.Add(_gameDeck.DrawCard());
            i = i + 1;
        }
        UpdateHandCards?.Invoke();
        ResetRessource();

        UpdateUI();

    }

    public void PlayHandCard(int index) {

        Card card = _handCards[index];

        if (card.Costs > _ressource) {
            return;
        }



        _ressource = _ressource - card.Costs;

        GlobalGameInfos.Instance.EnemyObject.enemy.TakeDmg(card.Attack);


        if (_shield + card.Shield < 0) {
            _shield = 0;
        }
        else {
            _shield += card.Shield;
        }
        _health = _health + card.Health;
        if (_health > _maxHealth) {
            _health = _maxHealth;
        }


        if (card.StatusEffects != null && card.StatusEffects.Count > 0) {
            foreach (Buff buff in card.StatusEffects) {
                Buff b = (Buff)buff.Clone();
                _buffList.Add(b);
            }
        }



        _gameDeck.ScrapCard(card);
        _handCards.RemoveAt(index);

        UpdateHealthUI(card.Health);
        UpdateShieldUI(card.Shield);
        UpdateEnergyUI(-card.Costs);

    }


    public void TakeDmg(int dmg) {
        int shieldDmg = 0;
        if (_shield > 0) {
            if (_shield > dmg) {
                _shield = _shield - dmg;
                shieldDmg = -dmg;
                dmg = 0;

                // Update Shield Counter UI
            }
            else {
                dmg = dmg - _shield;
                shieldDmg = -_shield;
                _shield = 0;

                // Update Shield Counter UI
            }
            UpdateShieldUI(shieldDmg);
        }
        int healthDmg = 0;
        if (_health - dmg < 0) {
            healthDmg = -_health;
            _health = 0;
        }
        else {
            healthDmg = -dmg;
            _health -= dmg;
        }

        UpdateHealthUI(healthDmg);


        if (_health <= 0) {

            PlayerDeath();
        }
    }

    public void PlayerDeath() {
        GameOverEvent?.Invoke();
    }

    private void UpdateUI(int deltaHealth = 0, int deltaShield = 0, int deltaEnergy = 0) {

        UpdateHealthUI(deltaHealth);
        UpdateShieldUI(deltaShield);
        UpdateEnergyUI(deltaEnergy);
    }

    private void UpdateHealthUI(int deltaValue) {
        HealthChange?.Invoke(deltaValue);
    }

    private void UpdateEnergyUI(int deltaValue) {
        RessourceChange?.Invoke(deltaValue);
    }

    private void UpdateShieldUI(int deltaValue) {
        ShieldChange?.Invoke(deltaValue);
    }

    public override void Cascade(ICascadable causedBy, PropertyInfo changedProperty = null, object changedValue = null) {

        if (causedBy is Hero || causedBy is Card) {
            ResetPlayer();
        }
        base.Cascade(causedBy, changedProperty, changedValue);
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

    int ICharacter.MaxHealth() {
        return _maxHealth;
    }

    public int CurrentHealth() {
        return _health;
    }

    public int CurrentShield() {
        return _shield;
    }

    public void EndTurn() {
        Debug.Log("player turn end");
        TurnEnded?.Invoke();
    }
}
