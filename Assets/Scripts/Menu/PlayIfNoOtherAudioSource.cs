using UnityEngine;

public class PlayIfNoOtherAudioSource : MonoBehaviour
{
    public string targetAudioSourceName = "GlobalMusicSource";
    private AudioSource localAudioSource;

    void Awake()
    {
        localAudioSource = GetComponent<AudioSource>();

        if (localAudioSource == null)
        {
            Debug.LogError("Aucun AudioSource trouvé sur cet objet.");
            return;
        }

        // ✅ Utilisation de la nouvelle API (Unity 2023+)
        AudioSource[] allSources = Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource source in allSources)
        {
            if (source != localAudioSource && source.gameObject.name == targetAudioSourceName)
            {
                Debug.Log($"Un AudioSource nommé '{targetAudioSourceName}' existe déjà, on ne joue pas celui-ci.");
                return;
            }
        }

        // Aucun AudioSource avec ce nom trouvé → on joue le nôtre
        localAudioSource.Play();
        Debug.Log("Aucun AudioSource existant détecté — lancement de la musique locale.");
    }
}
