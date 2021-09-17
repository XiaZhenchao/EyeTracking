using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EyeTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject leftEye;
    private GameObject rightEye;
    public ARFace arFace;

    void Start()
    {
        arFace = GetComponent<ARFace>();
        arFace.updated += OnUpdated;
    }


    void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
    {
        if (arFace.leftEye != null && leftEye == null)
        {
            leftEye.SetActive(false);
        }
        if (arFace.rightEye != null && rightEye == null)
        {
            rightEye.SetActive(false);
        }

        //set visibility
        bool shouldBeVisible = (arFace.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready);
        ShowMessage(shouldBeVisible);
    }

    void ShowMessage(bool isVisible) {
        if (rightEye != null && leftEye != null)
        {
            leftEye.SetActive(isVisible);
            rightEye.SetActive(isVisible);
            Debug.Log("------------------");
            Debug.Log("left eye position: " + arFace.leftEye.position);
            Debug.Log("left eye rotation: " + arFace.leftEye.rotation);
            Debug.Log("------------------");

            Debug.Log("------------------");
            Debug.Log("right eye position: " + arFace.rightEye.position);
            Debug.Log("right eye rotation: " + arFace.rightEye.rotation);
            Debug.Log("------------------");
        }
    }
}





/*
 using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
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
    GameObject[] VerticalBar;
    GameObject[] HorizontalBar;
    Vector3 HeadPosition;
    Vector3 PreviousPosition;
    public GameObject Head;
    //bool isRed = false;
    public GameObject NavigationSphere;
    GameObject Navigation;
    Vector3 position;
    public string result;
    public float TimeToLive = 0.01f;
    void Awake()
    {
        arFace = GetComponent<ARFace>();
        Content= GameObject.Find("Third").GetComponent<Text>();
        //Load();
        HeadPosition = Head.transform.position;
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
            //double tanAngle = 0;
            //double prefabDistance;

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
            
            VerticalBar = GameObject.FindGameObjectsWithTag("VerticalBar");
            foreach (GameObject bar in VerticalBar)
            {
                if (Math.Sqrt(Math.Pow(bar.transform.position.z - Head.transform.position.z, 2)
                    + Math.Pow(bar.transform.position.x - Head.transform.position.x, 2)) <= 0.2)
                {
                    bar.GetComponent<Renderer>().material.color = Color.red;
                }
            }


            //if (bar.GetComponent<Renderer>().material.color != Color.red && (Head.transform.position.z <= bar.transform.position.z + 0.3 || Head.transform.position.z > bar.transform.position.z - 0.3))
            //{


            //    if (bar.GetComponent<Renderer>().material.color != Color.red && Head.transform.position.x > 0.3)
            //    {
            //        //Generate the navigation line to right;
            //        position = Head.transform.position;
            //        while (position.x > 0.3)
            //        {
            //            Navigation = Instantiate(NavigationSphere, position, Quaternion.identity);
            //            position.x -= 0.8f;
            //            Destroy(Navigation, TimeToLive);
            //        }
            //    }
            //    else if (bar.GetComponent<Renderer>().material.color != Color.red && Head.transform.position.x > -4.7 && Head.transform.position.x <= 0.3)
            //    {
            //        //Generate the navigation line to left
            //        position = Head.transform.position;
            //        while (position.x > -4.7 && position.x <= 0.3)
            //        {
            //            Navigation = Instantiate(NavigationSphere, position, Quaternion.identity);
            //            position.x += 0.8f;
            //            Destroy(Navigation, TimeToLive);
            //        }
            //    }
            //    else if (bar.GetComponent<Renderer>().material.color != Color.red && Head.transform.position.x < -4.7 && Head.transform.position.x >= -9.7)
            //    {
            //        //Generate the navigation line to right;
            //        position = Head.transform.position;
            //        while (position.x < -4.7 && Head.transform.position.x >= -9.7)
            //        {
            //            Navigation = Instantiate(NavigationSphere, position, Quaternion.identity);
            //            position.x -= 0.8f;
            //            Destroy(Navigation, TimeToLive);
            //        }
            //    }
            //    else if (bar.GetComponent<Renderer>().material.color != Color.red && Head.transform.position.x < -9.7)
            //    {
            //        //Generate the navigation line to left;
            //        position = Head.transform.position;
            //        while (position.x < -9.7)
            //        {
            //            Navigation = Instantiate(NavigationSphere, position, Quaternion.identity);
            //            position.x += 0.8f;
            //            Destroy(Navigation, TimeToLive);
            //        }
            //    }



            //if (Head.transform.position.z == bar.transform.position.z)
            //{
            //    Debug.Log("get in to the else statement");
            //    distance = Math.Sqrt(Math.Pow(bar.transform.position.z - Head.transform.position.z, 2)
            //    + Math.Pow(bar.transform.position.x - Head.transform.position.x, 2));
            //    tanAngle = (Head.transform.position.z - bar.transform.position.z) /
            //        (Head.transform.position.x - bar.transform.position.x);
            //    while (distance > prefabDistance)
            //    {
            //        Debug.Log("Distance: " + distance);
            //        Debug.Log("prefabDistance: " + prefabDistance);
            //        Debug.Log("tanAngle: " + tanAngle);
            //        Debug.Log("Xposition: " + ToFloat(Math.Sqrt(Math.Pow(prefabDistance, 2) / (1 + Math.Pow(tanAngle, 2)))));
            //        Debug.Log("Zposition: " + ToFloat(Math.Sqrt(Math.Pow(prefabDistance, 2) / (1 + Math.Pow(tanAngle, 2))) * tanAngle));
            //        Navigation = Instantiate(NavigationSphere,
            //            new Vector3(bar.transform.position.x + ToFloat(Math.Sqrt(Math.Pow(prefabDistance, 2) / (1 + Math.Pow(tanAngle, 2)))),
            //            bar.transform.position.y,
            //            bar.transform.position.z + ToFloat(Math.Sqrt(Math.Pow(prefabDistance, 2) / (1 + Math.Pow(tanAngle, 2))) * tanAngle)),
            //            Quaternion.identity);
            //        prefabDistance += 0.1;
            //        Destroy(Navigation, TimeToLive);
            //    }



            //}
            //isRed = false;
            //}
        }


        //if (bar.GetComponent<Renderer>().material.color != Color.red && (Head.transform.position.x <= bar.transform.position.x + 0.3 || Head.transform.position.x > bar.transform.position.x - 0.3))
        //{
        //    distance = Math.Sqrt(Math.Pow(bar.transform.position.z - Head.transform.position.z, 2)
        //    + Math.Pow(bar.transform.position.x - Head.transform.position.x, 2));

        //    if (bar.GetComponent<Renderer>().material.color != Color.red && Head.transform.position.z > 0.3)
        //    {
        //        //Generate the navigation line to upward;
        //        position = Head.transform.position;
        //        while (position.z > 0.8)
        //        {
        //            Navigation = Instantiate(NavigationSphere, position, Quaternion.identity);
        //            position.z -= 0.8f;
        //            Destroy(Navigation, TimeToLive);
        //        }
        //    }
        //    else if (bar.GetComponent<Renderer>().material.color != Color.red && Head.transform.position.z > -4.7 && Head.transform.position.z <= 0.3)
        //    {
        //        //Generate the navigation line to downward
        //        position = Head.transform.position;
        //        while (position.z > -4.7 && position.z < 0.3)
        //        {
        //            Navigation = Instantiate(NavigationSphere, position, Quaternion.identity);
        //            position.z += 0.8f;
        //            Destroy(Navigation, TimeToLive);
        //        }
        //    }
        //    else if (bar.GetComponent<Renderer>().material.color != Color.red && Head.transform.position.z < -4.7 && Head.transform.position.z >= -9.7)
        //    {
        //        //Generate the navigation line to upward;
        //        position = Head.transform.position;
        //        while (position.z < -4.7 && Head.transform.position.z >= -9.7)
        //        {
        //            Navigation = Instantiate(NavigationSphere, position, Quaternion.identity);
        //            position.z -= 0.8f;
        //            Destroy(Navigation, TimeToLive);
        //        }
        //    }
        //    else if (bar.GetComponent<Renderer>().material.color != Color.red && Head.transform.position.z < -9.7)
        //    {
        //        //Generate the navigation line to downward;
        //        position = Head.transform.position;
        //        while (position.z < -9.7)
        //        {
        //            Navigation = Instantiate(NavigationSphere, position, Quaternion.identity);
        //            position.z += 0.8f;
        //            Destroy(Navigation, TimeToLive);
        //        }
        //    }
        //}

        HorizontalBar = GameObject.FindGameObjectsWithTag("HorzontalBar");
            foreach (GameObject bar in HorizontalBar)
            {
                if (Math.Sqrt(Math.Pow(bar.transform.position.z - Head.transform.position.z, 2)
                   + Math.Pow(bar.transform.position.x - Head.transform.position.x, 2)) <= 0.2)
                {
                    bar.GetComponent<Renderer>().material.color = Color.red;
                }

                
                PreviousPosition = arFace.transform.position;
                //Save();
            }
        }
    }

    //public void Save()
    //{
    //    result += "Left eye rotation: " + arFace.leftEye.rotation.ToString() + "\n";

    //    BinaryFormatter bf = new BinaryFormatter();
    //    FileStream file = File.Open(Application.persistentDataPath +
    //        "/EyeTrackData.dat", FileMode.Create);
    //    EyeTrackContainer etc = new EyeTrackContainer();
    //    etc.result = result;
    //    bf.Serialize(file, etc);
    //    file.Close();
    //}

    //public void Load()
    //{
    //    if (File.Exists(Application.persistentDataPath +
    //        "/EyeTrackData.dat"))
    //    {
    //        BinaryFormatter bf = new BinaryFormatter();
    //        FileStream file = File.Open(Application.persistentDataPath +
    //            "/EyeTrackData.dat", FileMode.Open);
    //        EyeTrackContainer etc = (EyeTrackContainer)bf.Deserialize(file);
    //        file.Close();
    //        result = etc.result;
    //    }

    //}

    //public static float ToFloat(double value)
    //{
    //    return (float)value;
    //}
//}




[Serializable]
public class EyeTrackContainer
{
    public string result;
}

 
 
 
 */