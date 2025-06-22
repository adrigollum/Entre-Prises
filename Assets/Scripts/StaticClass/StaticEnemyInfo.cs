
using System;
using System.Collections.Generic;
using UnityEngine;

public static class StaticEnemyInfo
{
    public static string name = "Unknown Enemy";
    public static int level = 1;

    private static bool isRandomInitialized = false;

    public static Tuple<List<EnumCardType.CardType>, List<EnumCardType.CardType>> GetRandomTypeList(int weaknessesSize, int resistancesSize)
    {
        if (isRandomInitialized == false)
        {
            isRandomInitialized = true;
            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        }

        List<EnumCardType.CardType> allTypes = new List<EnumCardType.CardType>();

        foreach (EnumCardType.CardType type in Enum.GetValues(typeof(EnumCardType.CardType)))
        {
            if (type == EnumCardType.CardType.None)
            {
                continue;
            }
            allTypes.Add(type);
        }

        if (allTypes.Count < weaknessesSize + resistancesSize)
        {
            Debug.LogError("Not enough types to assign weaknesses and resistances.");
            return null;
        }

        List<EnumCardType.CardType> selectedWeaknesses = new List<EnumCardType.CardType>();
        List<EnumCardType.CardType> selectedResistances = new List<EnumCardType.CardType>();
        for (int i = 0; i < weaknessesSize; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, allTypes.Count);
            selectedWeaknesses.Add(allTypes[randomIndex]);
            allTypes.RemoveAt(randomIndex);
        }
        for (int i = 0; i < resistancesSize; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, allTypes.Count);
            selectedResistances.Add(allTypes[randomIndex]);
            allTypes.RemoveAt(randomIndex);
        }

        return new Tuple<List<EnumCardType.CardType>, List<EnumCardType.CardType>>(selectedWeaknesses, selectedResistances);
    }

    public static void SaveEnemyInfo(string enemyName, List<EnumCardType.CardType> enemyWeaknesses, List<EnumCardType.CardType> enemyResistances)
    {
        if (PlayerPrefs.HasKey($"{enemyName}EnemyWeaknesses") || PlayerPrefs.HasKey($"{enemyName}EnemyResistances"))
        {
            Debug.LogWarning($"Enemy '{enemyName}' already saved. Overwriting existing data.");
            return;
        }

        List<string> weaknessesStrings = new List<string>();
        List<string> resistancesStrings = new List<string>();
        foreach (EnumCardType.CardType weakness in enemyWeaknesses)
        {
            weaknessesStrings.Add(EnumCardType.TypeToString(weakness));
        }
        foreach (EnumCardType.CardType resistance in enemyResistances)
        {
            resistancesStrings.Add(EnumCardType.TypeToString(resistance));
        }
        PlayerPrefs.SetString($"{enemyName}EnemyWeaknesses", string.Join("|", weaknessesStrings));
        PlayerPrefs.SetString($"{enemyName}EnemyResistances", string.Join("|", resistancesStrings));
        PlayerPrefs.Save();
    }

    public static Tuple<List<EnumCardType.CardType>, List<EnumCardType.CardType>> GetSaveEnemyInfo(string enemyName)
    {
        if (!PlayerPrefs.HasKey($"{enemyName}EnemyWeaknesses") || !PlayerPrefs.HasKey($"{enemyName}EnemyResistances"))
        {
            Debug.LogError($"Enemy '{enemyName}' not found in saved data.");
            return null;
        }

        string weaknessesString = PlayerPrefs.GetString($"{enemyName}EnemyWeaknesses", "");
        string resistancesString = PlayerPrefs.GetString($"{enemyName}EnemyResistances", "");

        string[] weaknessesArray = weaknessesString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        string[] resistancesArray = resistancesString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        List<EnumCardType.CardType> weaknesses = new List<EnumCardType.CardType>();
        List<EnumCardType.CardType> resistances = new List<EnumCardType.CardType>();

        foreach (string weakness in weaknessesArray)
        {
            weaknesses.Add(EnumCardType.StringToType(weakness));
        }
        foreach (string resistance in resistancesArray)
        {
            resistances.Add(EnumCardType.StringToType(resistance));
        }

        return new Tuple<List<EnumCardType.CardType>, List<EnumCardType.CardType>>(weaknesses, resistances);
    }
}
