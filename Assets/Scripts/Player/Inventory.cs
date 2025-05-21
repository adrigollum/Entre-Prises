using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int deckSize = 5;
    public List<GameObject> deck = new List<GameObject>();

    public float deckWidth = 0.5f;
    public bool isFull()
    {
        return deck.Count >= deckSize;
    }

    public bool AddCard(GameObject cardPrefab, Transform summoningPosition)
    {
        if (isFull())
        {
            Debug.Log("Deck is full. Cannot add more cards.");
            return false;
        }

        GameObject genCard = Instantiate(cardPrefab, summoningPosition.position, summoningPosition.rotation);

        deck.Add(genCard);

        RepositionAllCards();

        return true;
    }

    public void RemoveCard(GameObject card)
    {
        if (deck.Contains(card))
        {
            deck.Remove(card);
            Destroy(card);
            RepositionAllCards();
        }
        else
        {
            Debug.Log("Card not found in deck.");
        }
    }

    public void RepositionAllCards()
    {
        float xOffset = ((deckWidth * 2) / (deck.Count + 1));
        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].GetComponent<CardMovement>().targetPosition = new Vector3(
                transform.position.x + ((i + 1) * xOffset - deckWidth),
                transform.position.y,
                transform.position.z);
        }
    }
}
