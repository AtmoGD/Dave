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

    private void Start()
    {
        gridOffset = new Vector2(-gridSize.x / 2f, -gridSize.y / 2f) * gridElementSize;

        InitGrid();
    }

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
                gridElement.gridPosition = new Vector2Int(x, y);
                gridElement.worldGrid = this;

                grid[x][y] = gridElement;
                ElementCount++;
            }
        }
    }

    public void InitGrid()
    {
        grid = new GridElement[gridSize.x][];
        for (int x = 0; x < gridSize.x; x++)
        {
            grid[x] = new GridElement[gridSize.y];
        }

        foreach (Transform child in transform)
        {
            GridElement gridElement = child.GetComponent<GridElement>();
            if (gridElement != null)
            {
                grid[gridElement.gridPosition.x][gridElement.gridPosition.y] = gridElement;
                ElementCount++;
            }
        }
    }

    public GridElement GetGridElement(Vector2 _worldPosition)
    {
        Vector2 gridPosition = _worldPosition - gridOffset;
        gridPosition /= gridElementSize;

        int x = Mathf.RoundToInt(gridPosition.x);
        int y = Mathf.RoundToInt(gridPosition.y);

        if (x >= 0 && y >= 0 && x < gridSize.x && y < gridSize.y)
            return grid[x][y];

        return null;
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

    [ExecuteAlways]
    public void DeleteAllChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
