using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PortalWave
{
    public EnemyData enemy = null;
    public int amountMin = 0;
    public int amountMax = 0;
    public float cooldown = 0f;
}
