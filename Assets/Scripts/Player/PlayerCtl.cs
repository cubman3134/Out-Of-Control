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

    public Transform front;
    public Camera thisCamera;

    private bool ableToJump = false;
    private bool isAcceleration = false;
    private bool isMovingFowd = false;

    Rigidbody _rb;

    public int pillMode = -1;
    /*
     * Flip Up -> 0
     * LeftLeft -> 1
     * TheRock -> 2 --> think later
     * SleepLess -> 3
     */
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
        //power_stats[0] = true;
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

        if(pillMode == 0)
        {
            thisCamera.gameObject.transform.localRotation = Quaternion.Euler(30f, 0, 180f);
        }
        else
        {
            thisCamera.gameObject.transform.localRotation = Quaternion.Euler(30f, 0, 0);
        }
    }

    // Update is called once per frame
    void UpdateInput()
    {
        float velocity = Input.GetAxisRaw("Vertical");
        float heading_steer = Input.GetAxisRaw("Horizontal");

        Vector3 direction = transform.position - front.transform.position;

        if (velocity > 0f)
        {
            Vector3 v3Force = accel * front.transform.forward;
            _rb.AddForce(v3Force);
            //_rb.AddForce(front.transform.position * accel);
            //_rb.AddForceAtPosition(front.transform.position * accel, front.transform.position);
            //transform.position = Vector3.MoveTowards(transform.position, front.transform.position, velocity * Time.deltaTime * speed);
            isAcceleration = true;
            isMovingFowd = true;
        }
        else if(velocity < 0f)
        {
            Vector3 v3Force = -accel * front.transform.forward;
            _rb.AddForce(v3Force);
            //_rb.AddForce(front.transform.position * -accel);
            //transform.position = Vector3.MoveTowards(transform.position, front.transform.position, velocity * Time.deltaTime * speed);
            isAcceleration = true;
            isMovingFowd = false;
        }
        else
        {
            isAcceleration = false;
        }

        if (_rb.velocity.magnitude > maxSpeed)
            _rb.velocity = _rb.velocity.normalized * maxSpeed;

        if (Mathf.Abs(heading_steer) > 0.001f && isAcceleration && isMovingFowd)
        {
            // Smoothly tilts a transform towards a target rotation.
            if(pillMode == 1)
            {
                heading_steer = -Mathf.Abs(heading_steer);
            }

            float tiltAroundZ = heading_steer * turning_speed;

            //transform.rotation = new Quaternion(0, tiltAroundZ, 0, 1000);
            transform.Rotate(0, tiltAroundZ * Time.deltaTime, 0, Space.World);

        }else if(Mathf.Abs(heading_steer) > 0.001f && isAcceleration && !isMovingFowd)
        {
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
        if (Input.GetKeyDown(KeyCode.Space) && ableToJump && power_stats[2] > 0f)
        {
            _rb.AddForce(Vector3.up * jumpSpeed);
            ableToJump = false;
        }

        if(power_stats[0] > 0f)
        {
            if (transform.position.y < maxFlightDistance)
                transform.Translate(Vector3.up * Time.deltaTime * speed);

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Finish")
        {
            //Debug.LogError("Finish touched");
            ableToJump = true;
        }
    }
}
