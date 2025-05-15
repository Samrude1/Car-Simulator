using UnityEngine;

public class AdvancedThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;          // Auto
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -7f);
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Tilting")]
    [SerializeField] private float maxTiltAngle = 10f;  // Kuinka paljon kamera kallistuu sivulle
    [SerializeField] private float tiltSpeed = 3f;

    [Header("Zoom by Speed")]
    [SerializeField] private Rigidbody carRigidbody;
    [SerializeField] private float minZoom = -6f;
    [SerializeField] private float maxZoom = -12f;
    [SerializeField] private float zoomSensitivity = 0.05f;

    private float currentTilt = 0f;

    void LateUpdate()
    {
        if (!target || !carRigidbody) return;

        // Peruspaikka + dynaaminen zoom nopeuden mukaan
        float speed = carRigidbody.velocity.magnitude;
        float zoom = Mathf.Lerp(minZoom, maxZoom, speed * zoomSensitivity);
        Vector3 dynamicOffset = new Vector3(offset.x, offset.y, zoom);

        // Kameran sijainti
        Vector3 desiredPosition = target.position + target.TransformDirection(dynamicOffset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Kameran suuntaus kohti autoa
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // Laske kallistus käännöksestä
        float horizontalInput = Input.GetAxis("Horizontal");
        float targetTilt = -horizontalInput * maxTiltAngle;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        transform.Rotate(0f, 0f, currentTilt); // z-akselin kallistus
    }
}
