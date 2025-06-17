using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartButtonAction : MonoBehaviour
{
    public GameObject objectToActivate;
    public RawImage backgroundRawImageToFadeOut;
    public Light pointLight1;
    public Light pointLight2;

    public float buttonShrinkDuration = 0.5f;
    public float verticalShrinkDuration = 0.6f;
    public float linePauseDuration = 0.15f;
    public float horizontalShrinkDuration = 0.2f;

    [Tooltip("Hauteur cible finale (en proportion de la taille initiale) lors du rétrécissement vertical.")]
    public float targetHeightRatio = 0.05f; // ex: 0.05 = 5% de la hauteur initiale
    public float targetHeightMin = 0.01f;  // valeur minimale absolue en unité UI

    [Tooltip("Active ou non l'effet 'vieille TV' à l'extinction.")]
    public bool useTvEffect = true;

    private Button button;
    private RectTransform buttonRect;
    private RectTransform bgRect;
    private Vector2 originalBgSize;
    private Vector2 originalPivot;

    void Start()
    {
        button = GetComponent<Button>();
        buttonRect = GetComponent<RectTransform>();

        if (backgroundRawImageToFadeOut != null)
        {
            bgRect = backgroundRawImageToFadeOut.GetComponent<RectTransform>();
            originalBgSize = bgRect.sizeDelta;
            originalPivot = bgRect.pivot;

            // Pivot au centre pour rétrécir depuis le milieu
            bgRect.pivot = new Vector2(0.5f, 0.5f);
        }

        if (button != null)
        {
            button.onClick.AddListener(OnStartClicked);
        }
    }

    void OnStartClicked()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        // 1. Bouton réduit sa taille (scale down)
        float timer = 0f;
        Vector3 originalScale = buttonRect.localScale;
        while (timer < buttonShrinkDuration)
        {
            float t = timer / buttonShrinkDuration;
            buttonRect.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            timer += Time.deltaTime;
            yield return null;
        }
        buttonRect.localScale = Vector3.zero;

        if (useTvEffect && bgRect != null)
        {
            // Effet vieille TV complet

            float light1Start = pointLight1 != null ? pointLight1.intensity : 0f;
            float light2Start = pointLight2 != null ? pointLight2.intensity : 0f;

            float targetHeight = Mathf.Max(originalBgSize.y * targetHeightRatio, targetHeightMin);

            // 2a. Rétrécissement vertical
            timer = 0f;
            while (timer < verticalShrinkDuration)
            {
                float t = timer / verticalShrinkDuration;
                float newHeight = Mathf.Lerp(originalBgSize.y, targetHeight, t);
                bgRect.sizeDelta = new Vector2(originalBgSize.x, newHeight);

                if (pointLight1 != null)
                    pointLight1.intensity = Mathf.Lerp(light1Start, light1Start * 0.2f, t);
                if (pointLight2 != null)
                    pointLight2.intensity = Mathf.Lerp(light2Start, light2Start * 0.2f, t);

                timer += Time.deltaTime;
                yield return null;
            }
            bgRect.sizeDelta = new Vector2(originalBgSize.x, targetHeight);

            yield return new WaitForSeconds(linePauseDuration);

            // 2b. Réduction rapide largeur
            timer = 0f;
            while (timer < horizontalShrinkDuration)
            {
                float t = timer / horizontalShrinkDuration;
                float newWidth = Mathf.Lerp(originalBgSize.x, 0f, t);
                bgRect.sizeDelta = new Vector2(newWidth, targetHeight);

                if (pointLight1 != null)
                    pointLight1.intensity = Mathf.Lerp(light1Start * 0.2f, 0f, t);
                if (pointLight2 != null)
                    pointLight2.intensity = Mathf.Lerp(light2Start * 0.2f, 0f, t);

                timer += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            // Si l'effet TV est désactivé : on ne touche ni à l'image ni aux lumières
            // Pas d'animation sur bgRawImage ni intensité des lights
        }

        // Désactivation image et lumières (seulement si l’effet TV est activé)
        if (useTvEffect)
        {
            if (backgroundRawImageToFadeOut != null)
                backgroundRawImageToFadeOut.gameObject.SetActive(false);
            if (pointLight1 != null)
                pointLight1.gameObject.SetActive(false);
            if (pointLight2 != null)
                pointLight2.gameObject.SetActive(false);
        }
        // Sinon on laisse tout actif

        if (bgRect != null)
            bgRect.pivot = originalPivot;

        // Activation du nouvel objet
        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        // Désactivation du bouton (toujours à la fin)
        gameObject.SetActive(false);
    }
}