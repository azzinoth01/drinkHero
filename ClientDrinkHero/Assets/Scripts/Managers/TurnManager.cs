using System;
using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private bool _playerTurn;
    private TurnState _turnState;

    public static event Action<bool> togglePlayerUiControls;
    public static event Action<string> updateDebugText;

    private void OnEnable()
    {
        Enemy.enemyTurnDone += EndEnemyTurn;
    }

    private void Start()
    {
        _turnState = TurnState.Start;
        StartCoroutine(InitCombat());
    }

    private void OnDisable()
    {
        Enemy.enemyTurnDone -= EndEnemyTurn;
    }

    private IEnumerator InitCombat()
    {
        updateDebugText?.Invoke("Combat Initiated!");
        yield return new WaitForSeconds(1f);

        _turnState = TurnState.PlayerTurn;
        _playerTurn = true;

        yield return StartCoroutine(PlayerTurn());
    }

    private IEnumerator PlayerTurn()
    {
        togglePlayerUiControls?.Invoke(true);
        updateDebugText?.Invoke("Player Turn!");
        yield return new WaitForSeconds(1f);
        
        GlobalGameInfos.Instance.PlayerObject.Player.StartTurn();
    }

    private IEnumerator EnemyTurn()
    {
        togglePlayerUiControls?.Invoke(false);
        updateDebugText?.Invoke("Enemy Turn!");
        yield return new WaitForSeconds(1f);

        GlobalGameInfos.Instance.EnemyObject.enemy.EnemyTurn();

        yield return StartCoroutine(PlayerTurn());
    }

    private void EndEnemyTurn()
    {
        updateDebugText?.Invoke("Enemy Turn Ended!");
        _playerTurn = true;

        _turnState = TurnState.PlayerTurn;
    }

    public void EndPlayerTurn()
    {
        updateDebugText?.Invoke("Player Turn Ended!");
        // called by button in scene
        _playerTurn = false;
        _turnState = TurnState.EnemyTurn;

        StartCoroutine(EnemyTurn());
    }
}