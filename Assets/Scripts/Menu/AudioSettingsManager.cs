using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Références")]
    public AudioMixer audioMixer;
    public Slider volumeSliderMaster;
    public Slider volumeSliderMusic;
    public Slider volumeSliderSFX;

    [Header("Paramètres exposés")]
    public string exposedParamMaster = "MasterVolume";
    public string exposedParamMusic = "MusicVolume";
    public string exposedParamSFX = "SFXVolume";

    [Header("Paramètres Master")]
    [Range(0f, 1f)] public float defaultMaster = 1f;

    [Header("Paramètres Music")]
    [Range(0f, 1f)] public float defaultMusic = 1f;

    [Header("Paramètres SFX")]
    [Range(0f, 1f)] public float defaultSFX = 1f;

    private void Start()
    {
        SetupSlider(volumeSliderMaster, exposedParamMaster, defaultMaster);
        SetupSlider(volumeSliderMusic, exposedParamMusic, defaultMusic);
        SetupSlider(volumeSliderSFX, exposedParamSFX, defaultSFX);
    }

    private void SetupSlider(Slider slider, string exposedParam, float defaultVal)
    {
        slider.minValue = 0f;
        slider.maxValue = defaultVal * 2f;

        float currentVolumeDb;
        if (audioMixer.GetFloat(exposedParam, out currentVolumeDb))
        {
            float currentLinear = Mathf.Pow(10, currentVolumeDb / 20);
            slider.value = Mathf.Clamp(currentLinear, slider.minValue, slider.maxValue);
        }
        else
        {
            slider.value = defaultVal;
            SetVolume(exposedParam, defaultVal);
        }
    }

    public void SetMasterVolume(float volume)
    {
        SetVolume(exposedParamMaster, volume);
    }

    public void SetMusicVolume(float volume)
    {
        SetVolume(exposedParamMusic, volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume(exposedParamSFX, volume);
    }

    private void SetVolume(string exposedParam, float volume)
    {
        // Clamp volume min 0.0001f pour éviter log(0)
        float clampedVolume = Mathf.Clamp(volume, 0.0001f, sliderMax(exposedParam));
        float dB = Mathf.Log10(clampedVolume) * 20f;
        audioMixer.SetFloat(exposedParam, dB);
    }

    // Pour récupérer le max du slider associé au param exposé
    private float sliderMax(string exposedParam)
    {
        if (exposedParam == exposedParamMaster)
            return defaultMaster * 2f;
        else if (exposedParam == exposedParamMusic)
            return defaultMusic * 2f;
        else if (exposedParam == exposedParamSFX)
            return defaultSFX * 2f;
        return 1f;
    }
}
