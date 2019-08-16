using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public GameObject DirectionalLight;

    public GameObject FPSInfo;
    
    public GameObject MusicVisualiseCamera;
    public GameObject MusicVisualizer;

    public GameObject FolderPath;

    public Material visualiseCubeMaterial;

    public Material[] RainbowMaterials;

    public readonly string folderPathFileName = "FolderPathSave.txt";
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // If save.txt does not exist, it will be created and folder path will be writted
        if (!File.Exists(folderPathFileName))
        {
            File.WriteAllText(folderPathFileName, Visualizator.instance.folderPath);
        }
        else // If save.txt is already exist, overwrite current folder path
        {
            Visualizator.instance.folderPath = File.ReadAllText(folderPathFileName);
            FolderPath.GetComponent<InputField>().text = Visualizator.instance.folderPath;
        }

        InvokeRepeating("UpdateCurrentFPSInfo", 1f, 0.5f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            Visualizator.instance.Menues.SetActive(!Visualizator.instance.Menues.activeSelf);
            Visualizator.instance.helpTextShow = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.instance.PauseMenuInteraction();
        }

        Cursor.visible = Visualizator.instance.Menues.activeSelf;

        if (PauseMenu.instance._PauseMenu.activeSelf)
        {
            Cursor.visible = true;
        }
    }

    private void UpdateCurrentFPSInfo()
    {
        FPSInfo.GetComponent<Text>().text = "<color=magenta>FPS: " + (int)(1f / Time.deltaTime);
    }

    public void DEACTIVATE_BUTTON(GameObject Button)
    {
        Button.SetActive(false);
    }

    public void ACTIVATE_TEXT(GameObject InfoText)
    {
        InfoText.SetActive(true);
    }
}