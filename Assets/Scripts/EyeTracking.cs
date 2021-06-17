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
