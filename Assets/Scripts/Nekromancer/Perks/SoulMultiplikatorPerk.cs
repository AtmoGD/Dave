using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Soul Multiplikator Perk", menuName = "Perk/Soul Multiplikator Perk")]
public class SoulMultiplikatorPerk : Perk
{
    public float soulMultiplikator;
    public override void ActivatePerk(PlayerController _player)
    {
        base.ActivatePerk(_player);

        _player.Nekromancer.AddSoulMultiplikator(soulMultiplikator);
    }
}
