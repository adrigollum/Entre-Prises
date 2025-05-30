using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class EnemyInfo : MonoBehaviour
{
    public int level = 1;
    public string enemyName;
    public int turnToAttack = 2;


    public List<EnumCardType.CardType> weaknesses = new List<EnumCardType.CardType>();
    public List<EnumCardType.CardType> resistances = new List<EnumCardType.CardType>();
    public TextMeshProUGUI enemyNameText;
    public void Init()
    {
        level = PlayerPrefs.GetInt("EnemyLevel", 1);
        enemyName = PlayerPrefs.GetString("EnemyName", "Unknown Enemy");

        turnToAttack = level + 1;
        weaknesses.Clear();
        resistances.Clear();

        if (level == 1)
        {
            weaknesses = GetRandomTypeList(1);
            resistances = GetRandomTypeList(1);
        }
        else if (level == 2)
        {
            weaknesses = GetRandomTypeList(2);
            resistances = GetRandomTypeList(2);
        }
        else
        {
            weaknesses = GetRandomTypeList(1);
            resistances = GetRandomTypeList(2);
        }

        UpdateUI();
    }

    private List<EnumCardType.CardType> GetRandomTypeList(int size)
    {
        List<EnumCardType.CardType> allTypes = new List<EnumCardType.CardType>();

        foreach (EnumCardType.CardType type in System.Enum.GetValues(typeof(EnumCardType.CardType)))
        {
            if (type == EnumCardType.CardType.None)
            {
                continue;
            }
            allTypes.Add(type);
        }
        while (allTypes.Count > size)
        {
            int index = Random.Range(0, allTypes.Count);
            allTypes.RemoveAt(index);
        }

        return allTypes;
    }

    private int CalcDamage(int damage, EnumCardType.CardType cardType)
    {
        int mult = 1;

        if (weaknesses.Contains(cardType))
        {
            mult = 2;
        }
        else if (resistances.Contains(cardType))
        {
            mult = -1;
        }

        return damage * mult;
    }
    public int GetDamageFromCard(CardInfo cardInfo)
    {
        if (cardInfo == null)
        {
            return 0;
        }

        int damage = CalcDamage(cardInfo.cardStat, cardInfo.cardFirstType);
        damage += CalcDamage(cardInfo.cardStat, cardInfo.cardSecondType);

        return damage;
    }

    public int GetAttackDamage()
    {
        return 0;
    }

    private void UpdateUI()
    {
        enemyNameText.text = enemyName;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("EnemyLevel", level);
        PlayerPrefs.SetString("EnemyName", enemyName);
        PlayerPrefs.Save();
    }
}
