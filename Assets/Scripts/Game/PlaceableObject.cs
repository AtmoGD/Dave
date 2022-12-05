using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public LevelManager levelManager = null;
    public GridElement gridElement = null;
    public List<GridElement> placedOnGridElements = new List<GridElement>();

    public virtual void Start() {
        this.levelManager = GameManager.Instance as LevelManager;
    }
}
