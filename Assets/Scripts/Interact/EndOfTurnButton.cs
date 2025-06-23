using UnityEngine;
using System.Collections;

public class EndOfTurnButton : MonoBehaviour, IClickable
{
    private GameTurn gameTurn;

    [SerializeField] private float squishAmount = 0.7f; // Écrasement vertical (plus bas = plus écrasé)
    [SerializeField] private float duration = 0.1f; // Durée d'aller/retour

    private Vector3 originalScale;
    private bool isSquishing = false;

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

        originalScale = transform.localScale;
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

        Squish();
    }

    public void Squish()
    {
        if (!isSquishing)
            StartCoroutine(SquishRoutine());
    }

    private IEnumerator SquishRoutine()
    {
        isSquishing = true;

        Vector3 squishScale = new Vector3(originalScale.x * 1.1f, originalScale.y * squishAmount, originalScale.z * 1.1f);

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, squishScale, t / duration);
            yield return null;
        }

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(squishScale, originalScale, t / duration);
            yield return null;
        }

        transform.localScale = originalScale;
        isSquishing = false;
    }
}
