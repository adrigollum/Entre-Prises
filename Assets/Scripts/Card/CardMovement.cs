using UnityEngine;
using UnityEngine.InputSystem;

public class CardMovement : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 targetPosition = Vector3.positiveInfinity;

    public float percentScreenCardPlayed = 0.7f;
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

    public bool IsInPlayingArea()
    {
        if (Mouse.current == null)
        {
            return false;
        }

        Vector3 cardScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        bool isInPlayingArea = cardScreenPosition.y > Screen.height * percentScreenCardPlayed;

        if (isInPlayingArea)
        {
            PlayingAreaShader();
        }
        else
        {
            ResetShader();
        }
        return isInPlayingArea;
    }

    public void PlayingAreaShader()
    {
    }

    public void ResetShader()
    {
    }
}
