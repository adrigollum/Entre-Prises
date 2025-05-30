using TMPro;
using UnityEngine;

public class GameTurn : MonoBehaviour
{
    private PlayerTurn playerTurn;
    private EnemyTurn enemyTurn;
    private GameInfo gameInfo;
    public TextMeshProUGUI turnsText;

    public int turnCount = 0;

    public void Start()
    {
        turnCount = 0;

        playerTurn = GetComponent<PlayerTurn>();
        enemyTurn = GetComponent<EnemyTurn>();
        gameInfo = GetComponent<GameInfo>();

        gameInfo.Init();
        playerTurn.Init();
        enemyTurn.Init();

        UpdateUI();
    }

    public bool PlayCard(GameObject card)
    {
        if (card == null)
        {
            Debug.LogError("Card is null. Cannot play card.");
            return false;
        }

        bool cardPlayed = playerTurn.PlayCard(card);
        if (cardPlayed)
        {
            int damage = enemyTurn.GetDamage(card.GetComponent<CardInfo>());
            gameInfo.AddStat(damage);
        }

        return cardPlayed;
    }
    public void RepositionAllCards()
    {
        playerTurn.RepositionAllCards();
    }

    public void UpdateUI()
    {
        turnsText.text = $"{turnCount % enemyTurn.enemyInfo.turnToAttack}/{enemyTurn.enemyInfo.turnToAttack}";
    }
}
