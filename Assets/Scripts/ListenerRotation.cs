using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.UI;

public class ListenerRotation : MonoBehaviour
{
    public Text HeadRotationUI;
    public GameObject ARCamera;
    public List<double> yRotList = new List<double>();
    public List<double> xPosList = new List<double>();
    public List<double> zPosList = new List<double>();
    private float tempXpos = 0.0f;
    private float tempZpos = 0.0f;
    private float tempYrot = 0.0f;
    public float leftAddedAngle = 20.0f;
    public float rightAddedAngle = 30.0f;
    public GameObject head;
    public GameObject groundVision;
    public GameObject body;


    public GameObject myAvatar;
    public ARFaceManager ArFaceManager;




    public void pushToArray(float yRot, float xPos, float zPos)
    {
        yRotList.RemoveAt(0);
        xPosList.RemoveAt(0);
        zPosList.RemoveAt(0);
        yRotList.Add(yRot);
        xPosList.Add(xPos);
        zPosList.Add(zPos);

    }


    public float getTroughXpos()
    {
        return (float)xPosList[1];
    }

    public float getTroughZpos()
    {
        return (float)zPosList[1];
    }

    private bool isInitialized = false;

    public float headsAngle(float x1, float y1, float x2, float y2)
    {
        float result = 0.0f;
        if (!isInitialized)
        {
            head.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            isInitialized = true;
            return 0.0f;
        }

        float deltaX = x2 - x1;
        float deltaY = y2 - y1;
        Debug.Log("-------" + x1 + " " + y1 + " " + x2 + " " + y2 + " dx " + deltaX + "dy" + deltaY);

        if (x2 >= x1 && y2 >= y1) //Q1
        {
            result = 90 - Mathf.Atan2(deltaY, deltaX) * (180 / Mathf.PI);
        }
        else if (x2 <= x1 && y2 >= y1) //Q2
        {
            result = -1 * (Mathf.Atan2(deltaY, deltaX) * (180 / Mathf.PI) - 90);
        }
        else if (x2 <= x1 && y2 <= y1) //Q3
        {
            result = -1 * (Mathf.Atan2(deltaY, deltaX) * (180 / Mathf.PI) + 270);
        }
        else if (x2 >= x1 && y2 <= y1) //Q4
        {
            result = Mathf.Abs(Mathf.Atan2(deltaY, deltaX) * (180 / Mathf.PI) + 90);
        }

        head.transform.rotation = Quaternion.Euler(new Vector3(0, result, 0));
        HeadRotationUI.text = "Head Angle: " + Mathf.Round(result).ToString();

        return result;
    }
        
    /* head angle plan b*/

    public void headAnglePlanB(float result)
    {   
        Debug.Log("this is the value " + result);
        head.transform.rotation = Quaternion.Euler(new Vector3(0, result, 0));
    }

    /* head angle plan c*/

    public void headAnglePlanC(float resultX,float resultY)
    {
        Debug.Log("this is the head angle " + resultY);
        head.transform.rotation = Quaternion.Euler(new Vector3(0, resultY, 0)); // x rotation add here



    }

}
