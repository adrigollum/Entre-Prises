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
    public void onClick(GameObject camera, Vector3 worldPosition, Vector2 position, IClickable.ClickType button, bool isDown = true)
    {
        if (button == IClickable.ClickType.LeftClick && isDown)
        {
            if (!playerInventory.isFull())
            {
                playerInventory.AddCard(card, summoningPosition);
            }
        }
    }
}
