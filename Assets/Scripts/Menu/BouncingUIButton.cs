using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class BouncingUIButton : MonoBehaviour
{
    public float speed = 200f;

    private RectTransform rectTransform;
    private RectTransform parentRect;
    public Vector3 direction;
    private Vector2 size;

    public static List<BouncingUIButton> allButtons = new List<BouncingUIButton>();

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();

        size = new Vector2(rectTransform.rect.width * rectTransform.localScale.x,
                           rectTransform.rect.height * rectTransform.localScale.y);

        direction = Random.insideUnitCircle.normalized;
        allButtons.Add(this);
    }

    void OnDestroy()
    {
        allButtons.Remove(this);
    }

    void Update()
    {
        Vector3 pos = rectTransform.localPosition;
        pos += direction * speed * Time.deltaTime;

        Vector2 halfSize = size / 2f;
        Vector2 canvasHalfSize = new Vector2(parentRect.rect.width * parentRect.localScale.x / 2f,
                                             parentRect.rect.height * parentRect.localScale.y / 2f);

        // Wall collisions
        if (pos.x + halfSize.x > canvasHalfSize.x)
        {
            pos.x = canvasHalfSize.x - halfSize.x;
            direction.x *= -1;
        }
        else if (pos.x - halfSize.x < -canvasHalfSize.x)
        {
            pos.x = -canvasHalfSize.x + halfSize.x;
            direction.x *= -1;
        }

        if (pos.y + halfSize.y > canvasHalfSize.y)
        {
            pos.y = canvasHalfSize.y - halfSize.y;
            direction.y *= -1;
        }
        else if (pos.y - halfSize.y < -canvasHalfSize.y)
        {
            pos.y = -canvasHalfSize.y + halfSize.y;
            direction.y *= -1;
        }

        rectTransform.localPosition = new Vector3(pos.x, pos.y, 0f);

        HandleCollisions();
    }

    void HandleCollisions()
    {
        foreach (var other in allButtons)
        {
            if (other == this) continue;

            Vector3 posA = rectTransform.localPosition;
            Vector3 posB = other.rectTransform.localPosition;

            float minDistanceX = (size.x + other.size.x) * 0.5f;
            float minDistanceY = (size.y + other.size.y) * 0.5f;

            float dx = posA.x - posB.x;
            float dy = posA.y - posB.y;

            if (Mathf.Abs(dx) < minDistanceX && Mathf.Abs(dy) < minDistanceY)
            {
                // Éviter les micro-collisions en dessous d’un seuil
                Vector2 push = new Vector2(dx / minDistanceX, dy / minDistanceY).normalized * 0.5f;
                rectTransform.localPosition += (Vector3)push;
                other.rectTransform.localPosition -= (Vector3)push;

                direction = new Vector3(-other.direction.y, other.direction.x, 0f);
            }
        }
    }
}