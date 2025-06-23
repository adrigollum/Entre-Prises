using UnityEngine;
using UnityEngine.UI;

public class MenuMovementButton : MonoBehaviour
{
    public MenuCamMovement cameraController;
    public int waypointIndex;

    [SerializeField] private AudioClip ClicSound;

    [SerializeField] private AudioSource SFX;

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
            SFX.PlayOneShot(ClicSound);
            cameraController.GoToPoint(waypointIndex);
        });
    }
}
