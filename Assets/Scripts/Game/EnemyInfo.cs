using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyInfo : MonoBehaviour
{
    public string enemyName;
    public int turnToAttack = 2;
    public List<EnumCardType.CardType> weaknesses = new List<EnumCardType.CardType>();
    public List<EnumCardType.CardType> resistances = new List<EnumCardType.CardType>();

    public TextMeshProUGUI enemyNameText;
    public void Init()
    {
        UpdateUI();
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
    public int GetDamage(CardInfo cardInfo)
    {
        if (cardInfo == null)
        {
            return 0;
        }

        int damage = CalcDamage(cardInfo.cardStat, cardInfo.cardFirstType);
        damage += CalcDamage(cardInfo.cardStat, cardInfo.cardSecondType);

        return damage;
    }

    private void UpdateUI()
    {
        enemyNameText.text = enemyName;
    }
}
