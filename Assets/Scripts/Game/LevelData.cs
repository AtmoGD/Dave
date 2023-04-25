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

    [SerializeField] public Placeable placeable = null;
    [SerializeField] public Vector2Int gridPosition = Vector2Int.zero;
}

[CreateAssetMenu(fileName = "Level", menuName = "Game/Level"), Serializable]
public class LevelData : ScriptableObject
{
    [SerializeField] public List<CollectedRessource> startRessources = new List<CollectedRessource>();
    [SerializeField] public List<CycleState> cycleStates = new List<CycleState>();
    [SerializeField] public List<CycleState> cycleStatesLateGame = new List<CycleState>();
    [SerializeField] public Vector2Int levelSize = new Vector2Int(10, 10);
    [SerializeField] public List<PlacedObject> placedObjects = new List<PlacedObject>();
    [SerializeField] public GameObject levelPrefab = null;
    [SerializeField] public Vector2 NekromancerSpawnPosition = Vector2.zero;
}
