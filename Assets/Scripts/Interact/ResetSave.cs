using UnityEngine;

public class ResetSave : MonoBehaviour, IClickable
{
    public string ResetSceneName = "Hacking";
    private int clickCountToReset = 2;
    public void onClick(GameObject camera, Vector3 worldPosition, Vector2 position, IClickable.ClickType button, bool isDown = true)
    {
        if (clickCountToReset > 0)
        {
            clickCountToReset--;
            return;
        }
        Debug.Log("Resetting save data...");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene(ResetSceneName);
    }
}
