using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGetMainCamera : MonoBehaviour
{
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
}
