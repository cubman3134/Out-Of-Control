using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCtl : MonoBehaviour
{

    public float speed = 0f;
    public float turning_speed = 0f;

    public Transform front;

    public float steerAngle = 0.0f;

    bool isAcceleration = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        UpdateInput();

        Vector3 wsDownDirection = transform.TransformDirection(Vector3.down);
        wsDownDirection.Normalize();
    }

    // Update is called once per frame
    void UpdateInput()
    {
        float heading_steer = Input.GetAxisRaw("Horizontal");
        float velocity = Input.GetAxisRaw("Vertical");

        bool isBrakeNow = false;
        bool isHandBrakeNow = Input.GetKey(KeyCode.Space);

        if (velocity > 0.4f)
        {
            //transform.Translate(front.transform.position * speed * Time.deltaTime, 0);
            transform.position = Vector3.MoveTowards(transform.position, front.transform.position, velocity * Time.deltaTime * speed);
            isAcceleration = true;
        }
        else if (velocity < -0.4f)
        {
            transform.position = Vector3.MoveTowards(transform.position, front.transform.position, velocity * Time.deltaTime * speed);
            isAcceleration = true;
        }

        // Make tires more slippery (for 1 seconds) when player hit brakes
        //if (isBrakeNow == true && isBrake == false)
        //{
        //    brakeSlipperyTiresTime = 1.0f;
        //}

        if (Mathf.Abs(heading_steer) > 0.001f)
        {
            // Smoothly tilts a transform towards a target rotation.
            float tiltAroundZ = heading_steer * turning_speed;

            //transform.rotation = new Quaternion(0, tiltAroundZ, 0, 1000);
            transform.Rotate(0, tiltAroundZ * Time.deltaTime, 0, Space.World);

        }
    }
}
