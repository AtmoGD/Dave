using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunecircleLightingController : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    [SerializeField] private Transform centerPoint = null;
    [SerializeField] private float minIntensity = 1f;
    [SerializeField] private float maxIntensity = 0.5f;
    [SerializeField] private float minDistance = 0.5f;
    [SerializeField] private float maxDistance = 1f;

    private float intensity = 0f;

    private Nekromancer nekromancer = null;
    public Nekromancer Nekromancer
    {
        get
        {
            if (!nekromancer)
            {
                nekromancer = FindObjectOfType<Nekromancer>();
            }
            return nekromancer;
        }
    }

    private void Update()
    {
        UpdateIntensity();
    }

    private void UpdateIntensity()
    {
        if (!Nekromancer) return;

        float distance = Vector3.Distance(centerPoint.position, Nekromancer.transform.position);
        float distanceRatio = Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));

        intensity = Mathf.Lerp(minIntensity, maxIntensity, distanceRatio);

        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.material.SetFloat("_Intensity", intensity);
        }
    }
}
