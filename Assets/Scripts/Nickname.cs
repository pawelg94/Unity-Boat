using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nickname : MonoBehaviour
{
    public Transform cam;
    public Transform leftbroadsideface;
    public Transform rightbroadsideface;
    public float FixedSize = 0.002f;


    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("hpBar").transform;
        leftbroadsideface = GameObject.FindGameObjectWithTag("lbCamLocal").transform;
        rightbroadsideface = GameObject.FindGameObjectWithTag("rbCamLocal").transform;
    }

    private void LateUpdate()
    {
        if (CameraController.leftBroadSideStatus)
        {
            var distance = (leftbroadsideface.position - transform.position).magnitude;
            var size = distance * FixedSize;
            transform.localScale = Vector3.one * size * 1.5f;
            transform.LookAt(transform.position + leftbroadsideface.forward);
        }
        if (CameraController.rightBroadSideStatus)
        {
            var distance = (rightbroadsideface.position - transform.position).magnitude;
            var size = distance * FixedSize;
            transform.localScale = Vector3.one * size * 1.5f;
            transform.LookAt(transform.position + rightbroadsideface.forward);
        }
        if (!RotateAroundCam.locked)
        {
            var distance = (cam.position - transform.position).magnitude;
            var size = distance * FixedSize;
            transform.localScale = Vector3.one * size;
            transform.LookAt(transform.position + cam.forward);
        }
    }
}
