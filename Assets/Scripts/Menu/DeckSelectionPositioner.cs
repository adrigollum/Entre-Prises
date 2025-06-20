using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckSelectionPositioner : MonoBehaviour
{
    public float columnSpacing = 1.5f;
    public float rowSpacing = 1.5f;
    public int maxColumns = 3;
    public int maxDeckSize = 40;
    public List<GameObject> allCards = new List<GameObject>();
    public TextMeshProUGUI deckSizeText;
    public StaticDeckSave.DeckType deckType = StaticDeckSave.DeckType.Deck;

    public Dictionary<string, int> GetCount()
    {
        Dictionary<string, int> cardCount = new Dictionary<string, int>();
        foreach (GameObject card in allCards)
        {
            string cardName = card.GetComponent<CardInfo>().cardName;
            if (!cardCount.ContainsKey(cardName))
            {
                cardCount[cardName] = 0;
            }
            cardCount[cardName]++;
        }
        return cardCount;
    }
    private void UpdateCount()
    {
        Dictionary<string, int> cardCount = GetCount();

        foreach (GameObject card in allCards)
        {
            string cardName = card.GetComponent<CardInfo>().cardName;
            if (cardCount.ContainsKey(cardName))
            {
                card.GetComponent<CardInfo>().MaxCardInDeck = cardCount[cardName];
                card.GetComponent<CardInfo>().UpdateUI();
            }
            else
            {
                Debug.LogWarning("Card name not found in count dictionary: " + cardName);
            }
        }
    }
    public void PositionDeckSelection()
    {
        if (allCards.Count == 0)
        {
            Debug.LogWarning("No cards to position.");
            return;
        }
        allCards.Sort((a, b) => a.GetComponent<CardInfo>().name.CompareTo(b.GetComponent<CardInfo>().name));

        Vector3 startPosition = transform.position;
        Vector3 offset = new Vector3(-columnSpacing * (maxColumns - 1) / 2f, 0, 0);

        UpdateCount();

        // Along the x and z axis stack card with same cardName
        string lastCardName = allCards[0].GetComponent<CardInfo>().cardName;
        int sameCardIndex = 0;
        for (int i = 0; i < allCards.Count; i++)
        {
            if (lastCardName != allCards[i].GetComponent<CardInfo>().cardName)
            {
                lastCardName = allCards[i].GetComponent<CardInfo>().cardName;
                sameCardIndex++;
            }
            int column = sameCardIndex % maxColumns;
            int row = sameCardIndex / maxColumns;

            Vector3 position = startPosition + offset + new Vector3(column * columnSpacing, 0, row * rowSpacing);
            allCards[i].GetComponent<CardMovement>().targetPosition = position;

            allCards[i].GetComponent<CardInfo>().cardNumberText.gameObject.SetActive(true);
            allCards[i].GetComponent<CardInfo>().UpdateUI();
        }

        UpdateUI();
    }
    public void Init(StaticDeckSave.DeckType deckType)
    {
        this.deckType = deckType;
        if (deckType == StaticDeckSave.DeckType.NotDeck)
        {
            maxDeckSize = int.MaxValue;
        }
        else if (deckType == StaticDeckSave.DeckType.Deck)
        {
            maxDeckSize = 40;
        }
        else
        {
            Debug.LogError("Unknown deck type: " + deckType);
            return;
        }

        Dictionary<string, int> save = StaticDeckSave.GetDeck(deckType);

        foreach (Transform child in transform)
        {
            CardInfo cardInfo = child.gameObject.GetComponent<CardInfo>();
            if (cardInfo != null)
            {
                if (save.ContainsKey(cardInfo.cardName) && save[cardInfo.cardName] > 0)
                {
                    cardInfo.MaxCardInDeck = save[cardInfo.cardName];
                    allCards.Add(child.gameObject);
                }
                else
                {
                    Debug.LogWarning("Card not found in save: " + cardInfo.cardName);
                    Destroy(child.gameObject);
                }
            }
            else
            {
                Debug.LogWarning("Child does not have CardInfo component: " + child.name);
            }
        }

        // For each card copy the gameObject CardInfo.MaxCardInDeck times
        List<GameObject> tempCards = new List<GameObject>();
        foreach (GameObject card in allCards)
        {
            for (int i = 0; i < card.GetComponent<CardInfo>().MaxCardInDeck - 1; i++)
            {
                GameObject newCard = Instantiate(card, transform);
                newCard.name = card.name + " " + (i + 1);
                tempCards.Add(newCard);
            }
        }
        allCards.AddRange(tempCards);

        foreach (GameObject card in allCards)
        {
            card.GetComponent<CardMovement>().percentScreenUpY = 0f;
        }

        PositionDeckSelection();
    }
    public void RepositionAllCards()
    {
        PositionDeckSelection();
    }

    public bool AddCard(GameObject card)
    {
        if (!allCards.Contains(card) && allCards.Count < maxDeckSize)
        {
            allCards.Add(card);
            PositionDeckSelection();
            return true;
        }
        return false;
    }

    public bool RemoveCard(GameObject card)
    {
        if (allCards.Contains(card))
        {
            allCards.Remove(card);
            PositionDeckSelection();
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        if (deckSizeText != null)
        {
            deckSizeText.text = $"{allCards.Count}/{maxDeckSize}";
        }
    }

    // void Update()
    // {
    //     PositionDeckSelection();
    // }
}
