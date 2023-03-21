using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkType
{
    Farming,
    Building
}


[CreateAssetMenu(fileName = "Minion", menuName = "Game/Minion")]
public class MinionData : Placeable
{
    public PortalData portal = null;
    public GameObject spawnEnemy = null;
    public WorkType preferredWork = WorkType.Farming;

    public float moveSpeed = 5f;
    public float farmSpeed = 1f;
    public float buildSpeed = 1f;
    public float deliverSpeed = 1f;
    public int carryCapacity = 10;

    public float distanceThreshold = 0.1f;
    public float spawnRadiusAroundTower = 3f;

    public AnimationCurve chanceToTurnIntoPortal = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
}
