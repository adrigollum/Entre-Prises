using UnityEngine;
using UnityEngine.InputSystem;

public class CardMovement : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 targetPosition = Vector3.positiveInfinity;

    public float percentScreenUpY = 0.7f;
    public float percentScreenCardPlayedX = 0.5f;
    public float selectedOffset = 0.5f;
    void Start()
    {
        if (targetPosition == Vector3.positiveInfinity)
        {
            targetPosition = transform.position;
        }
    }

    void Update()
    {
        IsInPlayingArea();

        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance > 0.001f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            float smoothSpeed = SmoothSpeed(distance);
            transform.position += direction * smoothSpeed * Time.deltaTime;
        }
    }

    private float SmoothSpeed(float distance)
    {
        return Mathf.Clamp(distance * speed, 0.1f, speed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, targetPosition);
        Gizmos.DrawSphere(targetPosition, 0.1f);
    }

    public enum CardArea
    {
        None,
        PlayingArea,
        DiscardArea
    }
    public CardArea IsInPlayingArea()
    {
        if (Mouse.current == null)
        {
            return CardArea.None;
        }

        Vector3 cardScreenPosition = Camera.main.WorldToScreenPoint(transform.position);

        bool isInUpArea = cardScreenPosition.y > Screen.height * percentScreenUpY;
        bool isInPlayingArea = cardScreenPosition.x < Screen.width * percentScreenCardPlayedX;

        if (isInUpArea && isInPlayingArea)
        {
            PlayingAreaShader();
            return CardArea.PlayingArea;
        }
        else if (isInUpArea && !isInPlayingArea)
        {
            DiscardAreaShader();
            return CardArea.DiscardArea;
        }

        ResetShader();
        return CardArea.None;
    }

    public void PlayingAreaShader()
    {
    }

    public void DiscardAreaShader()
    {
    }

    public void ResetShader()
    {
    }
}
