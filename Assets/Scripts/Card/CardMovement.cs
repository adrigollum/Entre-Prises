using Unity.VisualScripting;
using UnityEngine;

public class CardMovement : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 targetPosition = Vector3.positiveInfinity;

    public bool isSelected = false;
    public float selectedOffset = 0.5f;
    void Start()
    {
        if (targetPosition == Vector3.positiveInfinity)
        {
            targetPosition = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 realTargetPosition = RealTargetPosition();
        float distance = Vector3.Distance(transform.position, realTargetPosition);
        if (distance > 0.001f)
        {
            Vector3 direction = (realTargetPosition - transform.position).normalized;
            float smoothSpeed = SmoothSpeed(distance);
            transform.position += direction * smoothSpeed * Time.deltaTime;
        }
    }

    private float SmoothSpeed(float distance)
    {
        return Mathf.Clamp(distance * speed, 0.1f, speed);
    }

    private Vector3 RealTargetPosition()
    {
        Vector3 realTargetPosition = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
        if (isSelected)
        {
            realTargetPosition.y += selectedOffset;
        }

        return realTargetPosition;
    }

    private void OnDrawGizmos()
    {
        Vector3 realTargetPosition = RealTargetPosition();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, realTargetPosition);
        Gizmos.DrawSphere(realTargetPosition, 0.1f);
    }
}
