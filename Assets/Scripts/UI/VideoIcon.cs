using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoIcon : MonoBehaviour
{
    public VideoClip videoClip;

    private Color defaultColor;

    private void Start()
    {
        defaultColor = GetComponent<Image>().color;
    }

    private void FixedUpdate()
    {
        if (Visualizator.instance.gameObject.GetComponent<VideoPlayer>().clip != videoClip)
        {
            GetComponent<Image>().color = defaultColor;
        }
        else
        {
            GetComponent<Image>().color = Color.gray;
        }
    }

    private void OnMouseDown()
    {
        if (Visualizator.instance.gameObject.GetComponent<VideoPlayer>().clip != videoClip)
        {
            Visualizator.instance.gameObject.GetComponent<AudioSource>().Stop();
            Visualizator.instance.gameObject.GetComponent<AudioSource>().clip = null;
            
            Visualizator.instance.gameObject.GetComponent<VideoPlayer>().clip = videoClip;
            Visualizator.instance.gameObject.GetComponent<VideoPlayer>().Play();
        }
        else
        {
            Visualizator.instance.gameObject.GetComponent<VideoPlayer>().Stop();
            Visualizator.instance.gameObject.GetComponent<VideoPlayer>().clip = null;
        }
    }
}