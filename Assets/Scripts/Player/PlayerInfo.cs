using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int MaxWattction = 2;
    public int CurrentWattction = 0;

    public int MaxLife = 10;
    public int CurrentLife = 0;

    public Inventory inventory;

    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI wattctionText;

    private void Start()
    {
        CurrentWattction = MaxWattction;
        CurrentLife = MaxLife;

        UpdateTexts();
    }

    public void PlayCard(GameObject card)
    {
        CardInfo cardInfo = card.GetComponent<CardInfo>();
        if (cardInfo != null)
        {
            if (CurrentWattction >= cardInfo.cardWattctionCost)
            {
                AddWattction(-cardInfo.cardWattctionCost);

                inventory.RemoveCard(card);
            }
            else
            {
                Debug.Log("Not enough Wattction to play this card.");
            }
        }
        else
        {
            Debug.LogError("CardInfo component not found on the card.");
        }
    }

    public void AddLife(int amount)
    {
        CurrentLife += amount;
        if (CurrentLife > MaxLife)
        {
            CurrentLife = MaxLife;
        }
        else if (CurrentLife < 0)
        {
            CurrentLife = 0;
        }

        UpdateTexts();
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

        UpdateTexts();
    }

    private void UpdateTexts()
    {
        lifeText.text = CurrentLife.ToString() + "/" + MaxLife.ToString();

        wattctionText.text = CurrentWattction.ToString() + "/" + MaxWattction.ToString();
    }
}
