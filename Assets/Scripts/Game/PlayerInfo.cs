using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int MaxWattction = 2;
    public int CurrentWattction = 0;

    public Inventory inventory;
    public TextMeshProUGUI wattctionText;

    public void Init()
    {
        CurrentWattction = MaxWattction;

        UpdateUI();
    }

    public void AddWattction(int amount)
    {
        CurrentWattction += amount;
        if (CurrentWattction > MaxWattction)
        {
            CurrentWattction = MaxWattction;
        }
        else if (CurrentWattction < 0)
        {
            CurrentWattction = 0;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        wattctionText.text = CurrentWattction.ToString() + "/" + MaxWattction.ToString();
    }
}
