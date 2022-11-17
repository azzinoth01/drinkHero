using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class Player : Character {
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

    public static event Action<float, float> updatePlayerHealthUI;
    public static event Action<float, float> updatePlayerEnergyUI;
    public static event Action<int> updatePlayerShieldUI;
    public static event Action updateHandCardUI, playerDamageReceived, playerDamageBlocked, playerHealed, playerShieldUp;

    public Player(GameDeck gameDeck) : base() {
        GameDeck = gameDeck;
        ResetPlayer();

    }

    public Player() : base() {

    }

    private void ResetPlayer() {
        if (_gameDeck != null) {
            _handCards = new List<Card>();
            _gameDeck.RecreateDeck();

            //define max handcards globaly
            for (int i = 0; i < 5;) {

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
        //UpdateEnergyUI();
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

        for (int i = _handCards.Count; i < 5;) {
            _handCards.Add(_gameDeck.DrawCard());
            i = i + 1;
        }
        updateHandCardUI?.Invoke();
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

        UpdateHealthUI();
        UpdateEnergyUI();
        UpdateShieldUI();
    }


    public void TakeDmg(int dmg) {

        if (_shield > 0) {
            if (_shield > dmg) {
                _shield = _shield - dmg;
                dmg = 0;

                // Update Shield Counter UI
            }
            else {
                dmg = dmg - _shield;
                _shield = 0;

                // Update Shield Counter UI
            }
            UpdateShieldUI();
        }

        if (_health - dmg < 0) {
            _health = 0;
        }
        else {
            _health -= dmg;
        }

        UpdateHealthUI();


        if (_health <= 0) {

            PlayerDeath();
        }
    }

    public void PlayerDeath() {

    }

    private void UpdateUI() {
        UpdateEnergyUI();
        UpdateHealthUI();
        UpdateShieldUI();
    }

    private void UpdateHealthUI() {
        updatePlayerHealthUI?.Invoke(_health, _maxHealth);
    }

    private void UpdateEnergyUI() {
        updatePlayerEnergyUI?.Invoke(_ressource, _maxRessource);
    }

    private void UpdateShieldUI() {
        updatePlayerShieldUI?.Invoke(_shield);
    }

    public override void Cascade(ICascadable causedBy, PropertyInfo changedProperty = null, object changedValue = null) {

        if (causedBy is Hero || causedBy is Card) {
            ResetPlayer();
        }
        base.Cascade(causedBy, changedProperty, changedValue);
    }
}
