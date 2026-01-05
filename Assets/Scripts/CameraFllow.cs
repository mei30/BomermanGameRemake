using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // The player to follow
    public Vector3 offset;         // Optional offset from the player
    public float smoothSpeed = 0.125f; // Smoothing factor

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
