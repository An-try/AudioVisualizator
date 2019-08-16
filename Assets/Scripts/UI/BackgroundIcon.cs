using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundIcon : MonoBehaviour
{
    public string backgroundPath = null;

    private Color defaultColor;

    private void Start()
    {
        defaultColor = GetComponent<Image>().color;
    }

    private void FixedUpdate()
    {
        if (Visualizator.instance.currentBackgroundPath != backgroundPath)
        {
            GetComponent<Image>().color = defaultColor;
        }
        else
        {
            GetComponent<Image>().color = Color.gray;
        }
    }

    private void OnMouseUp()
    {
        if (File.Exists(backgroundPath))
        {
            if (Visualizator.instance.currentBackgroundPath != backgroundPath)
            {
                DestroyCurrentBackground();
                SetNewBackground();
            }
        }
        else
        {
            Visualizator.instance.InfoWindowShow("<color=red>This background has been deleted</color>", 3f);
            Destroy(gameObject);
        }
    }

    private void SetNewBackground()
    {
        Visualizator.instance.currentBackgroundPath = backgroundPath;
        Visualizator.instance.SetBackground(backgroundPath);
        Visualizator.instance.InfoWindowShow("<color=green>Loading...</color>", 0.2f);
    }

    private void DestroyCurrentBackground()
    {
        Visualizator.instance.currentBackgroundPath = null;
        if (Visualizator.instance.backgroundDeletedFirstTime)
        {
            DestroyImmediate(Visualizator.instance.Background.GetComponent<Image>().sprite, true);
        }
        Visualizator.instance.backgroundDeletedFirstTime = true;
        Visualizator.instance.Background.GetComponent<Image>().sprite = null;
    }
}