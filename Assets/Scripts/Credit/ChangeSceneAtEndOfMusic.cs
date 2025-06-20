using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAtEndOfMusic : MonoBehaviour
{
    public AudioSource musicSource;      // Assigne ton AudioSource dans l’inspecteur
    public string sceneToLoad;           // Nom de la scène à charger

    private bool hasSwitchedScene = false;

    void Update()
    {
        if (musicSource != null && !musicSource.isPlaying && !hasSwitchedScene)
        {
            hasSwitchedScene = true;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
