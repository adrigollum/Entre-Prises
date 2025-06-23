using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WindowsErrorSpawner : MonoBehaviour
{
    [Header("Fenêtres à faire pop")]
    [SerializeField] private GameObject[] windowPrefabs; // Liste de prefabs (variantes de fenêtres)

    [Header("Zone de spawn (Canvas World Space)")]
    [SerializeField] private RectTransform spawnArea; // Le RectTransform du World Space Canvas

    [Header("Options de spawn")]
    [SerializeField] private int numberToSpawn = 10;
    [SerializeField] private float delayBetweenSpawns = 0.2f;

    [Header("Son d'erreur")]
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private float soundVolume = 1f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private System.Collections.IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            SpawnRandomWindow();
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
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

        // Jouer un son à la position de la caméra
        if (errorSound != null)
            AudioSource.PlayClipAtPoint(errorSound, Camera.main.transform.position, soundVolume);
    }
}
