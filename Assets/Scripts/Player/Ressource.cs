using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CollectedRessource
{
    public Ressource ressource;
    public int amount = 0;
}


[CreateAssetMenu(fileName = "Ressource", menuName = "Ressource", order = 1)]
public class Ressource : Collectable
{
    public GameObject prefab;
    public Sprite icon;
    public float weight = 1f;
}
