using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NekromancerData", menuName = "Nekromancer/NekromancerData")]
public class NekromancerData : ScriptableObject
{
    public float moveSpeed = 5f;
    public float lookSpeed = 5f;

    public float health = 100f;
    public float mana = 100f;
    public float baseDamage = 10f;

    public float interactRadius = 1f;
    public float moveThreshold = 0.1f;
    public float lookThreshold = 0.1f;
}
