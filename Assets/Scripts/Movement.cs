using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotSpeed = 1f;
    //public GameObject Cane; 

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(transform.position, new Vector3(0f, 1f, 0f), -rotSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(transform.position, new Vector3(0f, 1f, 0f), rotSpeed * Time.deltaTime);
        }
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    Cane.transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
        //    Debug.Log("Cane Rotation: " + Cane.transform.rotation);
        //}
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    Cane.transform.Rotate(Vector3.down, rotSpeed * Time.deltaTime);

        //}
    }
}
