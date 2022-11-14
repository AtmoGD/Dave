using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WorldGrid : MonoBehaviour
{
    [SerializeField] private GameObject gridElementPrefab = null;
    [SerializeField] private GridElement[][] grid = null;
    [SerializeField] private Vector2Int gridSize = Vector2Int.zero;
    [SerializeField] private float gridElementSize = 1f;

    public GridElement[][] Grid { get { return grid; } }
    public int ElementCount { get; private set; }
    private Vector2 gridOffset = Vector2.zero;

    [ExecuteAlways]
    public void CreateGrid()
    {
        ClearGrid();

        gridOffset = new Vector2(-gridSize.x / 2f, -gridSize.y / 2f) * gridElementSize;

        grid = new GridElement[gridSize.x][];

        for (int x = 0; x < gridSize.x; x++)
        {
            grid[x] = new GridElement[gridSize.y];

            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject gridElementObject = Instantiate(gridElementPrefab, transform);
                gridElementObject.name = "Grid Element " + x + ", " + y;

                Vector3 gridElementPosition = new Vector3(x, y, 0) * gridElementSize + (Vector3)gridOffset;
                gridElementObject.transform.position = gridElementPosition;

                GridElement gridElement = gridElementObject.GetComponent<GridElement>();
                gridElement.GridPosition = new Vector2Int(x, y);
                gridElement.WorldGrid = this;

                grid[x][y] = gridElement;
                ElementCount++;
            }
        }
    }

    [ExecuteAlways]
    public void ClearGrid()
    {
        if (grid != null)
        {
            for (int x = 0; x < grid.Length; x++)
            {
                for (int y = 0; y < grid[x].Length; y++)
                {
                    DestroyImmediate(grid[x][y].gameObject);
                }
            }
            grid = null;
            ElementCount = 0;
        }
    }
}
