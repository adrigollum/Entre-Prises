using TMPro;
using UnityEngine;

public class GameTurn : MonoBehaviour
{
    private PlayerTurn playerTurn;
    private EnemyTurn enemyTurn;
    private GameInfo gameInfo;
    public int turnCount = 0;
    public bool hadCardPlayed = false;

    [Header("UI Elements")]
    public GameObject GameCanvas;
    public GameObject EndOfGameCanvas;
    public TextMeshProUGUI EndOfGameText;
    public TextMeshProUGUI turnsText;

    public void Start()
    {
        turnCount = 0;

        playerTurn = GetComponent<PlayerTurn>();
        enemyTurn = GetComponent<EnemyTurn>();
        gameInfo = GetComponent<GameInfo>();

        enemyTurn.Init();
        playerTurn.Init();
        // Depends on Enemy level
        gameInfo.Init();

        GameCanvas.SetActive(true);
        EndOfGameCanvas.SetActive(false);
        EndOfGameText.text = string.Empty;

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

        TriggerEndOfGame();
        hadCardPlayed = cardPlayed || hadCardPlayed; // Track if any card was played this turn
        return cardPlayed;
    }
    public void RepositionAllCards()
    {
        playerTurn.RepositionAllCards();
    }
    public void TriggerEndOfGame()
    {
        EnumGameStatus gameStatus = gameInfo.GetGameStatus();
        if (gameStatus == EnumGameStatus.Playing)
        {
            return;
        }

        if (gameStatus == EnumGameStatus.Lost)
        {
            EndOfGameText.text = "You lost! Better luck next time!";
        }
        else if (gameStatus == EnumGameStatus.Won)
        {
            EndOfGameText.text = "You won! Congratulations!";
        }

        playerTurn.playerInfo.exp += enemyTurn.enemyInfo.GetExpReward();
        playerTurn.Save();
        enemyTurn.Save(gameStatus);

        EndOfGameCanvas.SetActive(true);
        GameCanvas.SetActive(false);
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

    private int CalcWattctionGain()
    {
        int wattctionGain = 2 + (playerTurn.playerInfo.level - 1) / 2;

        if (!hadCardPlayed)
        {
            wattctionGain += playerTurn.playerInfo.level;
        }
        return wattctionGain;
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

        int wattctionGain = CalcWattctionGain();

        playerTurn.playerInfo.AddWattction(wattctionGain);

        hadCardPlayed = false;
        UpdateUI();

        TriggerEndOfGame();
    }

    public void UpdateUI()
    {
        turnsText.text = (enemyTurn.enemyInfo.turnToAttack - (turnCount % enemyTurn.enemyInfo.turnToAttack)).ToString();
    }
}
