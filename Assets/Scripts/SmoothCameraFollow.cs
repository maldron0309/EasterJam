using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Tooltip("The target for camera follow")]
    public Transform target; // The player or object the camera will follow

    [Tooltip("How quickly the camera will move to target")]
    public float smoothSpeed = 0.125f; // More or less smoothly

    [Tooltip("Offset from the target position")]
    public Vector3 offset; // Distance between camera and the target

    // FixedUpdate makes camera movement smoother.
    void FixedUpdate()
    {
        if (target != null)
        {
            // Calculate position based on target position/offset
            Vector3 desiredPosition = target.position + offset;

            // Smoothly transition for camera's current position and end position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Apply
            transform.position = smoothedPosition;
        }
    }
}
