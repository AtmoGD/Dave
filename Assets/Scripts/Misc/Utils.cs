using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class Utils
{
    public static Direction ClampInFourDirections(Vector2 _dir)
    {
        if (Mathf.Abs(_dir.x) > Mathf.Abs(_dir.y))
        {
            return _dir.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            return _dir.y > 0 ? Direction.Up : Direction.Down;
        }
    }
}
