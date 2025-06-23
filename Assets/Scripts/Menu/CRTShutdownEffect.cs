using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro; // Si tu utilises TextMeshPro (optionnel)

public class CRTShutdownEffect : MonoBehaviour
{
    public RawImage targetRawImage;
    public float duration = 1.0f;
    public List<GameObject> objectsToDisable;

    private RectTransform rectTransform;
    private Button button;
    private Image buttonGraphic;
    private Text uiText;
    private TextMeshProUGUI tmpText;

    [SerializeField] private AudioSource audioSource;

    public void Start()
    {
        button = GetComponent<Button>();
        buttonGraphic = GetComponent<Image>();
        uiText = GetComponentInChildren<Text>(); // Pour UI Text classique
        tmpText = GetComponentInChildren<TextMeshProUGUI>(); // Pour TextMeshPro (si utilisé)

        button.onClick.AddListener(() =>
        {
            audioSource.Play();

            if (targetRawImage == null) return;

            // 🔻 Masquer visuellement le bouton et le texte
            if (buttonGraphic != null) buttonGraphic.enabled = false;
            if (uiText != null) uiText.enabled = false;
            if (tmpText != null) tmpText.enabled = false;

            // 🔻 Désactiver les autres objets
            foreach (var obj in objectsToDisable)
            {
                if (obj != null)
                    obj.SetActive(false);
            }

            rectTransform = targetRawImage.GetComponent<RectTransform>();
            StartCoroutine(ShutdownCRT());
        });
    }

    private System.Collections.IEnumerator ShutdownCRT()
    {
        Vector3 originalScale = rectTransform.localScale;

        // Phase 1 : réduction verticale
        float t = 0f;
        while (t < duration / 2f)
        {
            t += Time.deltaTime;
            float scaleY = Mathf.Max(0.01f, 1f - (t / (duration / 2f)));
            rectTransform.localScale = new Vector3(originalScale.x, originalScale.y * scaleY, originalScale.z);
            yield return null;
        }

        // Phase 2 : réduction horizontale
        t = 0f;
        while (t < duration / 2f)
        {
            t += Time.deltaTime;
            float scaleX = Mathf.Max(0.01f, 1f - (t / (duration / 2f)));
            rectTransform.localScale = new Vector3(originalScale.x * scaleX, 0.01f, originalScale.z);
            yield return null;
        }

        targetRawImage.gameObject.SetActive(false);
        gameObject.SetActive(false);

        // Changer de scène ici (remplace "NomDeTaScene" par le nom exact)
        SceneManager.LoadScene("MainMenu");
    }
}
