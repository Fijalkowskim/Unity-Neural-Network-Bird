using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BirdController : MonoBehaviour
{
    [Header("Fitness function")]
    [SerializeField] float distanceMult = 1.5f;
    [SerializeField] float avgSpeedMult = 1.5f;

    [Header("Sensors")]
    [Range(2, 30)]
    [SerializeField] int numberOfSensors = 3;
    [Range(30,200)]
    [SerializeField] int fieldOfView = 120;

    [Header("Movement")]
    [SerializeField] float maxSpeed = 1f;
    [SerializeField] float acceleration = 40f;
    [SerializeField] float maxTurnSpeed = 2f;

    [Space(2)]
    [SerializeField] bool drawGizmos = false;

    Vector3 startPosition, startRotation, lastPosition;
    List<Ray> sensors;
    float _speed, _turn;
    public float speed
    {
        get { return _speed; }
        set
        {
            _speed = Mathf.Clamp01(value);
        }
    }
    public float turn
    {
        get { return _turn; }
        set
        {
            _turn = Mathf.Clamp(value, -1f, 1f);
        }
    }
    public float timeSinceStart { get; private set; }
    public float fitness { get; private set; }
    public float totalDistance { get; private set; }
    public float avgSpeed { get; private set; }
    void Awake()
    {
        SetupVariables();
        SetupSensors();
    }
    void SetupVariables()
    {
        startPosition = transform.position;
        startRotation = transform.eulerAngles;   
        timeSinceStart = 0f;
        totalDistance = 0f;
        avgSpeed = 0f;
        fitness = 0f;
    }
    void SetupSensors()
    {
        sensors = new List<Ray>();

        float angleBetweenSensors = fieldOfView / (numberOfSensors - 1);
        float startAngle = -fieldOfView / 2;

        for (int i = 0; i < numberOfSensors; i++)
        {
            Vector3 sensorDirection = Quaternion.Euler(0, startAngle + i * angleBetweenSensors, 0) * transform.forward;
            Ray sensorRay = new Ray(transform.position, sensorDirection);
            sensors.Add(sensorRay);
        }
    }
    void Reset()
    {
        timeSinceStart = 0f;
        totalDistance = 0f;
        avgSpeed = 0f;
        fitness = 0f;
        lastPosition = startPosition;
        transform.position = startPosition;
        transform.eulerAngles = startRotation;
    }
    void Update()
    {
        Move();
        SetupSensors();
    }
    void Move()
    {
        Vector3 forwardMovement = Vector3.Lerp(Vector3.zero, transform.forward * _speed * maxSpeed, acceleration * Time.deltaTime);
        transform.position += forwardMovement;
        Vector3 rotation = new Vector3(0, _turn * maxTurnSpeed * Time.deltaTime, 0);
        transform.eulerAngles += rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Reset();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (!drawGizmos || sensors == null) return;
        for (int i = 0; i < sensors.Count; i++)
        {
            Gizmos.DrawLine(sensors[i].origin, sensors[i].direction * 100f);
        }
    }
}
