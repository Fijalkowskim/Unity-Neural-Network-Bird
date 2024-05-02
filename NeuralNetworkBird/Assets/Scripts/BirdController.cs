using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BirdController : MonoBehaviour
{
    [Header("Fitness function")]
    [Range(0f, 5f)]
    [Tooltip("How important distance is in fitness calculation")]
    [SerializeField] float distanceMult = 1.5f;
    [Range(0f, 5f)]
    [Tooltip("How important average speed is in fitness calculation")]
    [SerializeField] float avgSpeedMult = 0.3f;
    [Range(0f, 5f)]
    [Tooltip("How important staying far from obstacles is in fitness calculation")]
    [SerializeField] float sensorMult = 0.2f;
    [Tooltip("Amount of fintess score to save as sucessfull network")]
    [SerializeField] float successFitnessValue = 1000f;
    [Tooltip("If after given time fitness will be less then given threshold, reset simulation")]
    [SerializeField] float timeToResetSimulation = 20f;
    [Tooltip("If fitness will be lower then this threshold after given time, reset simulation")]
    [SerializeField] float fitnessToResetSimulation = 40f;

    [Header("Sensors")]
    [Range(2, 30)]
    [SerializeField] int _numberOfSensors = 3;
    [Range(30,200)]
    [SerializeField] int fieldOfView = 120;
    [Range(10,1000)]
    [SerializeField] float sensorDistance = 200f;
    [SerializeField] LayerMask sensorMask;

    [Header("Movement")]
    [SerializeField] float maxSpeed = 1f;
    [SerializeField] float acceleration = 40f;
    [SerializeField] float maxTurnSpeed = 2f;

    [Space(2)]
    [SerializeField] bool drawGizmos = false;

    Vector3 startPosition, startRotation, lastPosition;
    [SerializeField] float _speed, _turn;
    Ray[] sensors;
    RaycastHit hit;
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
    public int numberOfSensors { get { return _numberOfSensors; } }
    public float timeSinceStart { get; private set; }
    public float fitness { get; private set; }
    public float totalDistance { get; private set; }
    public float avgSpeed { get; private set; }
    public float[] sensorValues { get; private set; }
    void Awake()
    {
        SetupVariables();
    }
    void Update()
    {
        Move();
        CalculateFintess();
    }
    private void FixedUpdate()
    {
        ReadSensors();
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
    void ReadSensors()
    {
        sensors = new Ray[_numberOfSensors];
        sensorValues = new float[_numberOfSensors];

        float angleBetweenSensors = fieldOfView / (_numberOfSensors - 1);
        float startAngle = -fieldOfView / 2;
        for (int i = 0; i < _numberOfSensors; i++)
        {
            Vector3 sensorDirection = Quaternion.Euler(0, startAngle + i * angleBetweenSensors, 0) * transform.forward;
            Ray sensorRay = new Ray(transform.position, sensorDirection);
            sensors[i] = sensorRay;
            if (Physics.Raycast(sensorRay, out hit, sensorDistance, sensorMask))
                sensorValues[i] = hit.distance / sensorDistance;
            else
                sensorValues[i] = 0;

        }
    }

    void Move()
    {
        Vector3 forwardMovement = Vector3.Lerp(Vector3.zero, transform.forward * _speed * maxSpeed, acceleration * Time.deltaTime);
        transform.position += forwardMovement;
        Vector3 rotation = new Vector3(0, _turn * maxTurnSpeed * Time.deltaTime, 0);
        transform.eulerAngles += rotation;
    }
    void CalculateFintess()
    {
        totalDistance += Vector3.Distance(transform.position, lastPosition);
        avgSpeed = timeSinceStart == 0 ? 0 : totalDistance / timeSinceStart;

        float avgSensorValue = 0;
        if (sensorValues != null)
        {
            for (int i = 0; i < _numberOfSensors; i++)
            {
                avgSensorValue += sensorValues[i];
            }
            avgSensorValue /= _numberOfSensors;
        }

        fitness = (totalDistance * distanceMult) + (avgSpeed * avgSpeedMult) + (avgSensorValue * sensorMult);

        timeSinceStart += Time.deltaTime;
        lastPosition = transform.position;

        if (timeSinceStart >= timeToResetSimulation && fitness <= fitnessToResetSimulation)
        {
            Reset();
            return;
        }    
        if(fitness >= successFitnessValue)
        {
            //Save network
            Reset();
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
    private void OnCollisionEnter(Collision collision)
    {
        Reset();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (!drawGizmos || sensors == null) return;
        for (int i = 0; i < numberOfSensors; i++)
        {
            Gizmos.DrawLine(sensors[i].origin, sensors[i].GetPoint(sensorDistance) );
        }
    }
}
