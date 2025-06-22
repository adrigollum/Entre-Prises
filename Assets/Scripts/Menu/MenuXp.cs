using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuXp : MonoBehaviour
{
    public Slider xpSlider;
    public TextMeshProUGUI lvlText;

    void Start()
    {
        int xp = PlayerPrefs.GetInt("PlayerExp", 0);
        int level = StaticPlayerInfo.ExpToLevel(xp);
        int nextXpStep = StaticPlayerInfo.GetNextXpStepForUI(xp);
        int currentXp = StaticPlayerInfo.ClampExp(xp);

        xpSlider.maxValue = nextXpStep;
        xpSlider.minValue = 0;
        xpSlider.value = currentXp;

        lvlText.text = level.ToString();
    }
}
