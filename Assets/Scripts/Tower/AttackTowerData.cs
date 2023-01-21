using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Tower", menuName = "Tower/Attack Tower")]
public class AttackTowerData : TowerData
{
    public int damage = 10;
    public float attackSpeed = 1f;
}
