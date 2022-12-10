using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationAxis
{
    X,
    Y,
    Z
}

public class CopyRotation : MonoBehaviour
{
    public Transform source;
    public float offset;
    public RotationAxis sourceAxis;
    public RotationAxis targetAxis;
    public bool invert;

    void Update()
    {
        if (source)
        {
            float value = GetVector(sourceAxis) + offset;
            SetVector(targetAxis, value, invert);
        }

    }

    public float GetVector(RotationAxis axis)
    {
        switch (axis)
        {
            case RotationAxis.X:
                return source.rotation.eulerAngles.x;
            case RotationAxis.Y:
                return source.rotation.eulerAngles.y;
            case RotationAxis.Z:
                return source.rotation.eulerAngles.z;
        }

        return 0;
    }

    public void SetVector(RotationAxis axis, float value, bool invert = false)
    {
        Vector3 newRotation = transform.localRotation.eulerAngles;

        switch (axis)
        {
            case RotationAxis.X:
                newRotation.x = value;
                break;
            case RotationAxis.Y:
                newRotation.y = value;
                break;
            case RotationAxis.Z:
                newRotation.z = value;
                break;
        }

        if (newRotation != Vector3.zero)
            transform.localRotation = Quaternion.Euler(newRotation);
    }
}
