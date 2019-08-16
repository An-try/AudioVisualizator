using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBlock : MonoBehaviour
{
    public char buttonName;

    public GameObject ButtonText;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    private void FixedUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1, transform.localPosition.z);
    }

    public void SetButtonText()
    {
        ButtonText.GetComponent<Text>().text = buttonName.ToString();
    }
}
