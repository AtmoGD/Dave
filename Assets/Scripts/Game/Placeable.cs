using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Placeable : ScriptableObject
{
    public string id = "";
    public new string name = "";
    public string description = "";
    public List<Ressource> cost = new List<Ressource>();
    // public Vector2Int size = new Vector2Int(1, 1);
    public Vector2Int size = new Vector2Int(1, 1);
    public GameObject prefab = null;
    public GameObject preview = null;

}
