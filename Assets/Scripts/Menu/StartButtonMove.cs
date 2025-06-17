using UnityEngine;

public class StartButtonMove : MonoBehaviour
{
    public float speed = 200f;

    private RectTransform rectTransform;
    private RectTransform parentRect;
    private Vector3 direction;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();

        // Direction aléatoire dans le plan XY
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
    }

    void Update()
    {
        // Déplacer dans le plan local (UI en World Space)
        rectTransform.localPosition += direction * speed * Time.deltaTime;

        // Obtenir les bords en local space
        Vector3 pos = rectTransform.localPosition;

        float halfWidth = rectTransform.rect.width * rectTransform.localScale.x * 0.5f;
        float halfHeight = rectTransform.rect.height * rectTransform.localScale.y * 0.5f;

        float parentHalfWidth = parentRect.rect.width * parentRect.localScale.x * 0.5f;
        float parentHalfHeight = parentRect.rect.height * parentRect.localScale.y * 0.5f;

        // Rebond horizontal
        if (pos.x + halfWidth > parentHalfWidth)
        {
            pos.x = parentHalfWidth - halfWidth;
            direction.x *= -1;
        }
        else if (pos.x - halfWidth < -parentHalfWidth)
        {
            pos.x = -parentHalfWidth + halfWidth;
            direction.x *= -1;
        }

        // Rebond vertical
        if (pos.y + halfHeight > parentHalfHeight)
        {
            pos.y = parentHalfHeight - halfHeight;
            direction.y *= -1;
        }
        else if (pos.y - halfHeight < -parentHalfHeight)
        {
            pos.y = -parentHalfHeight + halfHeight;
            direction.y *= -1;
        }

        // Appliquer position corrigée
        pos.z = 0f; // rester dans le plan du Canvas
        rectTransform.localPosition = pos;
    }
}