using UnityEngine;
using UnityEngine.InputSystem;

public class CardMovement : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 targetPosition = Vector3.positiveInfinity;

    public float percentScreenCardPlayed = 0.7f;
    public bool isInPlayingArea = false;

    public float selectedOffset = 0.5f;
    void Start()
    {
        if (targetPosition == Vector3.positiveInfinity)
        {
            targetPosition = transform.position;
        }

        isInPlayingArea = false;
    }

    void Update()
    {
        if (Mouse.current != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            isInPlayingArea = mousePosition.y / Screen.height > percentScreenCardPlayed;
        }

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
}
