using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class PersistentMusic : MonoBehaviour
{
    public AudioSource audioSource;                        // AudioSource assigné dans l’inspecteur
    public string[] scenesWhereMusicPersists;              // Liste des scènes où la musique reste
    public string exposedVolumeParam = "MusicVolume";      // Nom exposé dans le mixer

    private static PersistentMusic instance;

    void Start()
    {
        // Singleton temporaire
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
        {
            Debug.LogError("AudioSource non assigné !");
            return;
        }

        // Lire le niveau du paramètre exposé du AudioMixer
        if (audioSource.outputAudioMixerGroup != null)
        {
            AudioMixer mixer = audioSource.outputAudioMixerGroup.audioMixer;

            if (mixer.GetFloat(exposedVolumeParam, out float volumeDb))
            {
                Debug.Log($"[Mixer] {exposedVolumeParam} = {volumeDb} dB");
                float volumeLinear = Mathf.Pow(10f, volumeDb / 20f);
                Debug.Log($"[Mixer] Volume linéaire : {volumeLinear}");
            }
            else
            {
                Debug.LogWarning($"Le paramètre '{exposedVolumeParam}' n'est pas exposé dans le mixer.");
            }
        }

        // Jouer la musique si elle ne l’est pas déjà
        if (!audioSource.isPlaying)
            audioSource.Play();

        // Écoute de chargement de scène
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Si la nouvelle scène n’est pas dans la liste → détruire le GameObject
        bool allowed = false;
        foreach (var sceneName in scenesWhereMusicPersists)
        {
            if (scene.name == sceneName)
            {
                allowed = true;
                break;
            }
        }

        if (!allowed)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
    }
}