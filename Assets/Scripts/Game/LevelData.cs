using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Game/Level")]
public class LevelData : ScriptableObject
{
    public List<CycleState> cycleStates = new List<CycleState>();
}
