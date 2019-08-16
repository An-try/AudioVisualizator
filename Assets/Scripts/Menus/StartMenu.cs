using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject OptionsMenu;

    public GameObject LoadingInfo;
    public GameObject LoadingProgressText;
    public Slider loadingProgressBar;

    void Awake()
    {
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        LoadingInfo.SetActive(false);
    }

    void Start()
    {
        Cursor.visible = true;
    }

    public void PlayButtonExecute() // Нажата кнопка начала игры
    {
        MainMenu.SetActive(false); // Деактивировать главное меню
        LoadGameScene("Visualizator"); // Загрузить сцену игры
    }
    public void OptionsButtonExecute() // Нажата кнопка настроек
    {
        OptionsMenu.SetActive(true); // Активировать окно настроек
        MainMenu.SetActive(false); // Деактивировать главное меню
    }
    public void CloseButtonExecute() // Нажата кнопка закрытия настроек
    {
        MainMenu.SetActive(true); // Активировать главное меню
        OptionsMenu.SetActive(false); // Деактивировать окно настроек
    }
    public void ExitButtonExecute() // Нажата кнопка выхода из приложения
    {
        Application.Quit(); // Выход из приложения
    }

    void LoadGameScene(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        // Загрузить сцену игры
        // LoadSceneAsync - Загрузить новую сцену в фоне, не закрывая текущую сцену
        // AsyncOperation - Возвращает объект с информацией о прогрессе загрузки новой сцены
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        LoadingInfo.SetActive(true);

        while(!operation.isDone) // Если сцена грузится
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingProgressBar.value = progress;
            LoadingProgressText.GetComponent<Text>().text = (int)(progress * 100) + "%";

            if (progress == 1)
                LoadingProgressText.GetComponent<Text>().text = "Loading...";

            yield return null;
        }
        yield break;
    }
}