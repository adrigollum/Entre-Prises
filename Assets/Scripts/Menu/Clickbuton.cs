using UnityEngine;
using UnityEngine.UI;

public class Clickbuton : MonoBehaviour
{
    public Button bouton; // << tu l’assignes dans l’Inspector
    public Cambouge cameraController;
    public int waypointIndex;

    void Start()
    {
        bouton.onClick.AddListener(() => {
            cameraController.GoToPoint(waypointIndex);
        });
    }
}
