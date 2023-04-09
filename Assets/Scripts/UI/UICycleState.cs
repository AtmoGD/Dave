using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class UICycleState : MonoBehaviour
{
    [SerializeField] private RectTransform fillTransform = null;
    [SerializeField] private Image fillImage = null;
    [SerializeField] private Transform volumeTransform = null;
    [SerializeField] private Light2D light2D = null;
    [SerializeField] private AnimationCurve alphaCurve = null;
    [SerializeField] private AnimationCurve widthCurve = null;
    [SerializeField] private AnimationCurve xScaleCurve = null;
    [SerializeField] private AnimationCurve yScaleCurve = null;
    [SerializeField] private AnimationCurve heightCurve = null;
    [SerializeField] private AnimationCurve volumeHeightCurve = null;
    [SerializeField] private AnimationCurve LightIntensityCurve = null;
    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.Level)
        {
            if (LevelManager.Instance.IsDay)
            {
                float percent = LevelManager.Instance.CurrentCycleState.PercentOfTimeLeft;
                float alpha = alphaCurve.Evaluate(percent);
                float posY = heightCurve.Evaluate(percent);
                float volumeY = volumeHeightCurve.Evaluate(percent);
                float lightIntensity = LightIntensityCurve.Evaluate(percent);

                Vector2 size = fillTransform.sizeDelta;
                size.x = widthCurve.Evaluate(percent);

                float xScale = xScaleCurve.Evaluate(percent);
                float yScale = yScaleCurve.Evaluate(percent);

                fillTransform.localScale = new Vector3(xScale, yScale, 1f);
                fillTransform.anchoredPosition = new Vector2(0f, posY);
                fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, alpha);

                volumeTransform.position = new Vector3(volumeTransform.position.x, volumeY, 0f);

                light2D.intensity = lightIntensity;
            }
        }
        else
        {
            fillTransform.localScale = Vector3.zero;
            volumeTransform.localScale = Vector3.zero;
            light2D.intensity = 0f;
            volumeTransform.position = new Vector3(volumeTransform.position.x, volumeTransform.position.y, 100f);
        }
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
