using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfGameUIButton : MonoBehaviour
{
    public string sceneToLoad = "MainMenu";
    void Start()
    {
        Button bouton = GetComponent<Button>();
        if (bouton == null)
        {
            Debug.LogError("Button component not found on this GameObject.");
            return;
        }

        bouton.onClick.AddListener(() =>
        {
            StaticSceneInfo.mainMenuCamIndex = StaticSceneInfo.MAINMENUEPLAYINDEX;
            SceneManager.LoadScene(sceneToLoad);
        });
    }
}
