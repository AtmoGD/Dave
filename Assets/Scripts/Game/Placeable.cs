using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Placeable : ScriptableObject
{
    public string id = "";
    public new string name = "";
    public string description = "";
    public Vector2Int size = new Vector2Int(1, 1);
    public GameObject prefab = null;

}
