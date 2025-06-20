using UnityEngine;
using UnityEngine.UI;
public class PanelCloser : MonoBehaviour
{
    public GameObject panelToClose; // Assigne le panel dans l'Inspector


public void Start()
    {
        Button button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found on this GameObject.");
            return;
        }

        button.onClick.AddListener(() =>
        {
            panelToClose.SetActive(false);
        Debug.Log("Panel closed: " + panelToClose.name);
        });
    }

    
}
