using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TextContent : MonoBehaviour
{

    public ARFace target;
    Text Greetings;
    Text Greetings2;
    Text LefteyePosition;
    Text LefteyeRotation;
    Text RighteyePosition;
    Text RighteyeRotation;
    void Start()
    {
        //target = GameObject.Find("/Assets/Prefabs/EyeTracking").GetComponent<EyeTracker>().arFace;
        Greetings = GameObject.Find("MainText").GetComponent<Text>();
        Greetings2 = GameObject.Find("Second").GetComponent<Text>();
        //LefteyePosition = GameObject.Find("Third").GetComponent<Text>();
        //LefteyeRotation.GetComponent<Text>();
        //RighteyePosition.GetComponent<Text>();
        //RighteyeRotation.GetComponent<Text>();
        Greetings.color = Color.white;
        Greetings.fontSize = 40;
        Greetings2.color = Color.white;
        Greetings2.fontSize = 40;
        //LefteyePosition.text = "";
        //LefteyeRotation.text = "";
        //RighteyePosition.text = "";
        //RighteyeRotation.text = "";
        //LefteyePosition.color = Color.white;
        //LefteyeRotation.color = Color.white;
        //RighteyePosition.color = Color.white;
        //RighteyeRotation.color = Color.white;
        //LefteyePosition.fontSize = 30;
        //LefteyeRotation.fontSize = 30;
        //RighteyePosition.fontSize = 30;
        //RighteyeRotation.fontSize = 30;
    }

    // Update is called once per frame
    void Update()
    {
        //LefteyePosition.text = target.leftEye.position.ToString();
        //LefteyeRotation.text = target.leftEye.rotation.ToString();
        //RighteyePosition.text = target.rightEye.position.ToString();
        //RighteyeRotation.text = target.rightEye.rotation.ToString();
        Greetings.text = "Welcome1!!!!!!";
        Greetings2.text = "Welcome2!!!!!!";
    }
}
