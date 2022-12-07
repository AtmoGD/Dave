using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class StaticLib
{
    public static void GetWorldPosition(Vector2 _screenPosition, out Vector3 _worldPos)
    {
        _worldPos = Camera.main.ScreenToWorldPoint(_screenPosition);
        _worldPos.z = 0;
    }

    public static void GetScreenPosition(Vector3 _worldPosition, out Vector2 _screenPos)
    {
        _screenPos = Camera.main.WorldToScreenPoint(_worldPosition);
    }

    public static List<T> ShuffleList<T>(List<T> _list)
    {
        List<T> newList = new List<T>();
        int randomIndex = 0;
        while (_list.Count > 0)
        {
            randomIndex = UnityEngine.Random.Range(0, _list.Count);
            newList.Add(_list[randomIndex]);
            _list.RemoveAt(randomIndex);
        }
        return newList;
    }
}
