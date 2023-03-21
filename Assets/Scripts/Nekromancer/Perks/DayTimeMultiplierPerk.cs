using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Day Multiplier Perk", menuName = "Perk/Day Multiplier Perk")]
public class DayTimeMultiplierPerk : Perk
{
    public float tmeMultiplier;
    public override void ActivatePerk(PlayerController _player)
    {
        base.ActivatePerk(_player);

        _player.Nekromancer.AddDayTimeMultiplier(tmeMultiplier);
    }
}
