using UnityEngine;

public interface IClickable
{
    public enum ClickType
    {
        None,
        LeftClick,
        RightClick,
        MiddleClick
    }
    public void onClick(Vector2 position, ClickType button, bool isDown = true);
}
