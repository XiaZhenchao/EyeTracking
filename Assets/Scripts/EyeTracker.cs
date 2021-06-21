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


    public GameObject Head;
    public string result;

    void Awake()
    {
        arFace = GetComponent<ARFace>();
        Content= GameObject.Find("Third").GetComponent<Text>();
        Load();
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
            Content.text = "Left eye position: " + arFace.leftEye.position.ToString() + "\n"
                + "Left eye rotation: " + arFace.leftEye.rotation.ToString() + "\n"
                + "Right eye position: " + arFace.rightEye.position.ToString() + "\n"
                + "right eye rotation: " + arFace.rightEye.rotation.ToString() + "\n"
                + "arface position: " + arFace.transform.position + "\n"
                + "arface rotation: " + arFace.transform.rotation;
            Debug.Log("Transform works from here");
            Quaternion changedrotation = Quaternion.Euler(-arFace.leftEye.rotation.eulerAngles);

            Head.transform.rotation = changedrotation;
            Debug.Log("Transform works end here");

            Debug.Log("------------------");
            Debug.Log("left eye position: "+arFace.leftEye.position);
            Debug.Log("left eye rotation: " + arFace.leftEye.rotation);
            Debug.Log("------------------");
            Debug.Log("------------------");
            Debug.Log("right eye position: " + arFace.rightEye.position);
            Debug.Log("right eye rotation: " + arFace.rightEye.rotation);
            Debug.Log("------------------");
            Debug.Log("arface position: " + arFace.transform.position);
            Save();
        }
    }

    public void Save()
    {
        result += "Left eye rotation: " + arFace.leftEye.rotation.ToString() + "\n";
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath +
            "/EyeTrackData.dat",FileMode.Create);
        EyeTrackContainer etc = new EyeTrackContainer();
        etc.result = result;
        bf.Serialize(file, etc);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath +
            "/EyeTrackData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath +
                "/EyeTrackData.dat", FileMode.Open);
            EyeTrackContainer etc = (EyeTrackContainer)bf.Deserialize(file);
            file.Close();
            result = etc.result;
        }
        
    }


}




[Serializable]
public class EyeTrackContainer
{
    public string result;
}
