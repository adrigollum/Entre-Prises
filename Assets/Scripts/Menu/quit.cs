using UnityEngine;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{
    
    public Button bouton; // << tu l’assignes dans l’Inspector
    public int waypointIndex;

    void Start()
    {
        bouton.onClick.AddListener(() => {
            Debug.Log("Quitter le jeu...");

        #if UNITY_EDITOR
                // Arrête le mode Play dans l'éditeur Unity
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    // Quitte l'application dans la build finale
                    Application.Quit();
        #endif
                });
    }

}
