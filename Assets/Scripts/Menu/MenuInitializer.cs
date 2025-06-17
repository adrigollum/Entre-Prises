using UnityEngine;

public class MenuInitializer : MonoBehaviour
{
    [Header("Objet à faire apparaître")]
    public GameObject objectToShow;
    public MenuCamMovement cameraController;
    public int waypointIndex;
    private MenuMovementButton menuButtonScript;

    void Start()
    {
        // 1. Récupérer le script sur ce même GameObject 
        cameraController.GoToPoint(waypointIndex);
        
    }
}