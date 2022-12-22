using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTower : Tower
{
    List<Minion> minions = new List<Minion>();

    public Ressource GetRessource()
    {
        return ((FarmTowerData)towerData).ressource;
    }
}
