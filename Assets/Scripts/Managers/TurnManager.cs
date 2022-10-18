using System;
using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    //[SerializeField] private PlayerObject _playerObject;
    //private Player _player;

    //[SerializeField] private EnemyObject _enemyObject;
    //private Enemy _enemy;

    private bool _playerTurn;

    public enum TurnState {
        Start, PlayerTurn, EnemyTurn, Win, Lost
    }

    private TurnState _turnState;




    public static event Action<bool> togglePlayerUiControls;
    public static event Action<string> updateDebugText;




    private void OnEnable() {
        Enemy.enemyTurnDone += EndEnemyTurn;

        //_player = _playerObject.PlayerReference;
        //_enemy = _enemyObject.enemy;


    }

    private void Start() {
        _turnState = TurnState.Start;
        StartCoroutine(InitCombat());
    }

    private void OnDisable() {
        Enemy.enemyTurnDone -= EndEnemyTurn;
    }

    private void HandleTurn() {
        switch (_turnState) {
            case TurnState.Start:
                break;
            case TurnState.PlayerTurn:
                break;
            case TurnState.EnemyTurn:
                break;
            case TurnState.Lost:
                break;
            case TurnState.Win:
                break;
        }
    }

    private IEnumerator InitCombat() {
        updateDebugText?.Invoke("Combat Initiated!");
        yield return new WaitForSeconds(1f);

        _turnState = TurnState.PlayerTurn;
        _playerTurn = true;

        yield return StartCoroutine(PlayerTurn());
    }

    private IEnumerator PlayerTurn() {
        togglePlayerUiControls?.Invoke(true);
        updateDebugText?.Invoke("Player Turn!");
        yield return new WaitForSeconds(1f);

        GlobalGameInfos.Instance.PlayerObject.Player.StartTurn();

        //_player.StartTurn();
    }

    private IEnumerator EnemyTurn() {
        togglePlayerUiControls?.Invoke(false);
        updateDebugText?.Invoke("Enemy Turn!");
        yield return new WaitForSeconds(1f);

        GlobalGameInfos.Instance.EnemyObject.enemy.EnemyTurn();

        yield return StartCoroutine(PlayerTurn());
    }

    private void EndEnemyTurn() {
        updateDebugText?.Invoke("Enemy Turn Ended!");
        _playerTurn = true;

        _turnState = TurnState.PlayerTurn;
    }

    public void EndPlayerTurn() {
        updateDebugText?.Invoke("Player Turn Ended!");
        // called by button in scene
        _playerTurn = false;
        _turnState = TurnState.EnemyTurn;

        StartCoroutine(EnemyTurn());
    }
}