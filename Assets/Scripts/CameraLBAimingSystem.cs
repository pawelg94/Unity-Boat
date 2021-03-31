using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLBAimingSystem : MonoBehaviour
{
    public PlayerManager player;
    public float sensivity = 100f;
    public float clampAngle = 85f;

    private float verticalRotation;
    private float horizontalRotation;

    // Start is called before the first frame update
    void Start()
    {
        verticalRotation = transform.localEulerAngles.x;
        horizontalRotation = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Look();
        }
        Debug.DrawRay(player.transform.position, transform.forward * 200, Color.red);
    }

    private void Look()
    {

        float _mouseVertical = -Input.GetAxis("Mouse Y");
        float _mouseHorizontal = Input.GetAxis("Mouse X");

        verticalRotation += _mouseVertical * sensivity * Time.deltaTime;
        horizontalRotation += _mouseHorizontal * sensivity * Time.deltaTime;

        verticalRotation = Mathf.Clamp(verticalRotation, -30f, 20f);
        horizontalRotation = Mathf.Clamp(horizontalRotation, -120f, -60f);

        transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);

    }

}
