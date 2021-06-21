using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCane : MonoBehaviour
{
    private Gyroscope gyro;
    private Quaternion rotation;
    private bool gyroActive;

    //aLREADY ACTIVATED
    public void EnableGyro()
    {
        if (gyroActive)
        {
            return;
        }
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
    }
    private void Update()
    {
        if (gyroActive)
        {
            rotation = gyro.attitude;
            gameObject.transform.rotation = rotation;
            Debug.Log("Rotation: "+ rotation);
        }

    }
}
