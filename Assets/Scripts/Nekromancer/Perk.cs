using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : ScriptableObject
{
    public Sprite perkIcon;
    public string id;
    public string perkName;
    public string perkDescription;

    public virtual void ActivatePerk(PlayerController _player)
    {
        _player.AddPerk(this);
    }
}
