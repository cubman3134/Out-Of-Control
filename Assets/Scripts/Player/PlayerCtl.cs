using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCtl : MonoBehaviour
{

    public float speed = 0f;
    public float turning_speed = 0f;
    public float jumpSpeed = 0f;

    public Transform front;

    public float steerAngle = 0.0f;

    private bool ableToJump = false;

    bool isAcceleration = false;
    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
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

        if (velocity != 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, front.transform.position, velocity * Time.deltaTime * speed);
            isAcceleration = true;
        }
        else
        {
            // if there is enough time 
        }

        if (Mathf.Abs(heading_steer) > 0.001f)
        {
            // Smoothly tilts a transform towards a target rotation.
            float tiltAroundZ = heading_steer * turning_speed;

            //transform.rotation = new Quaternion(0, tiltAroundZ, 0, 1000);
            transform.Rotate(0, tiltAroundZ * Time.deltaTime, 0, Space.World);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * jumpSpeed);
        }
    }

    private void OnCollisionStay(Collider collision)
    {
        if(collision.tag == "gorund")
        {
            ableToJump = true;
        }
        else
        {
            ableToJump = false;
        }
    }
}
