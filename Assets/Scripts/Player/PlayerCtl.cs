using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCtl : MonoBehaviour
{

    public float speed = 0f;
    public float turning_speed = 0f;

    public float steerAngle = 0.0f;

    float afterFlightSlipperyTiresTime = 0.0f;
    float brakeSlipperyTiresTime = 0.0f;
    float handBrakeSlipperyTiresTime = 0.0f;
    bool isBrake = false;
    bool isHandBrake = false;
    bool isAcceleration = false;
    bool isReverseAcceleration = false;
    float accelerationForceMagnitude = 0.0f;
    Rigidbody rb = null;

    float smooth = 5.0f;
    float tiltAngle = 60.0f;

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
            transform.Translate(Vector3.forward * speed, 0);
            isAcceleration = true;
        }
        else if (velocity < -0.4f)
        {
            transform.Translate(Vector3.forward * -speed, 0);
            isAcceleration = true;
        }

        // Make tires more slippery (for 1 seconds) when player hit brakes
        if (isBrakeNow == true && isBrake == false)
        {
            brakeSlipperyTiresTime = 1.0f;
        }

        if (Mathf.Abs(heading_steer) > 0.001f)
        {
            // Smoothly tilts a transform towards a target rotation.
            float tiltAroundZ = heading_steer * turning_speed;

            // Rotate the cube by converting the angles into a quaternion.
            Quaternion target = Quaternion.Euler(0, tiltAroundZ, 0);

            // Dampen towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * smooth);
        }
        else if (Mathf.Abs(heading_steer) < 0.001f)
        {
            // Smoothly tilts a transform towards a target rotation.
            float tiltAroundZ = heading_steer * -turning_speed;

            // Rotate the cube by converting the angles into a quaternion.
            Quaternion target = Quaternion.Euler(0, tiltAroundZ, 0);

            // Dampen towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * smooth);
        }
    }
}
