using UnityEngine;

public class CardClick : MonoBehaviour, IClickable
{
    [SerializeField] private AudioClip clips;

    [SerializeField] private string audioSourceNameInScene = "ErrorSoundPlayer";

    private AudioSource audioSource;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(audioSourceNameInScene))
        {
            GameObject audioObj = GameObject.Find(audioSourceNameInScene);
            if (audioObj != null)
            {
                audioSource = audioObj.GetComponent<AudioSource>();
            }
        }
    }
    public void onClick(GameObject camera, Vector3 worldPosition, Vector2 position, IClickable.ClickType button, bool isDown = true)
    {
        audioSource.PlayOneShot(clips);
        // GetComponent<CardMovement>().isSelected = !GetComponent<CardMovement>().isSelected;
    }
}
