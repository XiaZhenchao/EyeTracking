using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GameObject follow;
    float positionY;
    public GameObject temp;
    Quaternion HeadRotation;
    GameObject[] RightBar;
   
    void Awake()
    {
        positionY = follow.transform.position.y + 0.1f;
    }

    void Update()
    {
        RightBar = GameObject.FindGameObjectsWithTag("RightBar");
        //HeadRotation = follow.transform.rotation;
        //HeadRotation = temp.arFace.leftEye.transform.rotation;
        //gameObject.transform.position = new Vector3(-follow.transform.position.x, positionY, -follow.transform.position.z);
        Debug.Log("gameObjectposition: " +gameObject.transform.position);
        //var rotationVector = follow.transform.rotation.eulerAngles;
        //rotationVector.z = 0f;
        //rotationVector.x = 0f;
        //gameObject.transform.Rotate(0,rotationVector.y,0);
        //HeadRotation.x = -follow.transform.rotation.x;
        //HeadRotation.z = -follow.transform.rotation.z;
        gameObject.transform.rotation = HeadRotation;
        //rotationVector.y = 0f;
        foreach (GameObject bar in RightBar)
        {
            if (bar.transform.position.z == gameObject.transform.position.z
                && (gameObject.transform.position.x - bar.transform.position.x) < 1)
            {
                bar.GetComponent<Renderer>().material.color = Color.red;
            }
        }

    }       
}
