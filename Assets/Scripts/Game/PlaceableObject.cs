using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public LevelManager LevelManager { get; private set; } = null;
    public GridElement GridElement { get; set; } = null;
    public List<GridElement> PlacedOnGridElements { get; set; } = new List<GridElement>();

    public virtual void Start()
    {
        this.LevelManager = LevelManager.Instance;
    }
}
