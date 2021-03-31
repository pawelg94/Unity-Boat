using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static Transform cam;
    public static Transform leftbroadsideface;
    public static Transform rightbroadsideface;
    public Transform showDistanceOfUI;
    public float FixedSize = 0.005f;
    public float defaultScaleX;
    public float defaultScaleY;
    public float defaultScaleZ;


    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("hpBar").transform;
        leftbroadsideface = GameObject.FindGameObjectWithTag("lbCamLocal").transform;
        rightbroadsideface = GameObject.FindGameObjectWithTag("rbCamLocal").transform;
        showDistanceOfUI = GameObject.Find("LPlayer Test(Clone)").transform;
        defaultScaleX = transform.localScale.x;
        defaultScaleY = transform.localScale.y;
        defaultScaleZ = transform.localScale.z;
    }

    private void LateUpdate()
    {
        
        if (cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("hpBar").transform;
        }
        if (showDistanceOfUI == null)
        {
            showDistanceOfUI = GameObject.Find("LPlayer Test(Clone)").transform;
        }
        if (CameraController.leftBroadSideStatus)
        {
            var distance = (leftbroadsideface.position - transform.position).magnitude;
            var showDistance = (showDistanceOfUI.position - transform.position).magnitude;
            if (showDistance > 600f)
            {
                transform.localScale = Vector3.zero;
            }
            else
            {
                var size = distance * FixedSize;
                transform.localScale = new Vector3(defaultScaleX + 2f, defaultScaleY + 0.2f, defaultScaleZ) * size * 1.5f;
                transform.LookAt(transform.position + leftbroadsideface.forward);
            }
            
        }
        if (CameraController.rightBroadSideStatus)
        {
            var distance = (rightbroadsideface.position - transform.position).magnitude;
            var showDistance = (showDistanceOfUI.position - transform.position).magnitude;
            if (showDistance > 600f)
            {
                transform.localScale = Vector3.zero;
            }
            else
            {
                var size = distance * FixedSize;
                transform.localScale = new Vector3(defaultScaleX + 2f, defaultScaleY + 0.2f, defaultScaleZ) * size * 1.5f;
                transform.LookAt(transform.position + rightbroadsideface.forward);
            }
            
        }
        if (!RotateAroundCam.locked)
        {
            var distance = (cam.position - transform.position).magnitude;
            var showDistance = (showDistanceOfUI.position - transform.position).magnitude;
            if (showDistance > 600f)
            {
                transform.localScale = Vector3.zero;
            }
            else
            {
                var size = distance * FixedSize;
                transform.localScale = new Vector3(defaultScaleX + 2f, defaultScaleY + 0.2f, defaultScaleZ) * size;
                transform.LookAt(transform.position + cam.forward);
            }
            
        }

    }
}
