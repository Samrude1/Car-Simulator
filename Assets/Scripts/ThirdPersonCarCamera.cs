using UnityEngine;

public class ThirdPersonCarCamera : MonoBehaviour
{
    [Header("Target (Auto)")]
    public Transform target;

    [Header("Camera Settings")]
    public float distance = 6f;
    public float height = 2.5f;
    public float heightDamping = 2f;
    public float rotationDamping = 3f;
    public float tiltAmount = 2f;

    [Header("Look Settings")]
    public bool lookAtTarget = true;
    public Vector3 lookOffset = new Vector3(0, 1.5f, 0);

    private float currentRotationAngle;
    private float currentHeight;

    void LateUpdate()
    {
        if (!target) return;

        // Haetaan auton kulma
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Kameran suunta
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Kameran sijainti (selän taakse, hieman korkealle)
        Vector3 cameraPosition = target.position - currentRotation * Vector3.forward * distance;
        cameraPosition.y = currentHeight;

        transform.position = cameraPosition;

        // Valinnainen kevyesti kallistuva kamera käännöksissä
        float horizontalInput = Input.GetAxis("Horizontal");
        float tilt = Mathf.Lerp(transform.localEulerAngles.z, -horizontalInput * tiltAmount, Time.deltaTime * 2f);
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, tilt);

        // Kamera katsoo autoa, jos valittu
        if (lookAtTarget)
        {
            transform.LookAt(target.position + lookOffset);
        }
    }
}
