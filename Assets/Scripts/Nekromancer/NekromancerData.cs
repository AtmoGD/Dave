using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NekromancerData", menuName = "Nekromancer/NekromancerData")]
public class NekromancerData : ScriptableObject
{
    public float moveSpeed = 5f;
    public float lookSpeed = 5f;

    public int health = 100;
    public int mana = 100;
    public float damage = 10f;
    public float attackSpeed = 1f;
    public float attackRange = 1f;
    public float soulMultiplikator = 1f;
    public float dayTimeMultiplier = 1f;

    public float interactRadius = 1f;
    public float moveThreshold = 0.1f;
    public float velocityActionThreshold = 0.1f;
    public float lookThreshold = 0.1f;
    public float accleleration = 0.1f;
}
