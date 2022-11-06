using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string id = "Enemy";
    public new string name = "Enemy";
    public string description = "Enemy description";
    public Enemy enemyPrefab = null;
    public int health = 100;
    public int damage = 10;
    public int experience = 10;
}
