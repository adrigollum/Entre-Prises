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
        health = maxHealth / 2;
        UpdateUI();
    }

    public void AddStat(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateUI();
    }

    private void UpdateUI()
    {
        healthBar.value = (float)health / maxHealth;
        playerStatsText.text = $"{health * 100 / maxHealth}";
        enemyStatsText.text = $"{(maxHealth - health) * 100 / maxHealth}";
    }
}
