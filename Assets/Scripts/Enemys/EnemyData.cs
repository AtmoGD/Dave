using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackTarget { Player, Tower, FarmTower, AttackTower }


[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string id = "Enemy";
    public new string name = "Enemy";
    public string description = "Enemy description";
    public AttackTarget prefferedAttackTarget = AttackTarget.Player;
    public Enemy enemyPrefab = null;
    public GameObject prefab = null;
    public int health = 100;
    public int damage = 10;
    public float attackSpeed = 1f;
    public float attackRange = 1f;
    public int experience = 10;
    public float speed = 3f;
    public float moveThreshold = 1f;
}
