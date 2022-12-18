using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleController : MonoBehaviour
{
    [SerializeField] private AnimationCurve scaleCurveMin = null;
    [SerializeField] private AnimationCurve scaleCurveMax = null;
    [SerializeField] private float scaleMultiplier = 0.05f;
    [SerializeField] private float scaleTime = 0f;

    private float currentTime = 0f;

    private void Start()
    {
        currentTime = scaleTime;
    }

    [ExecuteAlways]
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= scaleTime)
        {
            currentTime = 0f;
        }

        float scale = Mathf.Lerp(scaleCurveMin.Evaluate(currentTime / scaleTime), scaleCurveMax.Evaluate(currentTime / scaleTime), Mathf.PingPong(currentTime, scaleTime / 2f) / (scaleTime / 2f));
        scale *= scaleMultiplier;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
