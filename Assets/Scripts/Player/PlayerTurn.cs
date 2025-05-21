using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    public Inventory playerInventory;
    public PlayerInfo playerInfo;
    public GameObject card;
    public Transform summoningPosition;
    public void DrawCards()
    {
        while (playerInventory.AddCard(card, summoningPosition)) ;
    }

    public void PlaySelectedCard()
    {
    }
}
