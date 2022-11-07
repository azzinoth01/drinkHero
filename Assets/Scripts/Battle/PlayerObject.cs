using System;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private SimpleAudioEvent _playerDamageSound;
    [SerializeField] private SimpleAudioEvent _playerDamageShieldedSound;
    [SerializeField] private SimpleAudioEvent _playerhealedSound;
    [SerializeField] private SimpleAudioEvent _playerShieldUpSound;
    
    public Player Player => _player;

    //remove after prototype
    public UserObject testUserField;

    private void Awake()
    {
        Player.playerDamageReceived += PlayerDamageFeedback;
        Player.playerDamageShielded += PlayerDamageShieldedFeedback;
        Player.playerHealed += PlayerHealedFeedback;
        Player.playerShieldUp += PlayerShieldUpFeedback;
    }

    private void OnDisable()
    {
        Player.playerDamageReceived -= PlayerDamageFeedback;
        Player.playerDamageShielded -= PlayerDamageShieldedFeedback;
        Player.playerHealed -= PlayerHealedFeedback;
        Player.playerShieldUp -= PlayerShieldUpFeedback;
    }

    // Start is called before the first frame update
    private void Start()
    {
        var deck = testUserField.Users.DeckList[0];

        var gameDeck = new GameDeck(deck);
        _player = new Player(gameDeck);
    }

    private void PlayerDamageFeedback()
    {
        GlobalAudioManager.Instance.Play(_playerDamageSound);
    }

    private void PlayerDamageShieldedFeedback()
    {
        GlobalAudioManager.Instance.Play(_playerDamageShieldedSound);
    }
    
    private void PlayerHealedFeedback()
    {
        Debug.Log("Player Healed!");
        GlobalAudioManager.Instance.Play(_playerhealedSound);
    }
    
    private void PlayerShieldUpFeedback()
    {
        Debug.Log("Player Shield!");
        GlobalAudioManager.Instance.Play(_playerShieldUpSound);
    }

    [ContextMenu("play Card 0")]
    public void PlayCardZero()
    {
        _player.PlayHandCard(0);
    }

    [ContextMenu("play Card 1")]
    public void PlayCardOne()
    {
        _player.PlayHandCard(1);
    }

    [ContextMenu("play Card 2")]
    public void PlayCardTwo()
    {
        _player.PlayHandCard(2);
    }

    [ContextMenu("play Card 3")]
    public void PlayCardThree()
    {
        _player.PlayHandCard(3);
    }

    [ContextMenu("play Card 4")]
    public void PlayCardfour()
    {
        _player.PlayHandCard(4);
    }

    [ContextMenu("start turn")]
    public void DrawCard()
    {
        _player.StartTurn();
    }
}