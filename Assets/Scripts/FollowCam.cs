using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GameObject follow;
    float positionY;

    void Awake()
    {
        positionY = follow.transform.position.y + 0.1f;
    }

    void Update()
    {
        gameObject.transform.position = new Vector3(follow.transform.position.x, positionY, follow.transform.position.z);
        Debug.Log("gameObjectposition: " +gameObject.transform.position);
        var rotationVector = follow.transform.rotation.eulerAngles;
        rotationVector.z = 0f;
        rotationVector.x = 0f;
        //gameObject.transform.Rotate(0,rotationVector.y,0);
        gameObject.transform.rotation = follow.transform.rotation;
        rotationVector.y = 0f;
        Debug.Log("rotationVector: "+ rotationVector);
        Debug.Log("rotationVector.x: ");
        Debug.Log("rotationVector.y: ");
        Debug.Log("rotationVector.z: ");
    }       
}
