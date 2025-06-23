using UnityEngine;

public class LevelUpSound : MonoBehaviour
{
    public AudioClip levelUpSound;
    public AudioSource audioSource;
    void Start()
    {
        int exp = PlayerPrefs.GetInt("PlayerExp", 0);
        int level = StaticPlayerInfo.ExpToLevel(exp);
        if (StaticPlayerInfo.lastLevel == -1)
        {
            StaticPlayerInfo.lastLevel = level;
        }

        if (level > StaticPlayerInfo.lastLevel)
        {
            Debug.Log("Level up! New level: " + level);
            audioSource.PlayOneShot(levelUpSound);
            StaticPlayerInfo.lastLevel = level;
        }
    }
}
