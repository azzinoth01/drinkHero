using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Player : Character, IHandCards, IPlayer, IPlayerAction {
    const int MaxHandCards = 5;

    [SerializeField] private string _name;

    [SerializeField] private int _attack;
    [SerializeField] private int _maxRessource;
    [SerializeField] private int _ressource;

    [SerializeField] private List<DeckCardContainer> _handCards;
    public List<DeckCardContainer> HandCards => _handCards;

    private Dictionary<int, DeckCardContainer> _hand;


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
    public event Action<int> DiscardCardAction;


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
            _handCards = new List<DeckCardContainer>();
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
        _handCards = new List<DeckCardContainer>();

    }


    public void ResetRessource() {
        _ressource = _maxRessource;
        RessourceChange?.Invoke(_ressource);
    }



    public bool PlayHandCard(int index) {

        DeckCardContainer container = _handCards[index];
        CardDatabase card = container.Card;


        if (card.Cost > _ressource) {
            return false;
        }


        //start Animation

        if (card.AnimationKey != null && card.AnimationKey != "") {
            VFXObjectContainer.Instance.PlayAnimation(card.AnimationKey);

        }


        // do card effect


        _handCards.RemoveAt(index);




        _ressource = _ressource - card.Cost;
        RessourceChange?.Invoke(-card.Cost);


        // Action Start
        CheckDebuffsAndBuffs(ActivationTimeEnum.actionStart);


        PlayerTeam.Instance.PlayAnimation("Attack");
        // Action
        for (int i = 0; i < _buffMultihit;) {
            Action(card);

            i = i + 1;
        }

        // Action End
        CheckDebuffsAndBuffs(ActivationTimeEnum.actionFinished, _dmgCausedThisAction);


        _dmgCausedThisAction = 0;
        _buffMultihit = 1;
        _discardedHandCardsThisAction = 0;

        _gameDeck.ScrapCard(container);

        return true;
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
                    if (b.ActivateEffectBase(GlobalGameInfos.Instance.EnemyObject.Enemy, ActivationTimeEnum.onCast) == true) {
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


    public void UpdateUI(int deltaHealth = 0, int deltaShield = 0, int deltaEnergy = 0) {
        base.UpdateUI(deltaHealth, deltaShield);
        RessourceChange?.Invoke(deltaEnergy);
    }




    public int HandCardCount() {
        return _handCards.Count;
    }

    public ICardDisplay GetHandCard(int index) {
        if (index < 0 || index >= HandCards.Count) {
            return null;
        }
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

        DrawCardsFromDeck(MaxHandCards);

        UpdateHandCards?.Invoke();
        ResetRessource();

        UpdateUI();

        if (_skipTurn > 0) {
            _skipTurn = _skipTurn - 1;
            InvokeEndTurn();
        }
    }

    private void DrawCardsFromDeck(int value) {
        AudioController.Instance.PlayAudio(AudioType.SFXDrawCards);
        for (int i = 0; i < value;) {
            if (_handCards.Count >= MaxHandCards) {
                break;
            }
            _handCards.Add(_gameDeck.DrawCard());


            i = i + 1;
        }

    }


    public override void SwapShieldWithEnemy() {

        UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "SHIELD SWAP");
        UIDataContainer.Instance.PlayerText.SpawnFlyingText(FlyingTextEnum.effect, "SHIELD SWAP");

        int tempShield = GlobalGameInfos.Instance.EnemyObject.Enemy.shield;
        GlobalGameInfos.Instance.EnemyObject.Enemy.shield = Shield;
        Shield = tempShield;
        UpdateUI();
        GlobalGameInfos.Instance.EnemyObject.Enemy.UpdateUI();
    }

    public override void AttackEnemy(int value) {

        value = _dmgModifier.CalcValue(value);

        GlobalGameInfos.Instance.EnemyObject.Enemy.TakeDmg(value);
        _dmgCausedThisAction = _dmgCausedThisAction + value;
    }

    public override void DiscardHandCards(int value) {

        UIDataContainer.Instance.PlayerText.SpawnFlyingText(FlyingTextEnum.effect, "DISCARD CARDS -" + value);
        for (int i = 0; i < value;) {
            if (_handCards.Count == 0) {
                break;
            }
            DiscardCard();

            _discardedHandCardsThisAction = _discardedHandCardsThisAction + 1;
            i = i + 1;
        }

    }

    private void DiscardCard() {
        int index = Random.Range(0, _handCards.Count);
        DeckCardContainer card = _handCards[index];

        DiscardCardAction?.Invoke(index);

        _handCards.RemoveAt(index);
        _gameDeck.ScrapCard(card);
    }

    public override void Mana(int value) {
        UIDataContainer.Instance.PlayerText.SpawnFlyingText(FlyingTextEnum.effect, "MANA +" + value);
        _ressource = _ressource + value;
        RessourceChange?.Invoke(value);
    }

    public override void DrawCard(int value) {
        UIDataContainer.Instance.PlayerText.SpawnFlyingText(FlyingTextEnum.effect, "DRAW CARD +" + value);
        DrawCardsFromDeck(value);
    }
}
