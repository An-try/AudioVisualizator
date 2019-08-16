using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public static bool gameIsPaused = false;

    public GameObject _PauseMenu;

    private GameObject PauseCanvas;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PauseCanvas = gameObject;
        _PauseMenu.SetActive(false);
    }

    public void PauseMenuInteraction()
    {
        if (gameIsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        PauseCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        Cursor.visible = true;
        _PauseMenu.SetActive(true);
        Time.timeScale = 0f; // Установить скорость игры на 0 (пауза)
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        _PauseMenu.SetActive(false);
        Time.timeScale = 1f; // Установить скорость игры на 1 (нормальная скорость игры)
        gameIsPaused = false;
    }

    public void ExitGame()
    {
        ResumeGame();
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
