using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Speed Perk", menuName = "Perk/Attack Speed Perk")]
public class AttackSpeedPerk : Perk
{
    public float attackSpeedIncrease;
    public override void ActivatePerk(PlayerController _player)
    {
        base.ActivatePerk(_player);

        _player.Nekromancer.AddAttackSpeed(attackSpeedIncrease);
    }
}
