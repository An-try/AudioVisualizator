using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBox : MonoBehaviour
{
    public Sprite checkMark;

    private void Start()
    {
        SetCheckMark();
    }

    private void OnMouseUp()
    {
        Invoke("SetCheckMark", 0.1f);
    }

    private void SetCheckMark()
    {
        if (GetComponent<Toggle>().isOn)
        {
            GetComponent<Image>().sprite = checkMark;
        }
        else
        {
            GetComponent<Image>().sprite = null;
        }
    }
}