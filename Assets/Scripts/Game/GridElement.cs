using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GridElement : MonoBehaviour
{
    [SerializeField] public WorldGrid WorldGrid { get; set; }
    [SerializeField] public Vector2Int GridPosition { get; set; }
    [SerializeField] public Placeable ObjectOnGrid { get; set; }
}
