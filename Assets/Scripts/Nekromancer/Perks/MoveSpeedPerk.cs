using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move Speed Perk", menuName = "Perk/Move Speed Perk")]
public class MoveSpeedPerk : Perk
{
    public float moveSpeedIncrease;
    public override void ActivatePerk(PlayerController _player)
    {
        base.ActivatePerk(_player);

        _player.Nekromancer.AddSpeed(moveSpeedIncrease);
    }
}
