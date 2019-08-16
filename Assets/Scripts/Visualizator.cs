using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Visualizator : MonoBehaviour
{
    public static Visualizator instance;

    public string folderPath;
    
    public AudioSource audioSource;

    public GameObject HelpText;
    public GameObject InfoWindow;

    public GameObject Menues;
    public GameObject MediaMenu;
    public GameObject BackgroundsMenu;

    public GameObject AnimeBackground;
    public GameObject Background;
    public Sprite defaultBackgroundSprite;
    private Texture2D texture2DForBackground = null;
    private Sprite spriteForBackground = null;

    public GameObject MediaIconPrefab;
    public GameObject BackgroundIconPrefab;

    private List<string> songsPaths = new List<string>();
    public string currentSongPath = null;

    private List<string> backgroundsPaths = new List<string>();
    public string currentBackgroundPath = null;

    public bool backgroundDeletedFirstTime = false;

    public bool helpTextShow = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InfoWindow.SetActive(false);
        
        audioSource = GetComponent<AudioSource>();

        Menues.SetActive(false);
        
        Destroy(audioSource.clip);

        Background.GetComponent<Image>().sprite = defaultBackgroundSprite;
        for (int i = 0; i < Background.transform.childCount; i++)
        {
            Background.transform.GetChild(i).GetComponent<Image>().sprite = defaultBackgroundSprite;
        }
    }

    private void Update()
    {
        HelpTextShowHide();
    }

    private bool FolderExist()
    {
        if (!Directory.Exists(folderPath))
        {
            InfoWindowShow
                ("<color=red>Directory does not exist.</color> <color=blue>Example: </color><color=magenta>C:/Users/admin/Desktop/Music</color>", 3f);
            return false;
        }
        else
        {
            InfoWindowShow("<color=green>Loading...</color>", 1f);
            return true;
        }
    }

    public void InfoWindowShow(string message, float liveTime)
    {
        InfoWindow.SetActive(true);
        InfoWindow.GetComponent<Text>().text = message;
        Invoke("DirectoryDoesNotExistDeactivate", liveTime);
    }

    private void DirectoryDoesNotExistDeactivate()
    {
        InfoWindow.SetActive(false);
    }

    public void LoadMusicFromFolder()
    {
        if (!FolderExist())
        {
            return;
        }

        foreach (string songPath in Directory.GetFiles(folderPath, "*.wav", SearchOption.TopDirectoryOnly))
        {
            CreateMusicIcon(songPath);
        }
        foreach (string songPath in Directory.GetFiles(folderPath, "*.ogg", SearchOption.TopDirectoryOnly))
        {
            CreateMusicIcon(songPath);
        }
        foreach (string songPath in Directory.GetFiles(folderPath, "*.mp3", SearchOption.TopDirectoryOnly))
        {
            CreateMusicIcon(songPath);
        }
    }

    public void LoadBackgroundsFromFolder()
    {
        if (!FolderExist())
        {
            return;
        }
        // Create new backgrounds list and icons
        foreach (string imagePath in Directory.GetFiles(folderPath, "*.png", SearchOption.TopDirectoryOnly))
        {
            CreateBackgroundIcon(imagePath);
        }
        foreach (string imagePath in Directory.GetFiles(folderPath, "*.jpg", SearchOption.TopDirectoryOnly))
        {
            CreateBackgroundIcon(imagePath);
        }
        foreach (string imagePath in Directory.GetFiles(folderPath, "*.jpeg", SearchOption.TopDirectoryOnly))
        {
            CreateBackgroundIcon(imagePath);
        }
    }

    private void CreateMusicIcon(string songPath)
    {
        if (!songsPaths.Contains(songPath))
        {
            songsPaths.Add(songPath);
            GameObject musicMenu = Instantiate(MediaIconPrefab, MediaMenu.transform);
            musicMenu.GetComponent<MusicIcon>().songPath = songPath;
            musicMenu.GetComponentInChildren<Text>().text = (songPath + "<color=red> Video</color>").Remove(0, folderPath.Length + 1);
        }
    }

    private void CreateBackgroundIcon(string imagePath)
    {
        if (!backgroundsPaths.Contains(imagePath))
        {
            backgroundsPaths.Add(imagePath);
            GameObject backgroundIcon = Instantiate(BackgroundIconPrefab, BackgroundsMenu.transform);
            backgroundIcon.GetComponent<BackgroundIcon>().backgroundPath = imagePath;
            backgroundIcon.GetComponentInChildren<Text>().text = imagePath.Remove(0, folderPath.Length + 1);
        }
    }

    public IEnumerator SetSong(string songPath)
    {
        using (var www = new WWW(songPath))
        {
            yield return www;

            if (songPath.Substring(songPath.Length - 4) != ".mp3")
            {
                audioSource.clip = www.GetAudioClip();
            }
            else
            {
                audioSource.clip = NAudioPlayer.FromMp3Data(www.bytes);
            }
        }
    }

    public void SetBackground(string imagePath)
    {
        Destroy(texture2DForBackground);
        Destroy(spriteForBackground);

        texture2DForBackground = new Texture2D(2, 2);
        texture2DForBackground.LoadImage(File.ReadAllBytes(imagePath));

        spriteForBackground =
            Sprite.Create(texture2DForBackground, new Rect(0f, 0f, texture2DForBackground.width, texture2DForBackground.height), new Vector2(0.5f, 0.5f));

        Background.GetComponent<Image>().sprite = spriteForBackground;
        for (int i = 0; i < Background.transform.childCount; i++)
        {
            Background.transform.GetChild(i).GetComponent<Image>().sprite = spriteForBackground;
        }
    }

    private void HelpTextShowHide()
    {
        if (helpTextShow)
        {
            HelpText.SetActive(true);
        }
        else
        {
            HelpText.SetActive(false);
        }
    }

    // Set path
    public void Path(string value)
    {
        if (value != null) folderPath = value;
        // If save.txt does not exist, it will be created and folder path will be writted
        // If save.txt is already exist, it will be overwrited by folder path
        File.WriteAllText(Manager.instance.folderPathFileName, folderPath);
    }
    // Set values
    public void PositionVisualizingIntencity(string value)
    {
        if (value != null) SoundVisualize.instance.positionVisualizingIntencity = float.Parse(value);
    }
    public void VisualModifier(string value)
    {
        if (value != null) SoundVisualize.instance.visualModifier = float.Parse(value);
    }
    public void SmoothSpeed(string value)
    {
        if (value != null) SoundVisualize.instance.smoothSpeed = float.Parse(value);
    }
    public void MaxVisualScale(string value)
    {
        if (value != null) SoundVisualize.instance.maxVisualScale = float.Parse(value);
    }
    public void MaxPositionVisualizing(string value)
    {
        if (value != null) SoundVisualize.instance.maxPositionVisualizing = float.Parse(value);
    }
    public void VisualCubeDepth(string value)
    {
        if (value != null) SoundVisualize.instance.visualCubeDefaultScale.z = float.Parse(value);
    }
    public void CircleRotationMultiply(string value)
    {
        if (value != null) SoundVisualize.instance.circleRotationMultiply = float.Parse(value);
    }
    public void CircleBigRotationMultiply(string value)
    {
        if (value != null) SoundVisualize.instance.circleBigRotationMultiply = float.Parse(value);
    }
    public void CircleBigRotationTime(string value)
    {
        if (value != null) SoundVisualize.instance.circleBigRotationTime = float.Parse(value);
    }
    // Set check marks
    public void VisualiserVisible(bool value)
    {
        SoundVisualize.instance.visualiserVisible = value;
    }
    public void VisualizeByCircleRotation(bool value)
    {
        SoundVisualize.instance.visualizeByCircleRotation = value;
    }
    public void VisualizeByCircleBigRotation(bool value)
    {
        SoundVisualize.instance.visualizeByCircleBigRotation = value;
    }
    public void VisualizeByPosition(bool value)
    {
        SoundVisualize.instance.visualizeByPosition = value;
    }
    public void VisualizeByRotation(bool value)
    {
        SoundVisualize.instance.visualizeByRotation = value;
    }
    public void VisualizeByScale(bool value)
    {
        SoundVisualize.instance.visualizeByScale = value;
    }
    public void VisualizeByBackground(bool value)
    {
        SoundVisualize.instance.visualizeByBackground = value;
    }
    public void Particles(bool value)
    {
        SoundVisualize.instance.visualizeByParticles = value;
    }
    public void ParticlesRainbowColor(bool value)
    {
        SoundVisualize.instance.particlesRainbowColor = value;
    }
    public void BloomEffect(bool value)
    {
        Manager.instance.MusicVisualiseCamera.GetComponent<UnityStandardAssets.ImageEffects.Bloom>().enabled = value;
    }
    // Background
    public void BackgroundOnOff(bool value)
    {
        Background.SetActive(value);
    }
    // Destroy icons
    public void DestroyAllMusicIcons(GameObject musicIcon)
    {
        //for (int i = 0; i < MediaMenu.transform.childCount; i++)
        //{
        //    Destroy(MediaMenu.transform.GetChild(i).GetComponent<MusicIcon>().audioClip);
        //    Destroy(MediaMenu.transform.GetChild(i).gameObject);
        //}
    }
    public void DestroyAllBackgroundIcon(GameObject backgroundIcon)
    {
        // Delete backgrounds list and icons
        //for (int i = 0; i < BackgroundsMenu.transform.childCount; i++)
        //{
        //    BackgroundsMenu.transform.GetChild(i).GetComponent<Image>().sprite = null;
        //    Destroy(BackgroundsMenu.transform.GetChild(i).GetComponent<Image>().sprite);
        //    Destroy(BackgroundsMenu.transform.GetChild(i).gameObject);
        //}
    }
}