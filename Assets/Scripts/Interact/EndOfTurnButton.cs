using UnityEngine;

public class EndOfTurnButton : MonoBehaviour, IClickable
{
    private GameTurn gameTurn;

    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        if (gameManager != null)
        {
            gameTurn = gameManager.GetComponent<GameTurn>();
        }
        else
        {
            Debug.LogError("GameManager not found. Ensure it is present in the scene.");
        }
    }
    public void onClick(GameObject camera, Vector3 worldPosition, Vector2 position, IClickable.ClickType button, bool isDown = true)
    {
        Debug.Log("EndOfTurnButton clicked: " + button + ", isDown: " + isDown);
        if (gameTurn == null)
        {
            Debug.LogError("GameTurn component is not initialized.");
            return;
        }

        if (button == IClickable.ClickType.LeftClick && isDown)
        {
            gameTurn.EndTurn();
        }
    }
}
