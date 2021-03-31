using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShipMovement : MonoBehaviour
{
    public float MaxSpeed = 20000000f;
    public float Acceleration = 0f;
    public float StartSpeed = 0f;
    public Transform SpeedSource;
    public float steer = 0f;
    float pochylenie = 0f;

    // Start is called before the first frame update
    private Rigidbody rbody;
    private BoatPhysics sbody;
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        sbody = GetComponent<BoatPhysics>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W) && StartSpeed < MaxSpeed)
        {
            Acceleration += 100f;
            StartSpeed = StartSpeed + Acceleration;
            
            rbody.AddForce(transform.forward * StartSpeed);
            Debug.Log("currentSpeed: " + StartSpeed + "Acceleration: " + Acceleration);

        }
        else if (Input.GetKey(KeyCode.W) && StartSpeed >= MaxSpeed)
            rbody.AddForce(transform.forward * MaxSpeed);
        else if (!Input.GetKey(KeyCode.W) && StartSpeed > 0)
        {
            StartSpeed = StartSpeed - 10000f;
            Debug.Log("predkosc: " + StartSpeed);
            rbody.AddForce(transform.forward * StartSpeed);

        }

        //Skret + pochylenie statku w lewo
        if (Input.GetKey(KeyCode.A) && steer <= 1)
        {
            steer = steer + 0.01f;
            sbody.centerOfMass = new Vector3(steer, -5, 10);
            rbody.AddForceAtPosition(steer * transform.right * 500000f, SpeedSource.position);
            pochylenie = 1f;

        }
        else if (Input.GetKey(KeyCode.A) && steer > 1 && pochylenie <= 2)
        {
            pochylenie = pochylenie + 0.001f;
            sbody.centerOfMass = new Vector3(pochylenie, -5, 10);
            rbody.AddForceAtPosition(1 * transform.right * 500000f, SpeedSource.position);
        }
        else if (Input.GetKey(KeyCode.A) && steer > 1 && pochylenie > 2)
        {
            sbody.centerOfMass = new Vector3(2, -5, 10);
            rbody.AddForceAtPosition(1 * transform.right * 500000f, SpeedSource.position);
        }

        //Skret + pochylenie statku w prawo
        if (Input.GetKey(KeyCode.D) && steer >= -1)
        {
            steer = steer - 0.01f;
            sbody.centerOfMass = new Vector3(steer, -5, 10);
            rbody.AddForceAtPosition(steer * transform.right * 500000f, SpeedSource.position);
            pochylenie = -1f;
        }
        else if (Input.GetKey(KeyCode.D) && steer < -1 && pochylenie > -2)
        {
            pochylenie = pochylenie - 0.001f;
            sbody.centerOfMass = new Vector3(pochylenie, -5, 10);
            rbody.AddForceAtPosition(-1 * transform.right * 500000f, SpeedSource.position);
        }
        else if (Input.GetKey(KeyCode.D) && steer < -1 && pochylenie < -2)
        {
            sbody.centerOfMass = new Vector3(-2, -5, 10);
            rbody.AddForceAtPosition(-1 * transform.right * 500000f, SpeedSource.position);
        }
        //Wyrownanie steru
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && steer >= 0 && pochylenie >= 0)
        {
            steer = steer - 0.01f;
            pochylenie = pochylenie - 0.01f;
            sbody.centerOfMass = new Vector3(pochylenie, -5, 10);
        }
        else if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && pochylenie >= 0)
        {
            pochylenie = pochylenie - 0.01f;
            sbody.centerOfMass = new Vector3(pochylenie, -5, 10);
        }
        else if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && steer < 0 && pochylenie < 0)
        {
            steer = steer + 0.01f;
            pochylenie = pochylenie + 0.01f;
            sbody.centerOfMass = new Vector3(pochylenie, -5, 10);
        }
        else if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && pochylenie < 0)
        {
            pochylenie = pochylenie + 0.01f;
            sbody.centerOfMass = new Vector3(pochylenie, -5, 10);
        }
    }
}
