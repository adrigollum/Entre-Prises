using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuCamMovement : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] points;
    public float speed = 5f;
    public float cameraRotationSpeed = 5f;

    [Header("Rotation de la porte")]
    public Transform objectToRotate;
    public float targetRotationZ = 90f;
    public float objectRotationDuration = 1f;

    [Header("Point secondaire")]
    public Transform secondaryPoint;

    private Transform[] currentPath = new Transform[0];
    private int pathIndex = 0;
    private bool isMoving = false;
    private bool sceneLoaded = false;
    private int lastReachedIndex = 0; // Mémorise le dernier index atteint dans points[]

    void Start()
    {
        pathIndex = StaticSceneInfo.mainMenuCamIndex;
        if (pathIndex != 0)
        {
            GoToPoint(pathIndex);
        }
    }

    void Update()
    {
        if (isMoving && currentPath.Length > 0 && pathIndex < currentPath.Length)
        {
            Transform target = currentPath[pathIndex];
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // --- Rotation spéciale si destination finale = point 4 ---
            Quaternion targetRotation;

            bool goingToPoint4 = (currentPath.Length >= 2 && currentPath[currentPath.Length - 1] == points[3]);

            if (goingToPoint4 && pathIndex < currentPath.Length - 1)
            {
                // Toujours regarder vers le point 4 (ignorer rotation du point secondaire)
                Vector3 lookDir = points[3].position - transform.position;
                if (lookDir.sqrMagnitude > 0.001f)
                    targetRotation = Quaternion.LookRotation(lookDir);
                else
                    targetRotation = transform.rotation; // pas de saut brusque
            }
            else
            {
                // Rotation normale vers le point courant
                targetRotation = target.rotation;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraRotationSpeed * Time.deltaTime);

            bool posReached = Vector3.Distance(transform.position, target.position) < 0.05f;
            bool rotReached = Quaternion.Angle(transform.rotation, targetRotation) < 1f;

            if (posReached && rotReached)
            {
                int pointIndex = System.Array.IndexOf(points, target);
                if (pointIndex != -1)
                {
                    lastReachedIndex = pointIndex;
                }

                pathIndex++;

                if (pathIndex >= currentPath.Length)
                {
                    isMoving = false;

                    if (currentPath[currentPath.Length - 1] == points[3] && !sceneLoaded)
                    {
                        sceneLoaded = true;
                        Debug.Log("Arrivé au point 3, chargement de la scène suivante...");
                        SceneManager.LoadScene("Credit");
                    }
                }
            }
        }
    }

    public void GoToPoint(int index)
    {
        if (index < 0 || index >= points.Length || isMoving)
            return;

        int previousIndex = lastReachedIndex;
        pathIndex = 0;

        // Si on va vers le point 3 (index 3), passer par le point secondaire
        if (index == 3 && secondaryPoint != null)
        {
            currentPath = new Transform[] { secondaryPoint, points[3] };
            Debug.Log("Déplacement fluide : point secondaire → point 3");
        }
        else
        {
            currentPath = new Transform[] { points[index] };
            Debug.Log($"Déplacement direct vers le point {index}");
        }

        isMoving = true;

        int lastIndex = points.Length - 1;
        if (previousIndex == 0 && index == lastIndex)
        {
            StartCoroutine(RotateObject(objectToRotate, targetRotationZ));
        }
        else if (previousIndex == lastIndex && index == 0)
        {
            StartCoroutine(RotateObject(objectToRotate, -targetRotationZ));
        }
    }

    IEnumerator RotateObject(Transform obj, float targetZ)
    {
        if (obj == null) yield break;

        float elapsed = 0f;
        Vector3 startEuler = obj.rotation.eulerAngles;
        Vector3 targetEuler = new Vector3(startEuler.x, startEuler.y, targetZ);

        while (elapsed < objectRotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / objectRotationDuration;
            Vector3 currentEuler = Vector3.Lerp(startEuler, targetEuler, t);
            obj.rotation = Quaternion.Euler(currentEuler);
            yield return null;
        }

        obj.rotation = Quaternion.Euler(targetEuler);
    }
}
