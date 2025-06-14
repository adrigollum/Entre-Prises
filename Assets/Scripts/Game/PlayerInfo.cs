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
        exp = PlayerPrefs.GetInt("PlayerExp", 0);
        level = ExpToLevel(exp);
        nbEntreprise = StaticEntreprisesSaveManager.CountEnemiesByStatus(EnumGameStatus.Won);

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

    private int ExpToLevel(int exp)
    {
        int level2Exp = 50;
        int level3Exp = 100;
        int level4Exp = 350;
        int level5Exp = 600;

        if (exp < level2Exp)
        {
            return 1;
        }
        else if (exp < level2Exp + level3Exp)
        {
            return 2;
        }
        else if (exp < level2Exp + level3Exp + level4Exp)
        {
            return 3;
        }
        else if (exp < level2Exp + level3Exp + level4Exp + level5Exp)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("PlayerExp", exp);
        PlayerPrefs.Save();
    }
}
