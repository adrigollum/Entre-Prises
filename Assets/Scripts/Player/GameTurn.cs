using System.Collections.Generic;
using UnityEngine;

public class GameTurn : MonoBehaviour
{
    public PlayerTurn playerTurn;
    public EnemyTurn enemyTurn;

    public int turnCount = 0;

    public void Start()
    {
        playerTurn = GetComponent<PlayerTurn>();
        enemyTurn = GetComponent<EnemyTurn>();

        playerTurn.DrawCards();
    }
}
