using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour
{
    public string perkName;
    public string perkDescription;

    protected PlayerController player;

    public virtual void ActivatePerk(PlayerController _player)
    {
        player = _player;
    }
}
