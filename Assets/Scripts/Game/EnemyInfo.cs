using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EnemyInfo : MonoBehaviour
{
    public int level = 1;
    public string enemyName;
    public int turnToAttack = 2;

    public List<EnumCardType.CardType> weaknesses = new List<EnumCardType.CardType>();
    public List<EnumCardType.CardType> resistances = new List<EnumCardType.CardType>();
    public TextMeshProUGUI enemyNameText;
    public TextMeshProUGUI EOGEnemyNameText;
    public void Init()
    {
        level = StaticEnemyInfo.level;
        enemyName = StaticEnemyInfo.name;

        turnToAttack = level + 1;

        Tuple<List<EnumCardType.CardType>, List<EnumCardType.CardType>> types = StaticEnemyInfo.GetSaveEnemyInfo(enemyName);
        if (types == null)
        {
            if (level == 1)
            {
                types = StaticEnemyInfo.GetRandomTypeList(1, 1);
            }
            else if (level == 2)
            {
                types = StaticEnemyInfo.GetRandomTypeList(2, 2);
            }
            else
            {
                types = StaticEnemyInfo.GetRandomTypeList(1, 2);
            }
        }

        weaknesses = types.Item1;
        resistances = types.Item2;

        UpdateUI();
    }

    private int CalcDamage(int damage, EnumCardType.CardType cardType)
    {
        if (cardType == EnumCardType.CardType.None)
        {
            return 0;
        }

        int mult = 1;

        if (weaknesses.Contains(cardType))
        {
            mult = 2;
            StaticGameActionLogs.AddPlayingCardLog(damage * mult, cardType, StaticGameActionLogs.EnumCardEffect.Weak);
        }
        else if (resistances.Contains(cardType))
        {
            mult = -1;
            StaticGameActionLogs.AddPlayingCardLog(damage * mult, cardType, StaticGameActionLogs.EnumCardEffect.Resistant);
        }
        else
        {
            StaticGameActionLogs.AddPlayingCardLog(damage * mult, cardType, StaticGameActionLogs.EnumCardEffect.Neutral);
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
        int normalDamage = 10;
        int strongDamage = 35;

        if (level == 2)
        {
            normalDamage = 20;
            strongDamage = 45;
        }
        else if (level > 2)
        {
            normalDamage = 35;
            strongDamage = 60;
        }

        int randomValue = UnityEngine.Random.Range(0, 100);
        if (randomValue < 20)
        {
            return strongDamage;
        }

        return normalDamage;
    }

    private void UpdateUI()
    {
        enemyNameText.text = enemyName;
        EOGEnemyNameText.text = enemyName;
    }

    public int GetExpReward()
    {
        if (level == 1)
        {
            return 10;
        }
        else if (level == 2)
        {
            return 50;
        }
        else
        {
            return 200;
        }
    }

    public void Save(EnumGameStatus gameStatus)
    {
        StaticEntreprisesSaveManager.Save(gameStatus, enemyName);
    }
}
