using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int deckSize = 5;
    public List<GameObject> deck = new List<GameObject>();

    public List<GameObject> allCardDeck = new List<GameObject>();

    public float deckWidth = 1.6f;

    private void Start()
    {
        // Init Random
        Random.InitState(System.DateTime.Now.Millisecond);
    }
    public void InitInventory()
    {
        allCardDeck = new List<GameObject>();

        Debug.Log("Initializing Inventory...");
        Transform allCardsTransform = transform.Find("AllCards");
        if (allCardsTransform != null)
        {
            foreach (Transform child in allCardsTransform)
            {
                if (child.gameObject.GetComponent<CardInfo>() != null)
                {
                    allCardDeck.Add(child.gameObject);
                }
                else
                {
                    Debug.LogWarning("Child does not have CardInfo component: " + child.name);
                }
            }
        }
        else
        {
            Debug.LogError("AllCards transform not found in Inventory.");
        }
    }
    public bool IsFull()
    {
        return deck.Count >= deckSize;
    }

    private int GetAllDeckSize()
    {
        int size = 0;
        foreach (GameObject card in allCardDeck)
        {
            if (card != null && card.GetComponent<CardInfo>() != null)
            {
                size += card.GetComponent<CardInfo>().MaxCardInDeck;
            }
        }

        return size;
    }

    private GameObject GetRandomCardInDeck()
    {
        if (allCardDeck.Count == 0)
        {
            Debug.LogError("No cards available in the deck to select from.");
            return null;
        }
        
        int allDeckSize = GetAllDeckSize();
        int randomIndex = Random.Range(0, allDeckSize);
        Debug.Log("Random index selected: " + randomIndex + " from total size: " + allDeckSize);
        int currentIndex = 0;
        foreach (GameObject card in allCardDeck)
        {
            if (card != null && card.GetComponent<CardInfo>() != null)
            {
                CardInfo cardInfo = card.GetComponent<CardInfo>();
                for (int i = 0; i < cardInfo.MaxCardInDeck; i++)
                {
                    if (currentIndex == randomIndex)
                    {
                        return card;
                    }
                    currentIndex++;
                }
            }
        }

        Debug.LogError("Random card selection failed. This should not happen if the deck is properly populated.");
        return null;
    }

    public bool AddRandomCard(Transform summoningPosition)
    {
        if (allCardDeck.Count == 0)
        {
            InitInventory();
        }

        if (IsFull())
        {
            Debug.Log("Deck is full. Cannot add more cards.");
            return false;
        }
        GameObject randomCard = GetRandomCardInDeck();
        if (randomCard == null)
        {
            Debug.LogError("No card available to add.");
            return false;
        }

        randomCard.GetComponent<CardInfo>().MaxCardInDeck--;
        
        return AddCardFromPrefab(randomCard, summoningPosition); ;
    }

    public bool AddCardFromPrefab(GameObject cardPrefab, Transform summoningPosition)
    {
        if (IsFull())
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
