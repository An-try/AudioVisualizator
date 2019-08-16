using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicIcon : MonoBehaviour
{
    public string songPath = null;

    private Color defaultColor;

    private void Start()
    {
        defaultColor = GetComponent<Image>().color;
    }

    private void FixedUpdate()
    {
        if (Visualizator.instance.currentSongPath != songPath)
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
        if (File.Exists(songPath))
        {
            if (Visualizator.instance.currentSongPath != songPath)
            {
                DestroyCurrentMusic();
                SetNewMusic();
            }
            else
            {
                DestroyCurrentMusic();
            }
        }
        else
        {
            Visualizator.instance.InfoWindowShow("<color=red>This track has been deleted</color>", 3f);
            Destroy(gameObject);
        }
    }

    private void SetNewMusic()
    {
        Visualizator.instance.currentSongPath = songPath;
        StartCoroutine(Visualizator.instance.SetSong(songPath));
        Visualizator.instance.InfoWindowShow("<color=green>Loading...</color>", 0.2f);
    }

    private void DestroyCurrentMusic()
    {
        Visualizator.instance.currentSongPath = null;
        Destroy(Visualizator.instance.audioSource.clip);
        Visualizator.instance.audioSource.Stop();
        Visualizator.instance.audioSource.clip = null;
    }
}