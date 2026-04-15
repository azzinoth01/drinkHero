using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;

    public static event Action<bool> togglePlayerUiControls;
    public static event Action<string> updateDebugText;

    private void OnEnable() {
        UIDataContainer.Instance.Enemy.TurnEnded += EndEnemyTurn;
        UIDataContainer.Instance.Player.TurnEnded += EndPlayerTurn;
        //Enemy.enemyTurnDone += EndEnemyTurn;
    }

    private void Start() {
        StartCoroutine(InitCombat());

        endTurnButton.onClick.AddListener(ViewTweener.ButtonClickTween(endTurnButton,endTurnButton.image.sprite,() => EndPlayerTurn()));
    }

    private void OnDisable() {
        UIDataContainer.Instance.Enemy.TurnEnded -= EndEnemyTurn;
        UIDataContainer.Instance.Player.TurnEnded -= EndPlayerTurn;
        //Enemy.enemyTurnDone -= EndEnemyTurn;
    }

    private IEnumerator InitCombat() {
        yield return new WaitForSeconds(0.2f);
        updateDebugText?.Invoke("FIGHT!");
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(PlayerTurn());
    }

    private IEnumerator PlayerTurn() {
        togglePlayerUiControls?.Invoke(true);
        updateDebugText?.Invoke("PLAYER TURN!");
        yield return new WaitForSeconds(2f);

        UIDataContainer.Instance.Player.StartTurn();

        //GlobalGameInfos.Instance.PlayerObject.Player.StartTurn();
    }

    private IEnumerator EnemyTurn() {
        togglePlayerUiControls?.Invoke(false);
        updateDebugText?.Invoke("ENEMY TURN!");
        yield return new WaitForSeconds(2f);

        UIDataContainer.Instance.Enemy.StartTurn();
        yield return StartCoroutine(PlayerTurn());
    }

    private void EndEnemyTurn() {
        updateDebugText?.Invoke("Enemy Turn Ended!");
        StartCoroutine(PlayerTurn());
    }

    public void EndPlayerTurn() {
        updateDebugText?.Invoke("Player Turn Ended!");
        UIDataContainer.Instance.Player.EndTurn();

        StartCoroutine(EnemyTurn());
    }
}