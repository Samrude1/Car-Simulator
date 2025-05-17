using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBrakeForce;
    private float targetBrakeForce;
    private bool isBraking;
    private Rigidbody rb;
    private bool isReversing;

    private float steerAngleVelocity; // Pehmennysmuuttuja ohjaukseen

    [Header("Settings")]
    [SerializeField] private float motorForce = 1500f;
    [SerializeField] private float brakeForce = 30f;
    [SerializeField] private float brakeSensitivity = 10f;
    [SerializeField] private float maxSteerAngle = 30f;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    [Header("Wheel Transforms")]
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    [SerializeField] private AudioSource engineAudioSource;
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 4.0f;
    [SerializeField] private float pitchSmoothing = 2f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.3f, 0.1f);
        SetWheelFriction();
        SetSuspension();
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        UpdateEngineSound();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
        isReversing = Input.GetKey(KeyCode.X);
        targetBrakeForce = Input.GetKey(KeyCode.S) ? brakeForce : 0f;

        // Pehmeä jarrutus
        float brakeProgress = Mathf.Clamp01(Time.fixedDeltaTime * brakeSensitivity);
        float curve = brakeProgress * brakeProgress;
        currentBrakeForce = Mathf.Lerp(currentBrakeForce, targetBrakeForce, curve);
    }

    private void HandleMotor()
    {
        float motorInput = verticalInput * motorForce;

        if (isReversing)
        {
            motorInput = -motorForce;
        }
        else if (verticalInput > 0f)
        {
            motorInput = verticalInput * motorForce;
        }

        if (isBraking)
        {
            rearLeftWheelCollider.brakeTorque = brakeForce * 2f;
            rearRightWheelCollider.brakeTorque = brakeForce * 2f;
            frontLeftWheelCollider.brakeTorque = brakeForce * 2f;
            frontRightWheelCollider.brakeTorque = brakeForce * 2f;
        }
        else
        {
            rearLeftWheelCollider.brakeTorque = 0f;
            rearRightWheelCollider.brakeTorque = 0f;
            frontLeftWheelCollider.brakeTorque = 0f;
            frontRightWheelCollider.brakeTorque = 0f;

            frontLeftWheelCollider.motorTorque = motorInput;
            frontRightWheelCollider.motorTorque = motorInput;

            ApplyBraking(); // Normaali jarru (S)
        }
    }

    private void ApplyBraking()
    {
        float frontBrake = currentBrakeForce * 0.5f;
        float rearBrake = currentBrakeForce * 0.5f;

        frontLeftWheelCollider.brakeTorque = frontBrake;
        frontRightWheelCollider.brakeTorque = frontBrake;
        rearLeftWheelCollider.brakeTorque = rearBrake;
        rearRightWheelCollider.brakeTorque = rearBrake;
    }

    private void HandleSteering()
    {
        float targetSteerAngle = maxSteerAngle * horizontalInput;
        currentSteerAngle = Mathf.SmoothDamp(currentSteerAngle, targetSteerAngle, ref steerAngleVelocity, 0.3f);

        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider collider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void SetWheelFriction()
    {
        WheelFrictionCurve forwardFriction = new WheelFrictionCurve
        {
            extremumSlip = 0.4f,
            extremumValue = 1.5f,
            asymptoteSlip = 0.8f,
            asymptoteValue = 1.2f,
            stiffness = 2.0f
        };

        WheelFrictionCurve sidewaysFriction = new WheelFrictionCurve
        {
            extremumSlip = 0.2f,
            extremumValue = 1.5f,
            asymptoteSlip = 0.5f,
            asymptoteValue = 1.2f,
            stiffness = 1.7f
        };

        ApplyFriction(frontLeftWheelCollider, forwardFriction, sidewaysFriction);
        ApplyFriction(frontRightWheelCollider, forwardFriction, sidewaysFriction);
        ApplyFriction(rearLeftWheelCollider, forwardFriction, sidewaysFriction);
        ApplyFriction(rearRightWheelCollider, forwardFriction, sidewaysFriction);
    }

    private void ApplyFriction(WheelCollider wheel, WheelFrictionCurve forward, WheelFrictionCurve sideways)
    {
        wheel.forwardFriction = forward;
        wheel.sidewaysFriction = sideways;
    }

    private void SetSuspension()
    {
        JointSpring frontSpring = new JointSpring
        {
            spring = 45000f,
            damper = 5500f,
            targetPosition = 0.5f
        };

        JointSpring rearSpring = new JointSpring
        {
            spring = 40000f,
            damper = 5000f,
            targetPosition = 0.5f
        };

        frontLeftWheelCollider.suspensionSpring = frontSpring;
        frontRightWheelCollider.suspensionSpring = frontSpring;
        rearLeftWheelCollider.suspensionSpring = rearSpring;
        rearRightWheelCollider.suspensionSpring = rearSpring;

        frontLeftWheelCollider.suspensionDistance = 0.15f;
        frontRightWheelCollider.suspensionDistance = 0.15f;
        rearLeftWheelCollider.suspensionDistance = 0.15f;
        rearRightWheelCollider.suspensionDistance = 0.15f;
    }
    private void UpdateEngineSound()
    {
        float speed = rb.velocity.magnitude; // metriä sekunnissa
        float pitch = Mathf.Lerp(minPitch, maxPitch, speed / 20f); // skaalaa 0–50 m/s nopeusalueelle
        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, pitch, Time.deltaTime * pitchSmoothing);

        if (!engineAudioSource.isPlaying)
        {
            engineAudioSource.Play();
        }
    }




}