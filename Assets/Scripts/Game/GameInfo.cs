using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour
{
    public int maxHealth = 100;
    private int health = 0;
    public Slider healthBar;
    public TextMeshProUGUI playerStatsText;
    public TextMeshProUGUI enemyStatsText;
    public void Init()
    {
        EnemyInfo enemyInfo = GetComponent<EnemyInfo>();

        health = maxHealth / 2;
        if (enemyInfo.level == 1)
        {
            health = 60;
        }
        else if (enemyInfo.level == 2)
        {
            health = 40;
        }
        else if (enemyInfo.level == 3)
        {
            health = 30;
        }
        UpdateUI();
    }

    public void AddStat(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateUI();
    }

    public EnumGameStatus GetGameStatus()
    {
        if (health <= 0)
        {
            return EnumGameStatus.Lost;
        }

        if (health >= maxHealth)
        {
            return EnumGameStatus.Won;
        }

        return EnumGameStatus.Playing;
    }

    private void UpdateUI()
    {
        healthBar.value = (float)health / maxHealth;
        playerStatsText.text = $"{health * 100 / maxHealth}";
        enemyStatsText.text = $"{(maxHealth - health) * 100 / maxHealth}";
    }
}
