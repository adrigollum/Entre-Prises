using UnityEngine;
using System.Collections.Generic;

public class CardSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [System.Serializable]
    public class CardTypeSound
    {
        public EnumCardType.CardType type;
        public List<AudioClip> clips;
    }

    [SerializeField] private List<CardTypeSound> cardTypeSounds;

    private Dictionary<EnumCardType.CardType, List<AudioClip>> typeToClips;

    private void Awake()
    {
        typeToClips = new Dictionary<EnumCardType.CardType, List<AudioClip>>();

        foreach (var pair in cardTypeSounds)
        {
            if (!typeToClips.ContainsKey(pair.type))
                typeToClips[pair.type] = pair.clips;
        }
    }

    public void PlaySoundForType(EnumCardType.CardType type)
    {
        if (typeToClips.TryGetValue(type, out List<AudioClip> clips) && clips != null && clips.Count > 0)
        {
            int randomIndex = Random.Range(0, clips.Count);
            AudioClip randomClip = clips[randomIndex];
            audioSource.PlayOneShot(randomClip);
        }
        else
        {
            Debug.LogWarning($"Aucun son disponible pour le type {type}");
        }
    }
}
