using System;
using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    private bool _playerTurn;
    private TurnStateEnum _turnState;

    public static event Action<bool> togglePlayerUiControls;
    public static event Action<string> updateDebugText;

    private void OnEnable() {
        UIDataContainer.Instance.Enemy.TurnEnded += EndEnemyTurn;
        //Enemy.enemyTurnDone += EndEnemyTurn;
    }

    private void Start() {
        _turnState = TurnStateEnum.Start;
        StartCoroutine(InitCombat());
    }

    private void OnDisable() {
        UIDataContainer.Instance.Enemy.TurnEnded -= EndEnemyTurn;
        //Enemy.enemyTurnDone -= EndEnemyTurn;
    }

    private IEnumerator InitCombat() {
        updateDebugText?.Invoke("Combat Initiated!");
        yield return new WaitForSeconds(1f);

        _turnState = TurnStateEnum.PlayerTurn;
        _playerTurn = true;

        yield return StartCoroutine(PlayerTurn());
    }

    private IEnumerator PlayerTurn() {
        togglePlayerUiControls?.Invoke(true);
        updateDebugText?.Invoke("Player Turn!");
        yield return new WaitForSeconds(1f);

        UIDataContainer.Instance.Player.StartTurn();

        //GlobalGameInfos.Instance.PlayerObject.Player.StartTurn();
    }

    private IEnumerator EnemyTurn() {
        togglePlayerUiControls?.Invoke(false);
        updateDebugText?.Invoke("Enemy Turn!");
        yield return new WaitForSeconds(1f);

        UIDataContainer.Instance.Enemy.StartTurn();

        //GlobalGameInfos.Instance.EnemyObject.enemy.EnemyTurn();

        yield return StartCoroutine(PlayerTurn());
    }

    private void EndEnemyTurn() {
        updateDebugText?.Invoke("Enemy Turn Ended!");
        _playerTurn = true;

        _turnState = TurnStateEnum.PlayerTurn;
    }

    public void EndPlayerTurn() {
        updateDebugText?.Invoke("Player Turn Ended!");
        // called by button in scene
        _playerTurn = false;
        _turnState = TurnStateEnum.EnemyTurn;

        StartCoroutine(EnemyTurn());
    }
}