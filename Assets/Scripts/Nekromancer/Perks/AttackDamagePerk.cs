using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Damage Perk", menuName = "Perk/Attack Damage Perk")]
public class AttackDamagePerk : Perk
{
    public float attackDamageIncrease;
    public override void ActivatePerk(PlayerController _player)
    {
        base.ActivatePerk(_player);

        _player.Nekromancer.AddDamage(attackDamageIncrease);
    }
}
