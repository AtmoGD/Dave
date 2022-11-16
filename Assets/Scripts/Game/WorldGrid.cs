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
    [SerializeField] private Vector2 gridElementSize = new Vector2(1f, 0.578f);
    [SerializeField] private Vector2 isometricMultiplier = new Vector2(1f, 0.578f);
    // [SerializeField] private LayerMask gridElementLayerMask = 0;

    public GridElement[][] Grid { get { return grid; } }
    public int ElementCount { get; private set; }
    // private Vector2 gridOffset = Vector2.zero;
    private Vector2 elementSize = Vector2.zero;

    private void Start()
    {
        elementSize = new Vector2(gridElementSize.x * isometricMultiplier.x, gridElementSize.y * isometricMultiplier.y);

        // gridOffset = new Vector2(-gridSize.x / 2f, -gridSize.y / 2f) * gridElementSize;

        InitGrid();
    }

    [ExecuteAlways]
    public void CreateGrid()
    {
        ClearGrid();

        elementSize = new Vector2(gridElementSize.x * isometricMultiplier.x, gridElementSize.y * isometricMultiplier.y);

        // gridOffset = new Vector2(-gridSize.x / 2f, -gridSize.y / 2f) * elementSize;

        grid = new GridElement[gridSize.x][];

        for (int x = 0; x < gridSize.x; x++)
        {
            grid[x] = new GridElement[gridSize.y];

            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject gridElementObject = Instantiate(gridElementPrefab, transform);
                gridElementObject.name = "Grid Element " + x + ", " + y;

                Vector2 offset = new Vector2(x * elementSize.x, y * elementSize.y);
                if (y % 2 == 1)
                {
                    offset.x += elementSize.x / 2f;
                }

                // Vector3 gridElementPosition = (Vector3)offset + (Vector3)gridOffset;
                Vector3 gridElementPosition = (Vector3)offset;
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

        // int y = Mathf.RoundToInt(gridPosition.y / elementSize.y) - Mathf.RoundToInt(gridOffset.y);

        // if (y % 2 == 1)
        //     gridPosition.x -= elementSize.x / 2f;

        // int x = Mathf.RoundToInt(gridPosition.x / elementSize.x) - Mathf.RoundToInt(gridOffset.x);

        // Vector2 offset = new Vector2(x * elementSize.x, y * elementSize.y);
        // if (y % 2 == 1)
        // {
        //     offset.x += elementSize.x / 2f;
        // }

        // Vector3 gridElementPosition = (Vector3)offset + (Vector3)gridOffset;


        // Vector2 gridPosition = _worldPosition - gridOffset;

        // int y = Mathf.RoundToInt(gridPosition.y / elementSize.y);

        // if (y % 2 == 1)
        //     gridPosition.x -= elementSize.x / 2f;

        // int x = Mathf.RoundToInt(gridPosition.x / elementSize.x);

        // x *= elementSize.x;
        // y *= elementSize.y;

        // // x = Mathf.Clamp(x, 0, gridSize.x - 1);
        // // y = Mathf.Clamp(y, 0, gridSize.y - 1);

        // return grid[x][y];


        // Vector2 gridPosition = _worldPosition + gridOffset;

        // Vector2 offset = new Vector2(gridPosition.x / elementSize.x, gridPosition.y / elementSize.y);

        // int y = Mathf.RoundToInt(gridPosition.y / elementSize.y);

        // if (y % 2 == 1)
        //     gridPosition.x -= elementSize.x / 2f;

        // int x = Mathf.RoundToInt(gridPosition.x / elementSize.x);

        // x = Mathf.Clamp(x - Mathf.RoundToInt(offset.x), 0, gridSize.x - 1);
        // y = Mathf.Clamp(y - Mathf.RoundToInt(offset.y), 0, gridSize.y - 1);

        // return grid[x][y];









        // Vector2 offset = new Vector2(gridPosition.x % elementSize.x, gridPosition.y % elementSize.y);

        // int x = Mathf.RoundToInt(gridPosition.x / elementSize.x);
        // int y = Mathf.RoundToInt(gridPosition.y / elementSize.y);

        // if (y % 2 == 1)
        // {
        //     x = Mathf.RoundToInt((gridPosition.x - elementSize.x / 2f) / elementSize.x);
        // }

        // x = Mathf.Clamp(x, 0, gridSize.x - 1);
        // y = Mathf.Clamp(y, 0, gridSize.y - 1);

        // return grid[x][y];








        // int y = Mathf.RoundToInt(offset.y);
        // if (y % 2 == 1)
        //     offset.x -= 0.5f;
        // int x = Mathf.RoundToInt(offset.x);



        // int y = Mathf.RoundToInt(gridPosition.y / elementSize.y);

        // if (y % 2 == 1)
        //     gridPosition.x -= elementSize.x / 2f;

        // int x = Mathf.RoundToInt(gridPosition.x / elementSize.x);

        // x = Mathf.Clamp(x, 0, gridSize.x - 1);
        // y = Mathf.Clamp(y, 0, gridSize.y - 1);

        // return grid[x][y];

        // Vector2 gridPosition = _worldPosition - gridOffset;
        // // if (Mathf.RoundToInt(gridPosition.y) % 2 == 1)
        // // {
        // //     gridPosition.x += elementSize.x / 2f;
        // // }

        // float y = ((gridPosition.y / elementSize.y) + (gridOffset.y / 2f));
        // if (Mathf.RoundToInt(y) % 2 == 1)
        // {
        //     gridPosition.x -= elementSize.x / 2f;
        // }
        // float x = gridPosition.x / elementSize.x;

        // int roundX = Mathf.Clamp(Mathf.RoundToInt(x), 0, gridSize.x - 1);
        // int roundY = Mathf.Clamp(Mathf.RoundToInt(y + (elementSize.y)), 0, gridSize.y - 1);

        // return grid[roundX][roundY];

        // Vector3 gridElementPosition = (Vector3)_worldPosition - (Vector3)gridOffset;
        // if (Mathf.RoundToInt(gridElementPosition.y) % 2 == 1)
        // {
        //     gridElementPosition.x += elementSize.x / 2f;
        // }
        // Vector2 offset = new Vector2(gridElementPosition.x / elementSize.x, (gridElementPosition.y / elementSize.y) + (gridOffset.y / 2f));

        // int x;
        // int y;

        // x = Mathf.Clamp(Mathf.RoundToInt(offset.x), 0, gridSize.x - 1);
        // y = Mathf.Clamp(Mathf.RoundToInt(offset.y), 0, gridSize.y - 1);

        // return grid[x][y];


        Vector2 gridPosition = _worldPosition;

        int y = Mathf.RoundToInt(gridPosition.y / elementSize.y);

        if (y % 2 == 1)
            gridPosition.x -= elementSize.x / 2f;

        int x = Mathf.RoundToInt(gridPosition.x / elementSize.x);

        x = Mathf.Clamp(x, 0, gridSize.x - 1);
        y = Mathf.Clamp(y, 0, gridSize.y - 1);

        return grid[x][y];
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

        grid = null;
        ElementCount = 0;
    }
}
