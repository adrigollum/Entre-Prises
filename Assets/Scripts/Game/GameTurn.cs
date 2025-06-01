using TMPro;
using UnityEngine;

public class GameTurn : MonoBehaviour
{
    private PlayerTurn playerTurn;
    private EnemyTurn enemyTurn;
    private GameInfo gameInfo;
    public TextMeshProUGUI turnsText;

    public int turnCount = 0;
    public bool hadCardPlayed = false;

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
            int damage = enemyTurn.GetDamageFromCard(card.GetComponent<CardInfo>());
            gameInfo.AddStat(damage);
        }

        hadCardPlayed = cardPlayed || hadCardPlayed; // Track if any card was played this turn
        return cardPlayed;
    }
    public void RepositionAllCards()
    {
        playerTurn.RepositionAllCards();
    }

    public void DiscardCard(GameObject card)
    {
        if (card == null)
        {
            Debug.LogError("Card is null. Cannot discard card.");
            return;
        }

        playerTurn.DiscardCard(card);
        EndTurn();
    }

    public void EndTurn()
    {
        turnCount++;

        if (turnCount % enemyTurn.enemyInfo.turnToAttack == 0)
        {
            int damage = enemyTurn.GetAttackDamage();
            gameInfo.AddStat(-damage);
        }

        playerTurn.DrawCards();

        int wattctionGain = playerTurn.playerInfo.level;
        if (!hadCardPlayed)
        {
            wattctionGain *= 2;
        }

        playerTurn.playerInfo.AddWattction(wattctionGain);

        hadCardPlayed = false;
        UpdateUI();
    }

    public void UpdateUI()
    {
        turnsText.text = "Tours avant attaque : " + (enemyTurn.enemyInfo.turnToAttack - (turnCount % enemyTurn.enemyInfo.turnToAttack)).ToString();
    }
}
