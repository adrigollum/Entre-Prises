using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int deckSize = 5;
    public List<GameObject> deck = new List<GameObject>();

    public float deckWidth = 0.5f;
    private void Start()
    {
        deck = new List<GameObject>();
    }
    private void Update()
    {
        RepositionAllCards();
    }
    public bool isFull()
    {
        return deck.Count >= deckSize;
    }

    public bool AddCard(GameObject cardPrefab)
    {
        if (isFull())
        {
            Debug.Log("Deck is full. Cannot add more cards.");
            return false;
        }

        deck.Add(cardPrefab);
        Debug.Log($"Card {cardPrefab.name} added to deck. Current deck size: {deck.Count}");
        RepositionAllCards();
        return true;
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
