using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyInfo : MonoBehaviour
{
    public string enemyName;
    public int maxHealth = 100;
    private int health;
    public int attackPower;
    public List<EnumCardType.CardType> weaknesses = new List<EnumCardType.CardType>();
    public List<EnumCardType.CardType> resistances = new List<EnumCardType.CardType>();

    public TextMeshProUGUI enemyNameText;
    public TextMeshProUGUI healthText;

    private void Start()
    {
        health = maxHealth;
        UpdateTexts();
    }

    private int CalcDamage(int damage, EnumCardType.CardType cardType)
    {
        int mult = 1;

        // Check if the card type is a weakness
        if (weaknesses.Contains(cardType))
        {
            mult = 2;
        }
        else if (resistances.Contains(cardType))
        {
            mult = -1;
        }

        // Calculate final damage
        return damage * mult;
    }
    public void TakeDamage(CardInfo cardInfo)
    {
        if (cardInfo != null)
        {
            Debug.Log($"Enemy {enemyName} takes damage from card: {cardInfo.cardName}");

            int damage = CalcDamage(cardInfo.cardStat, cardInfo.cardFirstType);
            damage += CalcDamage(cardInfo.cardStat, cardInfo.cardSecondType);

            AddLife(-damage);
        }
        else
        {
            Debug.LogError("CardInfo component not found on the card.");
        }
        UpdateTexts();
    }

    public void AddLife(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health < 0)
        {
            health = 0;
        }

        UpdateTexts();
    }

    private void UpdateTexts()
    {
        enemyNameText.text = enemyName;

        healthText.text = $"{health}/{maxHealth} %";
    }
}
