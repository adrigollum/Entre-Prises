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
    public void onClick(GameObject camera, Vector3 worldPosition, Vector2 position, ClickType button, bool isDown = true);
}
