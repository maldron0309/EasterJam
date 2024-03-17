using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public event ParallaxCameraDelegate onCameraTranslate;

    private float lastPositionX;

    void Start()
    {
        lastPositionX = transform.position.x;
    }

    void Update()
    {
        float currentPositionX = transform.position.x;
        if (currentPositionX != lastPositionX)
        {
            float deltaMovement = lastPositionX - currentPositionX;
            onCameraTranslate?.Invoke(deltaMovement);

            lastPositionX = currentPositionX;
        }
    }
}
