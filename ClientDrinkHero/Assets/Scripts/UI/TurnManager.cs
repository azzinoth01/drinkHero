using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    private bool _playerTurn;
    private TurnStateEnum _turnState;
    [SerializeField] private Button endTurnButton;

    public static event Action<bool> togglePlayerUiControls;
    public static event Action<string> updateDebugText;

    private void OnEnable()
    {
        UIDataContainer.Instance.Enemy.TurnEnded += EndEnemyTurn;
        UIDataContainer.Instance.Player.TurnEnded += EndPlayerTurn;
        //Enemy.enemyTurnDone += EndEnemyTurn;
    }

    private void Start()
    {
        _turnState = TurnStateEnum.Start;
        StartCoroutine(InitCombat());
        
        endTurnButton.onClick.AddListener(ViewTweener.ButtonClickTween(endTurnButton, 
            endTurnButton.image.sprite, () => EndPlayerTurn()));
    }

    private void OnDisable()
    {
        UIDataContainer.Instance.Enemy.TurnEnded -= EndEnemyTurn;
        UIDataContainer.Instance.Player.TurnEnded -= EndPlayerTurn;
        //Enemy.enemyTurnDone -= EndEnemyTurn;
    }

    private IEnumerator InitCombat()
    {
        updateDebugText?.Invoke("FIGHT!");
        yield return new WaitForSeconds(2f);

        _turnState = TurnStateEnum.PlayerTurn;
        _playerTurn = true;

        yield return StartCoroutine(PlayerTurn());
    }

    private IEnumerator PlayerTurn()
    {
        togglePlayerUiControls?.Invoke(true);
        updateDebugText?.Invoke("PLAYER TURN!");
        yield return new WaitForSeconds(2f);

        UIDataContainer.Instance.Player.StartTurn();

        //GlobalGameInfos.Instance.PlayerObject.Player.StartTurn();
    }

    private IEnumerator EnemyTurn()
    {
        togglePlayerUiControls?.Invoke(false);
        updateDebugText?.Invoke("ENEMY TURN!");
        yield return new WaitForSeconds(2f);

        UIDataContainer.Instance.Enemy.StartTurn();

        //GlobalGameInfos.Instance.EnemyObject.enemy.EnemyTurn();

        yield return StartCoroutine(PlayerTurn());
    }

    private void EndEnemyTurn()
    {
        updateDebugText?.Invoke("Enemy Turn Ended!");
        _playerTurn = true;

        _turnState = TurnStateEnum.PlayerTurn;
    }

    public void EndPlayerTurn()
    {
        updateDebugText?.Invoke("Player Turn Ended!");
        // called by button in scene

        UIDataContainer.Instance.Player.EndTurn();
        _playerTurn = false;
        _turnState = TurnStateEnum.EnemyTurn;

        StartCoroutine(EnemyTurn());
    }
}