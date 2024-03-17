using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
    public ParallaxCamera parallaxCamera;
    private List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    void Start()
    {
        InitializeParallaxCamera();
        InitializeParallaxLayers();
    }

    private void InitializeParallaxCamera()
    {
        parallaxCamera = parallaxCamera ?? Camera.main.GetComponent<ParallaxCamera>();
        if (parallaxCamera != null)
        {
            parallaxCamera.onCameraTranslate += UpdateParallaxEffect;
        }
    }

    private void InitializeParallaxLayers()
    {
        parallaxLayers.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            var layer = transform.GetChild(i).GetComponent<ParallaxLayer>();
            if (layer != null)
            {
                layer.name = $"Layer-{i}";
                parallaxLayers.Add(layer);
            }
        }
    }

    private void UpdateParallaxEffect(float delta)
    {
        foreach (var layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}
