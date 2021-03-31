using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    //public static CameraController instance;

    public Camera rotateAroundCam;
    public Camera leftBroadsideCam;
    public Camera rightBroadsideCam;
    public Image leftViewIndicator;
    public Image rightViewIndicator;
    public static bool rightBroadSideStatus;
    public static bool leftBroadSideStatus;

    // Camera Swapping

    void Start()
    {
        //UIManager.instance.aimingcursor.enabled = false;
        rotateAroundCam.enabled = true;
        leftBroadsideCam.enabled = false;
        rightBroadsideCam.enabled = false;
        leftViewIndicator.enabled = false;
        rightViewIndicator.enabled = false;
        RotateAroundCam.locked = false;

    }
    void Update()
    {
        if (Input.GetButtonDown("CameraRightBroadsideSwitch") && !leftBroadsideCam.enabled == true && !PlayerController.chatFocusStatus)
        {
            UIManager.instance.aimingcursor.enabled = !UIManager.instance.aimingcursor.enabled;
            RotateAroundCam.locked = !RotateAroundCam.locked;
            rightBroadsideCam.enabled = !rightBroadsideCam.enabled;
            rightViewIndicator.enabled = !rightViewIndicator.enabled;
            rotateAroundCam.enabled = !rotateAroundCam.enabled;
        }

        if (Input.GetButtonDown("CameraLeftBroadsideSwitch") && !rightBroadsideCam.enabled == true && !PlayerController.chatFocusStatus)
        {
            UIManager.instance.aimingcursor.enabled = !UIManager.instance.aimingcursor.enabled;
            RotateAroundCam.locked = !RotateAroundCam.locked;
            leftBroadsideCam.enabled = !leftBroadsideCam.enabled;
            leftViewIndicator.enabled = !leftViewIndicator.enabled;
            rotateAroundCam.enabled = !rotateAroundCam.enabled;
        }
        rightBroadSideStatus = rightBroadsideCam.enabled;
        leftBroadSideStatus = leftBroadsideCam.enabled;

    }


}
