using UnityEngine;

public class AutoQuitAfterSeconds : MonoBehaviour
{
    [SerializeField] private float delayInSeconds = 10f; // Temps avant arrêt

    private void Start()
    {
        Invoke(nameof(StopGame), delayInSeconds);
    }

    private void StopGame()
    {
#if UNITY_EDITOR
        // Stoppe le mode Play dans l'éditeur
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quitte le jeu si buildé
        Application.Quit();
#endif
    }
}
