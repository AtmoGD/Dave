using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlacedObject
{
    public PlacedObject(Placeable placeable, Vector2Int gridPosition)
    {
        this.placeable = placeable;
        this.gridPosition = gridPosition;
    }

    public Placeable placeable = null;
    public Vector2Int gridPosition = Vector2Int.zero;
}

[CreateAssetMenu(fileName = "Level", menuName = "Game/Level")]
public class LevelData : ScriptableObject
{
    public List<CycleState> cycleStates = new List<CycleState>();

    public Vector2Int levelSize = new Vector2Int(10, 10);
    public List<PlacedObject> placedObjects = new List<PlacedObject>();
}
