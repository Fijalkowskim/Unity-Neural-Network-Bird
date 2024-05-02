using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
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
    [Range(10f,90f)]
    [SerializeField] float maxTurnAngle = 45f;
    [SerializeField] float maxSpeed = 1f;

    Rigidbody rb;
    Vector3 startPosition, startRotation, lastPosition;
    List<Ray> sensors;

    public float acceleration { get; set; }
    public float turn { get; set; }
    public float timeSinceStart { get; private set; }
    public float fitness { get; private set; }
    public float totalDistance { get; private set; }
    public float avgSpeed { get; private set; }
    void Awake()
    {
        SetupVariables();
    }
    void SetupVariables()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        sensors = new List<Ray>();
        timeSinceStart = 0f;
        totalDistance = 0f;
        avgSpeed = 0f;
        fitness = 0f;
    }
    private void Reset()
    {
        timeSinceStart = 0f;
        totalDistance = 0f;
        avgSpeed = 0f;
        fitness = 0f;
        lastPosition = startPosition;
        transform.position = startPosition;
        transform.eulerAngles = startRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Reset();
    }
}
