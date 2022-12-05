using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Farm Tower", menuName = "Tower/Farm Tower")]
public class FarmTowerData : TowerData
{
    public Ressource ressource;
    public int maxWorkers = 5;
}
