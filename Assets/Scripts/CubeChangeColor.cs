using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeChangeColor : MonoBehaviour
{
    public float maxVisualScale;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    private void Start()
    {
        defaultPosition = transform.localPosition;
        defaultRotation = transform.localRotation;
    }

    private void FixedUpdate()
    {
        GetComponent<MeshRenderer>().enabled = SoundVisualize.instance.visualiserVisible;
        maxVisualScale = SoundVisualize.instance.maxVisualScale;

        if (!SoundVisualize.instance.visualizeByRotation)
        {
            transform.localRotation = defaultRotation;
        }

        if(SoundVisualize.instance.Red_Yellow)
        {
            ChangeColor_Red_Yellow();
        }
        if (SoundVisualize.instance.Yellow_Green)
        {
            ChangeColor_Yellow_Green();
        }
        if (SoundVisualize.instance.Red_Green)
        {
            ChangeColor_Red_Green();
        }
        if (SoundVisualize.instance.Rainbow)
        {
            ChangeColor_Rainbow();
        }
    }

    private void ChangeColor_Red_Yellow()
    {
        byte colorR = 255;
        byte colorG = (byte)(transform.localScale.y / (maxVisualScale / 255));
        byte colorB = 0;
        byte colorA = 255;
        
        GetComponent<MeshRenderer>().material.color = new Color32(colorR, colorG, colorB, colorA);
    }

    private void ChangeColor_Yellow_Green()
    {
        byte colorR = (byte)(255 - (transform.localScale.y / (maxVisualScale / 255)));
        byte colorG = 255;
        byte colorB = 0;
        byte colorA = 255;

        Color32 newColor = new Color32(colorR, colorG, colorB, colorA);
        ChangeColor(newColor);
    }

    private void ChangeColor_Red_Green()
    {
        byte colorR = 0;
        byte colorG = 0;
        byte colorB = 0;
        byte colorA = 255;

        if (transform.localScale.y >= 0 && transform.localScale.y < 30)
        {
            colorR = 255;
            colorG = (byte)(transform.localScale.y / (30f / 255f));
        }
        if (transform.localScale.y >= 30 && transform.localScale.y < 60)
        {
            colorR = (byte)(255f - ((transform.localScale.y - 30f) / (30f / 255f)));
            colorG = 255;
        }
        Color32 newColor = new Color32(colorR, colorG, colorB, colorA);
        ChangeColor(newColor);
    }

    private void ChangeColor_Rainbow()
    {
        byte colorR = 0;
        byte colorG = 0;
        byte colorB = 0;
        byte colorA = 255;

        if (transform.localScale.y >= 0 && transform.localScale.y < maxVisualScale / 6f)
        {
            colorR = 255;
            colorG = (byte)(transform.localScale.y / (maxVisualScale / 6f / 255f));
        }
        if (transform.localScale.y >= maxVisualScale / 6f && transform.localScale.y < maxVisualScale / 3f)
        {
            colorR = (byte)(255f - ((transform.localScale.y - maxVisualScale / 6f) / (maxVisualScale / 6f / 255f)));
            colorG = 255;
        }
        if (transform.localScale.y >= maxVisualScale / 3f && transform.localScale.y < maxVisualScale / 2f)
        {
            colorG = 255;
            colorB = (byte)((transform.localScale.y - maxVisualScale / 3f) / (maxVisualScale / 6f / 255f));
        }
        if (transform.localScale.y >= maxVisualScale / 2f && transform.localScale.y < maxVisualScale / 1.5f)
        {
            colorG = (byte)(255 - ((transform.localScale.y - maxVisualScale / 2f) / (maxVisualScale / 6f / 255f)));
            colorB = 255;
        }
        if (transform.localScale.y >= maxVisualScale / 1.5f && transform.localScale.y < maxVisualScale / 1.2f)
        {
            colorR = (byte)((transform.localScale.y - maxVisualScale / 1.5f) / (maxVisualScale / 6f / 255f));
            colorB = 255;
        }
        if (transform.localScale.y >= maxVisualScale / 1.2f && transform.localScale.y < maxVisualScale)
        {
            colorR = 255;
            colorB = (byte)(255 - ((transform.localScale.y - maxVisualScale / 1.2f) / (maxVisualScale / 6f / 255f)));
        }
        Color32 newColor = new Color32(colorR, colorG, colorB, colorA);
        ChangeColor(newColor);
    }

    private void ChangeColor(Color32 newColor)
    {
        GetComponent<MeshRenderer>().material.SetColor("_Color", newColor);
    }
}