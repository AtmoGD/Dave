using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICycleState : MonoBehaviour
{
    [SerializeField] private RectTransform fillTransform = null;
    [SerializeField] private Image fillImage = null;
    [SerializeField] private AnimationCurve alphaCurve = null;
    [SerializeField] private AnimationCurve widthCurve = null;
    [SerializeField] private AnimationCurve heightCurve = null;
    // [SerializeField] private float fullWidth = 1200f;
    // [SerializeField] private float fullHeight = 200f;
    // [SerializeField] private float minHeight = 160f;

    private void Update()
    {

        if (LevelManager.Instance.IsDay)
        {
            float percent = LevelManager.Instance.CurrentCycleState.PercentOfTimeLeft;
            float alpha = alphaCurve.Evaluate(percent);
            float posY = heightCurve.Evaluate(percent);

            // float width = fullWidth * percent;
            // float height = Remap(alpha, 0f, 1f, minHeight, fullHeight);
            // float height = heightCurve.Evaluate(percent) * fullHeight;

            Vector2 size = fillTransform.sizeDelta;
            size.x = widthCurve.Evaluate(percent);

            // float width = widthCurve.Evaluate(percent);
            // float height = heightCurve.Evaluate(percent);

            // fillTransform.sizeDelta = new Vector2(width, height);
            fillTransform.sizeDelta = size;
            fillTransform.anchoredPosition = new Vector2(0f, posY);
            fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, alpha);
        }
    }

    //Remap Function
    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
