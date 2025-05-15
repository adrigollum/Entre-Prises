using UnityEngine;

public class DrawButton : MonoBehaviour, IClickable
{
    public GameObject card;
    public Transform summoningPosition;
    public Inventory playerInventory;

    void Start()
    {
        summoningPosition.gameObject.SetActive(false);
    }
    public void onClick(Vector2 position, IClickable.ClickType button, bool isDown = true)
    {
        if (button == IClickable.ClickType.LeftClick && isDown)
        {
            if (!playerInventory.isFull())
            {
                GameObject genCard = Instantiate(card, summoningPosition.position, summoningPosition.rotation);
                playerInventory.AddCard(genCard);
            }
        }
    }
}
