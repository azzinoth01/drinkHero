using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private PlayerObject _playerObject;
    private Player _player;
    
    private EnemyObject _enemyObject;
    private Enemy _enemy;

    private bool _playerTurn;
    
    void Start()
    {
        
        _playerTurn = true;
    }
    
    private void HandleTurn()
    {
        // determine whose turn it is
        // if playerTurn, PlayerTurn(), else EnemyTurn()
    }

    private void PlayerTurn()
    {
        _player.StartTurn();
    }

    private void EnemyTurn()
    {
        // enemy attack
        // _playerTurn = true;  
    }
    
    private void EndTurn()
    {
        // called by button in scene
    }
}