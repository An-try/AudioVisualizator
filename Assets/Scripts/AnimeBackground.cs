using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimeBackground : MonoBehaviour
{
    public GameObject Sun;

    public GameObject EyeRight;
    public GameObject EyeLeft;

    public GameObject Mouth;
    public bool changeScaleDirection = false;

    private void Start()
    {
        StartCoroutine(CloseOpenEyes());
    }

    private void FixedUpdate()
    {
        byte A = (byte)(-SoundVisualize.instance.backgroundNewRotation.z / SoundVisualize.instance.maxBackgroundNewRotation * 254);
        Sun.GetComponent<Image>().color = new Color32(255, 255, 255, A);

        if (!changeScaleDirection)
        {
            Mouth.transform.localScale =
                new Vector3(Mouth.transform.localScale.x, Mouth.transform.localScale.y - 0.003f, Mouth.transform.localScale.z);

            if (Mouth.transform.localScale.y <= 0.9f)
            {
                changeScaleDirection = true;
            }
        }
        else
        {
            Mouth.transform.localScale =
                new Vector3(Mouth.transform.localScale.x, Mouth.transform.localScale.y + 0.003f, Mouth.transform.localScale.z);

            if (Mouth.transform.localScale.y >= 1f)
            {
                changeScaleDirection = false;
            }
        }
    }

    private IEnumerator CloseOpenEyes()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 11));

            EyeRight.transform.localScale = new Vector3(EyeRight.transform.localScale.x, 0.1f, EyeRight.transform.localScale.z);
            EyeLeft.transform.localScale = new Vector3(EyeLeft.transform.localScale.x, 0.1f, EyeLeft.transform.localScale.z);

            yield return new WaitForSeconds(0.05f);

            EyeRight.transform.localScale = new Vector3(EyeRight.transform.localScale.x, 1f, EyeRight.transform.localScale.z);
            EyeLeft.transform.localScale = new Vector3(EyeLeft.transform.localScale.x, 1f, EyeLeft.transform.localScale.z);

        }
    }
}
