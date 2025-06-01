using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int level = 1;
    public int exp = 0;
    public int nbEntreprise = 1;
    public int maxWattction = 2;
    public int currentWattction = 0;
    public Inventory inventory;
    public TextMeshProUGUI wattctionText;

    public void Init()
    {
        level = PlayerPrefs.GetInt("PlayerLevel", 1);
        exp = PlayerPrefs.GetInt("PlayerExp", 0);
        nbEntreprise = PlayerPrefs.GetInt("PlayerNbEntreprise", 1);

        maxWattction = nbEntreprise * 2;
        currentWattction = 1;

        inventory.deckSize = 3 + (level - 1) / 2;
        UpdateUI();
    }
    public bool PlayCard(GameObject card)
    {
        CardInfo cardInfo = card.GetComponent<CardInfo>();
        if (cardInfo == null)
        {
            Debug.LogError("CardInfo component not found on the card.");
            return false;
        }

        if (currentWattction < cardInfo.cardWattctionCost)
        {
            Debug.Log("Not enough Wattction to play this card.");
            return false;
        }

        AddWattction(-cardInfo.cardWattctionCost);

        inventory.PutCardInPool(card);
        return true;
    }
    public void DiscardCard(GameObject card)
    {
        if (card == null)
        {
            Debug.LogError("Card is null. Cannot discard card.");
            return;
        }

        if (!inventory.PutCardInPool(card))
        {
            Debug.LogError("Failed to discard card: " + card.name);
        }

        AddWattction(1);
    }
    public void AddWattction(int amount)
    {
        currentWattction += amount;
        if (currentWattction > maxWattction)
        {
            currentWattction = maxWattction;
        }
        else if (currentWattction < 0)
        {
            currentWattction = 0;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        wattctionText.text = currentWattction.ToString() + "/" + maxWattction.ToString();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("PlayerLevel", level);
        PlayerPrefs.SetInt("PlayerExp", exp);
        PlayerPrefs.SetInt("PlayerNbEntreprise", nbEntreprise);
        PlayerPrefs.Save();
    }
}
