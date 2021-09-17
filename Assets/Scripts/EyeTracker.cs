using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(ARFace))]

public class EyeTracker : MonoBehaviour
{
    [SerializeField]
    private GameObject eyePrefab;
    private GameObject leftEye;
    private GameObject rightEye;
    public ARFace arFace;
    public Text Content;

    public GameObject User;
    public GameObject[] RightBar;
    public GameObject[] TopBar;
    public GameObject[] LeftBar;
    public GameObject[] BottomBar;
    Vector3 HeadPosition;
    Vector3 PreviousPosition;
    public GameObject Head;
    public GameObject NavigationSphere;
    public GameObject bar;
    GameObject Navigation;
    Vector3 position;
    public string result;
    public float TimeToLive = 0.0001f;
    bool RightRedFlag;
    bool TopRedFlag;
    bool LeftRedFlag;
    bool BottomRedFlag;

    void Awake()
    {
        arFace = GetComponent<ARFace>();
        Content= GameObject.Find("Third").GetComponent<Text>();
        HeadPosition = Head.transform.position;
        RightBar = new GameObject[10];
        TopBar = new GameObject[10];
        LeftBar = new GameObject[10];
        BottomBar = new GameObject[10];
        for (int i = 0; i <= 9; i++)
        {
            bar = GameObject.Find("RightBar/CubeR" + i);
            RightBar[i] = bar;
            bar = GameObject.Find("TopBar/CubeT" + i);
            TopBar[i] = bar;
            bar = GameObject.Find("LeftBar/CubeL" + i);
            LeftBar[i] = bar;
            bar = GameObject.Find("BottomBar/CubeB" + i);
            BottomBar[i] = bar;
            Debug.Log("Number." + i +": "+RightBar[i].transform.position);
        }
        
        PreviousPosition = arFace.transform.position;
    }

    void OnEnable()
    {
        ARFaceManager faceManager = FindObjectOfType<ARFaceManager>();
        if (faceManager != null && faceManager.subsystem != null && faceManager.subsystem.SubsystemDescriptor.supportsEyeTracking)
        {
            arFace.updated += OnUpdated;
            // eyeTrackingSupportedText.text = $"Eye Tracking is supported.";
            Debug.Log("Eye Tracking is support on this device");
        }
        else {
            // eyeTrackingSupportedText.text = $"Eye Tracking is currently not supported on this deveice.";
            Debug.LogError("Eye Tracking is not support on this device");
        }
        

    }

    void OnDisable()
    {
        arFace.updated -= OnUpdated;
        SetVisibility(false);
    }

    void OnUpdated(ARFaceUpdatedEventArgs eventArgs) {
        if (arFace.leftEye != null && leftEye == null) {
            leftEye = Instantiate(eyePrefab, arFace.leftEye);
            leftEye.SetActive(false);
        }
        if (arFace.rightEye != null && rightEye == null)
        {
            rightEye = Instantiate(eyePrefab, arFace.rightEye);
            rightEye.SetActive(false);
        }

        //set visibility
        bool shouldBeVisible = (arFace.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready);
        SetVisibility(shouldBeVisible);
    }

