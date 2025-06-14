using UnityEngine;

public static class StaticEntreprisesSaveManager
{
    public const string ENEMIES_DEFEATED = "EnemiesDefeated";
    public const string ENEMIES_LOST_TO = "EnemiesLostTo";

    public static EnumGameStatus GetEnemyStatus(string enemyName)
    {
        string enemiesBattled = PlayerPrefs.GetString(ENEMIES_DEFEATED, "");
        string enemiesLostTo = PlayerPrefs.GetString(ENEMIES_LOST_TO, "");

        if (enemiesBattled.Contains(enemyName))
        {
            return EnumGameStatus.Won;
        }
        else if (enemiesLostTo.Contains(enemyName))
        {
            return EnumGameStatus.Lost;
        }
        else
        {
            return EnumGameStatus.None;
        }
    }

    public static string EnemyStatusToSaveKey(EnumGameStatus gameStatus)
    {
        switch (gameStatus)
        {
            case EnumGameStatus.Won:
                return ENEMIES_DEFEATED;
            case EnumGameStatus.Lost:
                return ENEMIES_LOST_TO;
            default:
                return "";
        }
    }

    public static int CountEnemiesByStatus(EnumGameStatus gameStatus)
    {
        string enemiesBattledKey = EnemyStatusToSaveKey(gameStatus);
        string enemiesBattled = PlayerPrefs.GetString(enemiesBattledKey, "");

        if (string.IsNullOrEmpty(enemiesBattled))
        {
            return 0;
        }

        return enemiesBattled.Split('|').Length;
    }
    public static string[] GetEnemiesByStatus(EnumGameStatus gameStatus)
    {
        string enemiesBattledKey = EnemyStatusToSaveKey(gameStatus);
        string enemiesBattled = PlayerPrefs.GetString(enemiesBattledKey, "");

        if (string.IsNullOrEmpty(enemiesBattled))
        {
            return new string[0];
        }

        return enemiesBattled.Split('|');
    }

    public static void InitSave(string defaultEnemyName)
    {
        if (GetEnemyStatus(defaultEnemyName) == EnumGameStatus.None)
        {
            Save(EnumGameStatus.Won, defaultEnemyName);
        }
    }
    public static void Save(EnumGameStatus gameStatus, string enemyName)
    {
        if (GetEnemyStatus(enemyName) != EnumGameStatus.None)
        {
            Debug.LogWarning($"Enemy '{enemyName}' already recorded with status: {GetEnemyStatus(enemyName)}");
            return;
        }

        string enemiesBattledKey = EnemyStatusToSaveKey(gameStatus);

        string enemiesBattled = PlayerPrefs.GetString(enemiesBattledKey, "");
        string separator = string.IsNullOrEmpty(enemiesBattled) ? "" : "|";
        PlayerPrefs.SetString(enemiesBattledKey, enemiesBattled + separator + enemyName);
        PlayerPrefs.Save();
    }
}
