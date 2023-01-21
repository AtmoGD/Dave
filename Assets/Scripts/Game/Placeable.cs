using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Placeable : ScriptableObject
{
    public string id = "";
    public new string name = "";
    public string description = "";
    public Sprite image = null;
    public List<CollectedRessource> cost = new List<CollectedRessource>();
    public Vector2Int size = new Vector2Int(1, 1);
    public GameObject prefab = null;
    public GameObject preview = null;

}