    void SetVisibility(bool isVisible) {
        if (rightEye != null && leftEye != null)
        {
            leftEye.SetActive(isVisible);
            rightEye.SetActive(isVisible);
            Content.color = Color.red;
            Content.fontSize = 60;
            //Content.text = "Left eye position: " + arFace.leftEye.position.ToString() + "\n"
            //    + "Left eye rotation: " + arFace.leftEye.rotation.ToString() + "\n"
            //    + "Right eye position: " + arFace.rightEye.position.ToString() + "\n"
            //    + "right eye rotation: " + arFace.rightEye.rotation.ToString() + "\n"
            //    + "arface position: " + arFace.transform.position + "\n"
            //    + "arface rotation: " + arFace.transform.rotation;
            Quaternion changedrotation = Quaternion.Euler(arFace.leftEye.rotation.eulerAngles);

            double distance = 0;
            Debug.Log("HeadRotation: " + Head.transform.rotation);
            Debug.Log("Headposition: "+ HeadPosition);

            Head.transform.rotation = changedrotation;
            if ((arFace.transform.position.x - PreviousPosition.x) > 0)
            {
                HeadPosition.x -= (arFace.transform.position.x - PreviousPosition.x);
            }

            if ((arFace.transform.position.x - PreviousPosition.x) < 0)
            {
                HeadPosition.x += (PreviousPosition.x - arFace.transform.position.x);
            }

            if ((arFace.transform.position.z - PreviousPosition.z) > 0)
            {
                HeadPosition.z -= (arFace.transform.position.z - PreviousPosition.z);
            }

            if ((arFace.transform.position.z - PreviousPosition.z) < 0)
            {
                HeadPosition.z += (PreviousPosition.z - arFace.transform.position.z);
            }


            Head.transform.position = HeadPosition;

            for (int i = 0; i <10; i++)
            {
                if (Math.Sqrt(Math.Pow(RightBar[i].transform.position.z - Head.transform.position.z, 2)
                    + Math.Pow(RightBar[i].transform.position.x - Head.transform.position.x, 2)) <= 0.4)
                {
                    RightBar[i].GetComponent<Renderer>().material.color = Color.red;
                }
                else if (Math.Sqrt(Math.Pow(RightBar[i].transform.position.z - Head.transform.position.z, 2)
                    + Math.Pow(RightBar[i].transform.position.x - Head.transform.position.x, 2)) > 0.4)
                {
                    //if (i == 0 && RightBar[i].GetComponent<Renderer>().material.color == Color.white)
                    //{
                    //    int NumOfNavigation = Convert.ToInt32(Math.Sqrt(Math.Pow(RightBar[i].transform.position.z - Head.transform.position.z, 2)
                    //  + Math.Pow(RightBar[i].transform.position.x - Head.transform.position.x, 2)) / 1.5);
                    //    position = RightBar[i].transform.position;
                    //    float xdistance = (RightBar[i].transform.position.x - Head.transform.position.x) / NumOfNavigation;
                    //    float zdistance = (RightBar[i].transform.position.z - Head.transform.position.z) / NumOfNavigation;
                    //    for (int j = 0; j < NumOfNavigation; j++)
                    //    {
                    //        Navigation = Instantiate(NavigationSphere, position, Quaternion.identity);
                    //        Debug.Log("NavigationPosition: " + position);
                    //        position.x -= xdistance;
                    //        position.z -= zdistance;
                    //    }
                    //    Destroy(Navigation, TimeToLive);
                    //}  We don't need to worried about when about first block, since the initial position of user is first
                    //   block
                      if (RightBar[i-1].GetComponent<Renderer>().material.color == Color.red
                        && RightBar[i].GetComponent<Renderer>().material.color == Color.white)
                    {
                        int NumOfNavigation = Convert.ToInt32(Math.Sqrt(Math.Pow(RightBar[i].transform.position.z - Head.transform.position.z, 2)
                      + Math.Pow(RightBar[i].transform.position.x - Head.transform.position.x, 2)) / 1.5);
                        position = RightBar[i].transform.position;
                        float xdistance = (RightBar[i].transform.position.x - Head.transform.position.x) / NumOfNavigation;
                        float zdistance = (RightBar[i].transform.position.z - Head.transform.position.z) / NumOfNavigation;
                        for (int j = 0; j < NumOfNavigation; j++)
                        {
                            Navigation = Instantiate(NavigationSphere, position, Quaternion.identity);
                            Debug.Log("NavigationPosition: " + position);
                            position.x -= xdistance;
                            position.z -= zdistance;
                        }
                        Destroy(Navigation, TimeToLive);
                    }
                }
            }

            for (int i = 0; i < TopBar.Length; i++)
            {
                if (Math.Sqrt(Math.Pow(TopBar[i].transform.position.z - Head.transform.position.z, 2)
                    + Math.Pow(TopBar[i].transform.position.x - Head.transform.position.x, 2)) <= 0.2)
                {
                    TopBar[i].GetComponent<Renderer>().material.color = Color.red;
                }
            }

            for (int i = 0; i < LeftBar.Length; i++)
            {
                if (Math.Sqrt(Math.Pow(LeftBar[i].transform.position.z - Head.transform.position.z, 2)
                    + Math.Pow(LeftBar[i].transform.position.x - Head.transform.position.x, 2)) <= 0.2)
                {
                    LeftBar[i].GetComponent<Renderer>().material.color = Color.red;
                }
            }

            for (int i = 0; i < BottomBar.Length; i++)
            {
                if (Math.Sqrt(Math.Pow(BottomBar[i].transform.position.z - Head.transform.position.z, 2)
                    + Math.Pow(BottomBar[i].transform.position.x - Head.transform.position.x, 2)) <= 0.2)
                {
                    BottomBar[i].GetComponent<Renderer>().material.color = Color.red;
                }
            }

            PreviousPosition = arFace.transform.position;


        }
     }

     
}






[Serializable]
public class EyeTrackContainer
{
    public string result;
}
