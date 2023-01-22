using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Add Health Perk", menuName = "Perk/Add Health Perk")]
public class AddHealthPerk : Perk
{
    public int healthAmount;
    public override void ActivatePerk(PlayerController _player)
    {
        base.ActivatePerk(_player);

        _player.Nekromancer.AddHealth(healthAmount);
    }
}