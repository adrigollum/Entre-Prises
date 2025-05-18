using UnityEngine;

public class CardClick : MonoBehaviour, IClickable
{
    public void onClick(Vector2 position, IClickable.ClickType button, bool isDown = true)
    {
        GetComponent<CardMovement>().isSelected = !GetComponent<CardMovement>().isSelected;
    }
}
