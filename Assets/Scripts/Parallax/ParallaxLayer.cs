using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;

    public void Move(float delta)
    {
        transform.localPosition -= new Vector3(delta * parallaxFactor, 0, 0);
    }
}
