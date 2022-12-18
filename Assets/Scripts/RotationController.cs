using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] private float rotationSpeedMin = 0f;
    [SerializeField] private float rotationSpeedMax = 0f;
    [SerializeField] private float rotationAngle = 360f;

    private float currentAngle = 0f;
    private float rotationSpeed = 0f;

    private void Start()
    {
        rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
    }

    [ExecuteAlways]
    private void Update()
    {
        currentAngle += rotationSpeed * Time.deltaTime;
        if (currentAngle >= rotationAngle)
        {
            currentAngle = 0f;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
