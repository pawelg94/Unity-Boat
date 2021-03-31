using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{

    public Vector3 northDirection;
    public Transform playerCam;
    public RectTransform northLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeNorthDirection();
    }

    public void ChangeNorthDirection()
    {
            northDirection.z = playerCam.eulerAngles.y;
            northLayer.localEulerAngles = northDirection;               
    }

}
