using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    public Inventory playerInventory;
    public PlayerInfo playerInfo;
    public Transform summoningPosition;
    public void Init()
    {
        playerInventory.Init();
        playerInfo.Init();
        DrawCards();
    }
    public void DrawCards()
    {
        while (playerInventory.AddRandomCard(summoningPosition)) ;
    }

    public bool PlayCard(GameObject card)
    {
        CardInfo cardInfo = card.GetComponent<CardInfo>();
        if (cardInfo != null)
        {
            if (playerInfo.currentWattction >= cardInfo.cardWattctionCost)
            {
                playerInfo.AddWattction(-cardInfo.cardWattctionCost);

                playerInfo.inventory.RemoveCard(card);
                return true;
            }
            else
            {
                Debug.Log("Not enough Wattction to play this card.");
                return false;
            }
        }
        else
        {
            Debug.LogError("CardInfo component not found on the card.");
            return false;
        }
    }

    public void RepositionAllCards()
    {
        playerInventory.RepositionAllCards();
    }
}
