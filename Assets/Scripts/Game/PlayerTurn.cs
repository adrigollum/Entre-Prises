using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public Transform summoningPosition;
    public void Init()
    {
        playerInfo = GetComponent<PlayerInfo>();
        playerInfo.Init();
        playerInfo.inventory.Init();
        DrawCards();
    }
    public void DrawCards()
    {
        while (playerInfo.inventory.AddRandomCard(summoningPosition)) ;
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
        playerInfo.inventory.RepositionAllCards();
    }

    public void Save()
    {
        playerInfo.Save();
    }
}
