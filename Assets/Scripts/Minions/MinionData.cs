using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Minion", menuName = "Game/Minion")]
public class MinionData : Placeable
{
    public PortalData portal = null;
    public float moveSpeed = 5f;
    public float farmSpeed = 1f;
    public int carryCapacity = 10;
}
