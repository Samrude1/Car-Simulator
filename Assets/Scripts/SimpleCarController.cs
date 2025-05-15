using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeftCollider;
    public WheelCollider frontRightCollider;
    public WheelCollider rearLeftCollider;
    public WheelCollider rearRightCollider;

    [Header("Wheel Transforms")]
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform rearLeftWheel;
    public Transform rearRightWheel;

    [Header("Car Settings")]
    public float maxMotorTorque = 1500f;
    public float maxSteerAngle = 30f;

    private float motorInput;
    private float steerInput;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Center of mass
        rb.centerOfMass = new Vector3(0f, -0.9f, 0f);
    }
    void Update()
    {
        // Get input
        motorInput = Input.GetAxis("Vertical");   // W/S or Up/Down
        steerInput = Input.GetAxis("Horizontal"); // A/D or Left/Right
    }

    void FixedUpdate()
    {
        // Apply motor torque to rear wheels
        rearLeftCollider.motorTorque = motorInput * maxMotorTorque;
        rearRightCollider.motorTorque = motorInput * maxMotorTorque;

        // Apply steering to front wheels
        frontLeftCollider.steerAngle = steerInput * maxSteerAngle;
        frontRightCollider.steerAngle = steerInput * maxSteerAngle;

        // Update wheel meshes to match collider movement
        UpdateWheelPose(frontLeftCollider, frontLeftWheel);
        UpdateWheelPose(frontRightCollider, frontRightWheel);
        UpdateWheelPose(rearLeftCollider, rearLeftWheel);
        UpdateWheelPose(rearRightCollider, rearRightWheel);
    }

    void UpdateWheelPose(WheelCollider collider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
