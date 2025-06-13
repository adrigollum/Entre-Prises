using UnityEngine;
using UnityEngine.UI;

public class MenuMovementButton : MonoBehaviour
{
    public MenuCamMovement cameraController;
    public int waypointIndex;

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found on this GameObject.");
            return;
        }

        button.onClick.AddListener(() =>
        {
            cameraController.GoToPoint(waypointIndex);
        });
    }
}
