using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class noir : MonoBehaviour
{
    [SerializeField] private Color faceColor = Color.black;

    void Start()
    {
        // Récupère le TMP
        TextMeshProUGUI tmp = GetComponent<TextMeshProUGUI>();

        // Crée un matériau unique à partir du matériel actuel
        tmp.fontMaterial = new Material(tmp.fontMaterial);

        // Change uniquement la "face color" sur ce matériel
        tmp.fontMaterial.SetColor("_FaceColor", faceColor);
    }
}

