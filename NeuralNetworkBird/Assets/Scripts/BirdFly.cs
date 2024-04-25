using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFly : MonoBehaviour
{
    [SerializeField] Transform body;
    [SerializeField] float flySpeed = 10f;
    [SerializeField] float rotSpeed = 5f;
    Vector3 startPos;
    Vector3 startRot;
    float x, y;
    private void Awake()
    {
        startPos = body.position;
        startRot = body.eulerAngles;
    }

    void Update()
    {
        GetInput();
        Move();
    }
    void GetInput()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
    }
    void Move()
    {
        body.position += body.forward * Math.Clamp(y,0f,1f) * Time.deltaTime * flySpeed;
        body.rotation *= Quaternion.Euler(0, x * Time.deltaTime * rotSpeed, 0) ;
    }
    private void Reset()
    {
        body.position = startPos;
        body.rotation = Quaternion.Euler(startRot);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Reset();
    }
}
