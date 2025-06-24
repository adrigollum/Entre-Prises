using UnityEngine;
using TMPro;
using System.Collections;

public class WindowsErrorSpawner : MonoBehaviour
{
    [Header("Fenêtres à faire pop")]
    [SerializeField] private GameObject[] windowPrefabs;

    [Header("Zone de spawn (Canvas World Space)")]
    [SerializeField] private RectTransform spawnArea;

    [Header("Options de spawn")]
    [SerializeField] private int numberToSpawn = 10;

    [Header("Délai avant début du spawn (secondes)")]
    [SerializeField] private float startDelay = 1f;

    [Header("Délai aléatoire entre apparitions (secondes)")]
    [SerializeField] private float minDelayBetweenSpawns = 0.1f;
    [SerializeField] private float maxDelayBetweenSpawns = 0.5f;

    [Header("Sons d'erreur (liste)")]
    [SerializeField] private AudioClip[] errorSounds;
    [SerializeField] private float soundVolume = 1f;

    [Header("Arrêt automatique")]
    [SerializeField] private float delayBeforeQuitAfterLastSpawn = 3f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        // délai avant de commencer à spawn
        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i < numberToSpawn; i++)
        {
            SpawnRandomWindow();

            // délai aléatoire entre spawns
            float delay = Random.Range(minDelayBetweenSpawns, maxDelayBetweenSpawns);
            yield return new WaitForSeconds(delay);
        }

        // Après la dernière fenêtre, attendre un peu puis arrêter le jeu
        yield return new WaitForSeconds(delayBeforeQuitAfterLastSpawn);
        StopGame();
    }

    private void SpawnRandomWindow()
    {
        if (windowPrefabs.Length == 0) return;

        // Choisir un prefab aléatoire
        GameObject selectedPrefab = windowPrefabs[Random.Range(0, windowPrefabs.Length)];

        // Instancier dans le Canvas
        GameObject instance = Instantiate(selectedPrefab, spawnArea);

        // Position aléatoire dans les limites du RectTransform
        RectTransform rt = instance.GetComponent<RectTransform>();
        float maxX = (spawnArea.rect.width - rt.rect.width) / 2f;
        float maxY = (spawnArea.rect.height - rt.rect.height) / 2f;

        Vector2 randomPos = new Vector2(
            Random.Range(-maxX, maxX),
            Random.Range(-maxY, maxY)
        );

        rt.anchoredPosition = randomPos;

        // Jouer un son aléatoire dans la liste
        if (errorSounds != null && errorSounds.Length > 0)
        {
            AudioClip clip = errorSounds[Random.Range(0, errorSounds.Length)];
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, soundVolume);
        }
    }

    private void StopGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
