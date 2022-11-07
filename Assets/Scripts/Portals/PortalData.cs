using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Portal", menuName = "Game/Portal")]
public class PortalData : ScriptableObject
{
    public GameObject prefab = null;
    public int health = 100;
    public float spawnRadius = 2f;
    public List<PortalWave> waves = new List<PortalWave>();
}
