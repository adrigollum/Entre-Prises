using System.Collections;
using UnityEngine;

public class Squich : MonoBehaviour, IClickable
{
    [SerializeField] private float squishAmount = 0.7f; // Écrasement vertical (plus bas = plus écrasé)
    [SerializeField] private float duration = 0.1f; // Durée d'aller/retour
    private Vector3 originalScale;
    private bool isSquishing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = transform.localScale;
    }

    public void onClick(GameObject camera, Vector3 worldPosition, Vector2 position, IClickable.ClickType button, bool isDown = true)
    {
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
