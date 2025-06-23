using TMPro;
using UnityEngine;

public class GameTurn : MonoBehaviour
{
    private PlayerTurn playerTurn;
    private EnemyTurn enemyTurn;
    private GameInfo gameInfo;
    public int turnCount = 0;
    public bool hadCardPlayed = false;
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;
    private float musicVolume;

    [Header("UI Elements")]
    public GameObject GameCanvas;
    public GameObject EndOfGameCanvas;
    public TextMeshProUGUI EndOfGameText;
    public TextMeshProUGUI turnsText;
    public TextMeshProUGUI GameActionLogsText;

    [Header("Log Colors")]
    public Color NeutralColor = Color.blue;
    public Color WeakColor = Color.green;
    public Color ResistantColor = Color.red;
    public Color StartTurnColor = Color.yellow;
    public Color DiscardColor = Color.lightGreen;
    public Color SkipTurnColor = Color.gray;
    public Color EnemyAttackColor = Color.magenta;


    public void Start()
    {
        turnCount = 0;

        playerTurn = GetComponent<PlayerTurn>();
        enemyTurn = GetComponent<EnemyTurn>();
        gameInfo = GetComponent<GameInfo>();

        enemyTurn.Init();
        playerTurn.Init();
        gameInfo.Init(); // Depends on Enemy level

        GameCanvas.SetActive(true);
        EndOfGameCanvas.SetActive(false);
        EndOfGameText.text = string.Empty;

        StaticGameActionLogs.NeutralColor = NeutralColor;
        StaticGameActionLogs.WeakColor = WeakColor;
        StaticGameActionLogs.ResistantColor = ResistantColor;
        StaticGameActionLogs.StartTurnColor = StartTurnColor;
        StaticGameActionLogs.DiscardColor = DiscardColor;
        StaticGameActionLogs.SkipTurnColor = SkipTurnColor;
        StaticGameActionLogs.EnemyAttackColor = EnemyAttackColor;
        StaticGameActionLogs.ClearLogs();

        UpdateUI();
    }

    public bool PlayCard(GameObject card)
    {
        if (card == null || gameInfo.GetGameStatus() != EnumGameStatus.Playing)
        {
            Debug.LogWarning("Card is null or game is not in playing status. Cannot play card.");
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
        UpdateUI();
        return cardPlayed;
    }
    public void RepositionAllCards()
    {
        playerTurn.RepositionAllCards();
    }

    private void ResetMusicVolume()
    {
        musicAudioSource.volume = musicVolume;
    }

    public void TriggerEndOfGame()
    {
        EnumGameStatus gameStatus = gameInfo.GetGameStatus();
        if (gameStatus == EnumGameStatus.Playing || gameStatus == EnumGameStatus.None)
        {
            return;
        }

        musicVolume = musicAudioSource.volume;
        if (gameStatus == EnumGameStatus.Lost)
        {
            EndOfGameText.text = "Vous avez perdu !";
            sfxAudioSource.PlayOneShot(loseClip);
            musicAudioSource.volume *= 0.5f;
        }
        else if (gameStatus == EnumGameStatus.Won)
        {
            EndOfGameText.text = "Vous avez gagné !";
            playerTurn.playerInfo.exp += enemyTurn.enemyInfo.GetExpReward();
            sfxAudioSource.PlayOneShot(winClip);
            musicAudioSource.volume *= 0.5f;
        }
        Invoke("ResetMusicVolume", sfxAudioSource.clip.length);

        playerTurn.Save();
        enemyTurn.Save(gameStatus);

        EndOfGameCanvas.SetActive(true);
        GameCanvas.SetActive(false);
        gameInfo.hasEnded = true;
    }

    public void DiscardCard(GameObject card)
    {
        if (card == null || gameInfo.GetGameStatus() != EnumGameStatus.Playing)
        {
            Debug.LogWarning("Card is null or game is not in playing status. Cannot discard card.");
            return;
        }

        StaticGameActionLogs.AddDiscardCardLog(playerTurn.DiscardCard(card));
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
    public void EndTurn(bool isSkip = false)
    {
        if (gameInfo.GetGameStatus() != EnumGameStatus.Playing)
        {
            Debug.LogWarning("Cannot end turn, game is not in playing status.");
            return;
        }

        turnCount++;

        if (turnCount % enemyTurn.enemyInfo.turnToAttack == 0)
        {
            int damage = enemyTurn.GetAttackDamage();
            gameInfo.AddStat(-damage);
            StaticGameActionLogs.AddEnemyAttackLog(damage);
        }

        playerTurn.DrawCards();

        int wattctionGain = CalcWattctionGain();
        if (isSkip)
        {
            StaticGameActionLogs.AddSkipTurnLog(wattctionGain);
        }
        else
        {
            StaticGameActionLogs.AddTurnStartLog(wattctionGain);
        }
        playerTurn.playerInfo.AddWattction(wattctionGain);

        hadCardPlayed = false;
        UpdateUI();

        TriggerEndOfGame();
    }

    public void UpdateUI()
    {
        turnsText.text = (enemyTurn.enemyInfo.turnToAttack - (turnCount % enemyTurn.enemyInfo.turnToAttack)).ToString();

        GameActionLogsText.text = StaticGameActionLogs.GetLogs();
    }
}
