using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float minShakeSpeed = 0.02f;
    public float maxShakeSpeed = 0.04f;
    public float maxShakeAngle = 3f;

    public float clampShakeRotation = 0.02f;

    private bool changeRotationDirection = false;

    private void Start()
    {
        switch (Random.Range(0, 2))
        {
            case 0:
                changeRotationDirection = true;
                break;
            case 1:
                changeRotationDirection = false;
                break;
            default:
                changeRotationDirection = false;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (!changeRotationDirection)
        {
            Quaternion newRotation = Quaternion.Euler(0f, 0f, maxShakeAngle);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, newRotation, GetRandomShakeSpeed());

            if (transform.localRotation.z >= clampShakeRotation)
            {
                changeRotationDirection = true;
            }
        }
        else
        {
            Quaternion newRotation = Quaternion.Euler(0f, 0f, -maxShakeAngle);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, newRotation, GetRandomShakeSpeed());

            if (transform.localRotation.z <= -clampShakeRotation)
            {
                changeRotationDirection = false;
            }
        }
    }

    private float GetRandomShakeSpeed()
    {
        return Random.Range(minShakeSpeed, maxShakeSpeed);
    }
}
