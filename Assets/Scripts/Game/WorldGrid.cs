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
    [SerializeField] private Vector2 gridElementSize = new Vector2(2f, 1f);
    [SerializeField] private Vector2 isometricRatio = new Vector2(2f, 1f);

    [SerializeField] public GridElement[][] Grid { get { return grid; } }
    public int ElementCount { get; private set; }
    private Vector2 elementSize = Vector2.zero;

    private void Start()
    {
        elementSize = new Vector2(gridElementSize.x * isometricRatio.x, gridElementSize.y * isometricRatio.y);

        InitGrid();
    }

    [ExecuteAlways]
    public void CreateGrid()
    {
        DeleteAllChildren();

        elementSize = new Vector2(gridElementSize.x * isometricRatio.x, gridElementSize.y * isometricRatio.y);

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

    public GridElement GetGridElement(Vector2 _worldPosition, bool _forceToGetElement = false)
    {
        Vector2 gridPosition = _worldPosition;

        int y = Mathf.RoundToInt(gridPosition.y / elementSize.y);

        if (y % 2 == 1)
            gridPosition.x -= elementSize.x / 2f;

        int x = Mathf.RoundToInt(gridPosition.x / elementSize.x);

        if (_forceToGetElement)
        {
            x = Mathf.Clamp(x, 0, gridSize.x - 1);
            y = Mathf.Clamp(y, 0, gridSize.y - 1);
        }
        else if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y)
        {
            return null;
        }


        return grid[x][y];
    }

    // [ExecuteAlways]
    // public void ClearGrid()
    // {
    //     if (grid != null)
    //     {
    //         for (int x = 0; x < grid.Length; x++)
    //         {
    //             for (int y = 0; y < grid[x].Length; y++)
    //             {
    //                 DestroyImmediate(grid[x][y].gameObject);
    //             }
    //         }
    //         grid = null;
    //         ElementCount = 0;
    //     }
    // }

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
