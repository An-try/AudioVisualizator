using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public Text DEBUG_TEXT; //

    public GameObject ResolutionDropdown; // Объект выпадающего списка
    public GameObject FullscreenCheckMark;

    public GameObject VolumeSlider;
    private static Slider volumeSlider;
    public AudioMixer AudioMixer;
    private static AudioMixer audioMixer;

    public GameObject ConfirmUpdateSettingsScreen;
    public Text timerText;

    private int[,] Resolutions; // Массив разрешений экрана
    private Dropdown _dropdown;

    private void Awake()
    {

        Resolutions = new int[,]{ { 800, 1024, 1280, 1280, 1280, 1280, 1366, 1600, 1920 },
                                  { 600,  768,  600,  720,  768, 1024,  768,  900, 1080 } };

        volumeSlider = VolumeSlider.GetComponent<Slider>();
        audioMixer = AudioMixer;

        _dropdown = ResolutionDropdown.GetComponent<Dropdown>(); // Выпадающий список
    }

    private void Start()
    {
        volumeSlider.value = GetCurrentAudioVolume();
        FullscreenCheckMark.SetActive(Screen.fullScreen);
        SetStartGameResolution();
        SetCurrentResolutionInDropdown();
        SaveOldSettings();
        ConfirmUpdateSettingsScreen.SetActive(false);
    }

    private void SetStartGameResolution()
    {
        int currentScreenWidth = Screen.currentResolution.width;
        int currentScreenHeight= Screen.currentResolution.height;

        int newScreenWidth = currentScreenWidth;
        int newScreenHeight = currentScreenWidth;

        for (int width = 0; width < Resolutions.Length / 2; width++)
        {
            if (Resolutions[0, width] == currentScreenWidth)
            {
                newScreenWidth = currentScreenWidth;
                break;
            }
            if (Resolutions[0, width] <= currentScreenWidth)
                newScreenWidth = Resolutions[0, width];
        }

        for (int height = 0; height < Resolutions.Length / 2; height++)
        {
            if (Resolutions[1, height] == currentScreenHeight)
            {
                newScreenHeight = currentScreenHeight;
                break;
            }
            if (Resolutions[1, height] <= currentScreenHeight)
                newScreenHeight = Resolutions[1, height];
        }
        Screen.SetResolution(newScreenWidth, newScreenHeight, Screen.fullScreen);
    }

    private void SetCurrentResolutionInDropdown()
    {
        List<Dropdown.OptionData> dropdownOptions = _dropdown.options; // Массив разрешений в выпадающем списке
        for (int i = 0; i < dropdownOptions.Count; i++) // Пройти все разрешения в выпадающем списке
        {
            // Если разрешение в выпадающем списке совпало с текущим разрешением экрана
            if (dropdownOptions[i].text == string.Format(Screen.width + "x" + Screen.height))
            {
                _dropdown.value = i; // Выбрать в выпадающем списке текущее разрешение экрана
            }
        }
    }

    private static float GetCurrentAudioVolume()
    {
        float volumeValue;
        bool valueExist = audioMixer.GetFloat("Volume", out volumeValue); // Если существует параметр "Volume", передать значение громкости в "volume"
        if (valueExist)
            return volumeValue; // Передать громкость в слайдер громкости в настройках
        return 0;
    }

    public void ChangeResolution()
    {
        NewSettings.NewResolution(Resolutions[0, _dropdown.value], Resolutions[1, _dropdown.value]);
    }

    public void ChangeFullscreen()
    {
        FullscreenCheckMark.SetActive(!FullscreenCheckMark.activeSelf);
        NewSettings.NewFullscreen(FullscreenCheckMark.activeSelf);
    }

    public void SetVolume(float volumeValue)
    {
        NewSettings.NewVolume(volumeValue);
    }

    public void ApplySettings()
    {
        UpdateNewSettings();
        ConfirmUpdateSettingsScreen.SetActive(true);
        StartCoroutine("Timer", 10);
    }

    public void ConfirmButtonPressed()
    {
        SaveOldSettings();
        ConfirmUpdateSettingsScreenDeactivate();
    }

    public void CancelButtonPressed()
    {
        UpdateOldSettings();
        ConfirmUpdateSettingsScreenDeactivate();
    }

    private void ConfirmUpdateSettingsScreenDeactivate()
    {
        ConfirmUpdateSettingsScreen.SetActive(false);
        StopCoroutine("Timer");
    }

    private void SaveOldSettings()
    {
        OldSettings.OldResolution(Screen.width, Screen.height);
        OldSettings.OldFullscreen(Screen.fullScreen);
        OldSettings.OldVolume(GetCurrentAudioVolume());
    }

    private void UpdateNewSettings()
    {
        if (Screen.width != NewSettings.resolution[0] || Screen.height != NewSettings.resolution[1])
            Screen.SetResolution(NewSettings.resolution[0], NewSettings.resolution[1], Screen.fullScreen);

        if(Screen.fullScreen != NewSettings.fullscreen)
            Screen.fullScreen = NewSettings.fullscreen;

        if (GetCurrentAudioVolume() != NewSettings.volume)
            audioMixer.SetFloat("Volume", NewSettings.volume);
    }

    private void UpdateOldSettings()
    {
        DEBUG_TEXT.text += "\nupdated old settings";//

        //if (Screen.width != OldSettings.resolution[0] || Screen.height != OldSettings.resolution[1])
        //{
            Screen.SetResolution(OldSettings.resolution[0], OldSettings.resolution[1], Screen.fullScreen);
            SetCurrentResolutionInDropdown();
        //}
        //if (Screen.fullScreen != OldSettings.fullscreen)
        //{
            Screen.fullScreen = OldSettings.fullscreen;
            FullscreenCheckMark.SetActive(OldSettings.fullscreen);
        //}
        //if (GetCurrentAudioVolume() != OldSettings.volume)
        //{
            audioMixer.SetFloat("Volume", OldSettings.volume);
            volumeSlider.value = GetCurrentAudioVolume();
        //}
    }

    private class NewSettings
    {
        public static int[] resolution = new int[2] { Screen.width, Screen.height };
        public static bool fullscreen = Screen.fullScreen;
        public static float volume = GetCurrentAudioVolume();

        public static void NewResolution(int width, int height)
        {
            resolution = new int[2] { width, height };
        }
        public static void NewFullscreen(bool _fullscreen)
        {
            fullscreen = _fullscreen;
        }
        public static void NewVolume(float volumeValue)
        {
            volume = volumeValue;
        }
    }

    private class OldSettings
    {
        public static int[] resolution = new int[2] { Screen.width, Screen.height };
        public static bool fullscreen = Screen.fullScreen;
        public static float volume = GetCurrentAudioVolume();

        public static void OldResolution(int width, int height)
        {
            resolution = new int[2] { width, height };
        }
        public static void OldFullscreen(bool _fullscreen)
        {
            fullscreen = _fullscreen;
        }
        public static void OldVolume(float volumeValue)
        {
            volume = volumeValue;
        }
    }

    IEnumerator Timer(float _time)
    {
        float additionalTime = 0.5f;
        for(float time = _time + additionalTime;    time >= 0;    time -= Time.deltaTime)
        {
            timerText.text = ((int)time).ToString();
            if (time <= additionalTime)
            {
                CancelButtonPressed();
                yield break;
            }
            yield return null;
        }
    }
}