using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedPerk : Perk
{
    public float attackSpeedIncrease;
    public override void ActivatePerk(PlayerController _player)
    {
        base.ActivatePerk(_player);

        player.Nekromancer.AddAttackSpeed(attackSpeedIncrease);
    }
}
