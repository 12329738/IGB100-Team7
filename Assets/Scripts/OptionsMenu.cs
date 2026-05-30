using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [Header("AUDIO")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("DISPLAY")]
    public Toggle fullscreenToggle;

    [Header("OPTIONAL VALUE TEXT")]
    public TextMeshProUGUI masterText;
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI sfxText;

    void Start()
    {
        LoadSettings();

        // Audio listeners
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

        // Display listeners
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        UpdateVolumeTexts();
    }

    // =====================================================
    // AUDIO
    // =====================================================

    public void SetMasterVolume(float value)
    {
        AudioListener.volume = value;

        PlayerPrefs.SetFloat("MasterVolume", value);

        UpdateVolumeTexts();
    }

    public void SetMusicVolume(float value)
    {
        // Placeholder for future mixer integration
        PlayerPrefs.SetFloat("MusicVolume", value);

        UpdateVolumeTexts();
    }

    public void SetSFXVolume(float value)
    {
        // Placeholder for future mixer integration
        PlayerPrefs.SetFloat("SFXVolume", value);

        UpdateVolumeTexts();
    }

    // =====================================================
    // DISPLAY
    // =====================================================

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    // =====================================================
    // LOAD SETTINGS
    // =====================================================

    void LoadSettings()
    {
        float masterVolume =
            PlayerPrefs.GetFloat("MasterVolume", 1f);

        float musicVolume =
            PlayerPrefs.GetFloat("MusicVolume", 1f);

        float sfxVolume =
            PlayerPrefs.GetFloat("SFXVolume", 1f);

        bool fullscreen =
            PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;

        fullscreenToggle.isOn = fullscreen;

        AudioListener.volume = masterVolume;
        Screen.fullScreen = fullscreen;
    }

    // =====================================================
    // UI TEXT
    // =====================================================

    void UpdateVolumeTexts()
    {
        if (masterText != null)
        {
            masterText.text =
                Mathf.RoundToInt(masterVolumeSlider.value * 100) + "%";
        }

        if (musicText != null)
        {
            musicText.text =
                Mathf.RoundToInt(musicVolumeSlider.value * 100) + "%";
        }

        if (sfxText != null)
        {
            sfxText.text =
                Mathf.RoundToInt(sfxVolumeSlider.value * 100) + "%";
        }
    }

    // =====================================================
    // RESET SETTINGS
    // =====================================================

    public void ResetSettings()
    {
        masterVolumeSlider.value = 1f;
        musicVolumeSlider.value = 1f;
        sfxVolumeSlider.value = 1f;

        fullscreenToggle.isOn = true;

        SetMasterVolume(1f);
        SetMusicVolume(1f);
        SetSFXVolume(1f);
        SetFullscreen(true);

        UpdateVolumeTexts();
    }
}