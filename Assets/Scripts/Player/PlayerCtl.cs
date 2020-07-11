using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCtl : MonoBehaviour
{
    public float speed = 0f;
    public float accel = 0f;
    public float maxSpeed = 0f;
    public float maxFlightDistance = 0f;
    public float turning_speed = 0f;
    public float jumpSpeed = 0f;

    //i need this because after the car is rotating, it has no reference where should it go
    public Transform front;
    public Camera thisCamera;

    private bool ableToJump = false;
    private bool isAcceleration = false;
    private bool isMovingFowd = false;

    //this is important i guess
    Rigidbody _rb;

    public int pillMode = -1;
    /*
     * Flip Up -> 0
     * LeftLeft -> 1
     * TheRock -> 2 --> think later
     * SleepLess -> 3
     */

    //yes float, it represent remaining time
    public List<float> power_stats;
    /*
     * JP + wing --> 0
     * coin magnet --> 1
     * power spring --> 2
     * invis --> 3
     */

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void powerCheck()
    {
        for(int i=0; i<power_stats.Count; i++)
        {
            if (power_stats[i] > 0f)
                power_stats[i] -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        UpdateInput();
        powerCheck();

        //it is about flip up pills --> up down left right
        if(pillMode == 0)
        {
            thisCamera.gameObject.transform.localRotation = Quaternion.Euler(30f, 0, 180f);
        }
        else
        {
            thisCamera.gameObject.transform.localRotation = Quaternion.Euler(30f, 0, 0);
        }
    }

    void UpdateInput()
    {
        float velocity = Input.GetAxisRaw("Vertical");
        float heading_steer = Input.GetAxisRaw("Horizontal");

        if (velocity > 0f)
        {
            //idk how but this code works for acceleration
            Vector3 v3Force = accel * front.transform.forward;
            _rb.AddForce(v3Force);
            isAcceleration = true;
            isMovingFowd = true;
        }
        else if(velocity < 0f)
        {
            Vector3 v3Force = -accel * front.transform.forward;
            _rb.AddForce(v3Force);
            isAcceleration = true;
            isMovingFowd = false;
        }
        else
        {
            isAcceleration = false;
        }

        if (_rb.velocity.magnitude > maxSpeed)
            _rb.velocity = _rb.velocity.normalized * maxSpeed;

        //if moving forward the tiltaroundz has to be positive
        if (Mathf.Abs(heading_steer) > 0.001f && isAcceleration && isMovingFowd)
        {
            //pill mode 1 --> all left
            if(pillMode == 1)
            {
                heading_steer = -Mathf.Abs(heading_steer);
            }

            // Smoothly tilts a transform towards a target rotation.
            float tiltAroundZ = heading_steer * turning_speed;

            //rotate the car
            transform.Rotate(0, tiltAroundZ * Time.deltaTime, 0, Space.World);

        }else if(Mathf.Abs(heading_steer) > 0.001f && isAcceleration && !isMovingFowd)
        {
            //pill mode 1 --> all left
            if (pillMode == 1)
            {
                heading_steer = -Mathf.Abs(heading_steer);
            }

            float tiltAroundZ = heading_steer * turning_speed;

            transform.Rotate(0, -tiltAroundZ * Time.deltaTime, 0, Space.World);
        }
    }

    private void Update()
    {
        //power stats 2 --> able to jump
        if (Input.GetKeyDown(KeyCode.Space) && ableToJump && power_stats[2] > 0f)
        {
            _rb.AddForce(Vector3.up * jumpSpeed);
            ableToJump = false;
        }

        //power stats 0 --> flying car
        if(power_stats[0] > 0f)
        {
            if (transform.position.y < maxFlightDistance)
                transform.Translate(Vector3.up * Time.deltaTime * speed);

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //idk how to express ground, instead i just use the existing tag from unity
        if(collision.gameObject.tag == "Finish")
        {
            //Debug.LogError("Finish touched");
            ableToJump = true;
        }
    }
}
