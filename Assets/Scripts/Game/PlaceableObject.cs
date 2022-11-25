using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public GridElement gridElement = null;
    public List<GridElement> placedOnGridElements = new List<GridElement>();
}
