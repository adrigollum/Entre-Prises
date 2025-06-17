using System.Collections.Generic;
using UnityEngine;

public class DeckSelectionPositioner : MonoBehaviour
{
    public float columnSpacing = 1.5f;
    public float rowSpacing = 1.5f;
    public int maxColumns = 3;
    public List<GameObject> allCards = new List<GameObject>();
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
    }
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<CardInfo>() != null)
            {
                allCards.Add(child.gameObject);
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

        PositionDeckSelection();
    }
    public void RepositionAllCards()
    {
        PositionDeckSelection();
    }

    // void Update()
    // {
    //     PositionDeckSelection();
    // }
}
