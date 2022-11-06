using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticLib
{
    public static void GetWorldPosition(Vector2 _screenPosition, out Vector3 _worldPos)
    {
        _worldPos = Camera.main.ScreenToWorldPoint(_screenPosition);
        _worldPos.z = 0;
    }
}
