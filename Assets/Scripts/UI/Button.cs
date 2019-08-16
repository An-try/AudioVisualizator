using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerUpHandler,
                                     IPointerDownHandler,
                                     IPointerEnterHandler,
                                     IPointerExitHandler
{
    Text buttonText;
    Image buttonSprite;

    Color32 neutralButtonColor;
    Color32 activeButtonColor;

    private Vector3 defaulButtontScale;
    private float buttonPressedScaleMultiplier = 0.9f;

    private bool buttonPressed = false;

    void Awake()
    {
        neutralButtonColor = new Color32(255, 255, 255, 127);// 255, 255, 255, 127
        activeButtonColor = new Color32(255, 255, 255, 255);//255, 255, 255, 255

        defaulButtontScale = transform.localScale;

        if (transform.GetComponent<Text>() != null)
        {
            buttonText = transform.GetComponent<Text>();
            buttonText.color = neutralButtonColor;
        }
        if (transform.GetComponent<Image>() != null)
        {
            buttonSprite = transform.GetComponent<Image>();
            buttonSprite.color = neutralButtonColor;
        }
        InvokeRepeating("UpdateButton", 1f, 1f);
    }

    private void UpdateButton()
    {
        if (!gameObject.activeInHierarchy)
        {
            SetButtonColor(neutralButtonColor);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetButtonColor(activeButtonColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!buttonPressed)
        {
            SetButtonColor(neutralButtonColor);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetButtonColor(neutralButtonColor);
        SetButtonDefaultScale();
        buttonPressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetButtonPressedScale();
        buttonPressed = true;
    }

    private void SetButtonDefaultScale()
    {
        transform.localScale = defaulButtontScale;
    }

    private void SetButtonPressedScale()
    {
        transform.localScale = new Vector3(transform.localScale.x * buttonPressedScaleMultiplier,
                                           transform.localScale.y * buttonPressedScaleMultiplier,
                                           transform.localScale.z * buttonPressedScaleMultiplier);
    }

    private void SetButtonColor(Color32 newColor)
    {
        if (buttonText != null)
        {
            buttonText.color = newColor;
        }
        if (buttonSprite != null)
        {
            buttonSprite.color = newColor;
        }
    }
}