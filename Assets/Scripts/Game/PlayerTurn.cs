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
        return playerInfo.PlayCard(card);
    }

    public void DiscardCard(GameObject card)
    {
        playerInfo.DiscardCard(card);
    }

    public void RepositionAllCards()
    {
        playerInventory.RepositionAllCards();
    }
}
