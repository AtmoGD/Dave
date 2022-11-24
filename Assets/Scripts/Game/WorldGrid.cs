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
    private Vector2 gridOffset = Vector2.zero;

    private void Start()
    {
        elementSize = new Vector2(gridElementSize.x * isometricRatio.x, gridElementSize.y * isometricRatio.y);

        gridOffset = new Vector2(elementSize.x / 2f, gridSize.y * elementSize.y);

        InitGrid();
    }

    [ExecuteAlways]
    public void CreateGrid()
    {
        DeleteAllChildren();

        elementSize = new Vector2(gridElementSize.x * isometricRatio.x, gridElementSize.y * isometricRatio.y);

        // gridOffset = new Vector2(elementSize.x / 2f, gridSize.y * elementSize.y);

        grid = new GridElement[gridSize.x][];

        // Vector2 currentPos = Vector2.zero;

        for (int x = 0; x < gridSize.x; x++)
        {
            grid[x] = new GridElement[gridSize.y];

            // currentPos.x = x * (elementSize.x / 2f);
            // currentPos.y = x * -(elementSize.y);

            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject gridElementObject = Instantiate(gridElementPrefab, transform);
                gridElementObject.name = "Grid Element " + x + ", " + y;

                // Vector2 offset = currentPos;
                // offset.x = x * elementSize.x + ((x * elementSize.x) / 2);
                // offset.y = y * elementSize.y - (((y * elementSize.y) / 2));

                // offset.x = 
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



                // currentPos.x += (elementSize.x / 2f);
                // currentPos.y += elementSize.y;
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

    public List<GridElement> GetGridElements(Vector2 _gridPosition, Vector2Int _size)
    {
        List<GridElement> gridElements = new List<GridElement>();

        Vector2 currentPos = _gridPosition;

        for (int x = 0; x < _size.x; x++)
        {
            currentPos.x = x * (elementSize.x / 2f);
            currentPos.y = x * -(elementSize.y);

            for (int y = 0; y < _size.y; y++)
            {
                GridElement gridElement = GetGridElement(_gridPosition + currentPos);
                if (gridElement != null)
                {
                    gridElements.Add(gridElement);
                }

                currentPos.x += (elementSize.x / 2f);
                currentPos.y += elementSize.y;
            }
        }

        return gridElements;
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
        // Vector2 gridPosition = _worldPosition - gridOffset;

        // currentPos.x = x * (elementSize.x / 2f);
        // currentPos.y = x * -(elementSize.y);


        // float x = gridPosition.x - (y * (elementSize.x / 2f));
        // gridPosition.x /= elementSize.x;
        // gridPosition.y /= elementSize.y;
        // float y = (gridPosition.y / (elementSize.y)) + (elementSize.y / ((gridPosition.y == 0f ? 1f : gridPosition.y) / 2f));
        // // float y = (gridPosition.y / (elementSize.y / 2f)) - (gridPosition.x / (elementSize.x / 2f));
        // float x = (gridPosition.x / (elementSize.x / 2f)) - ((int)y * elementSize.y);
        // // float x = (gridPosition.x / (elementSize.x / 2f)) - (elementSize.x / 4f);
        // // float y = (gridPosition.y / (elementSize.y / 2f)) - (elementSize.y / 4f);
        // // float y = (gridPosition.y / (elementSize.y / 2f)) - (x * (elementSize.y / 2f));


        // int gridX = Mathf.RoundToInt(x);
        // int gridY = Mathf.RoundToInt(y);

        // // gridY = 0;

        // // gridX -= (int)gridOffset.x;
        // // gridY -= (int)gridOffset.y;


        // return grid[gridX][gridY];

        // Vector2 gridPosition = _worldPosition;

        // int y = Mathf.RoundToInt(gridPosition.y / elementSize.y);

        // if (y % 2 == 1)
        //     gridPosition.x -= elementSize.x / 2f;

        // int x = Mathf.RoundToInt(gridPosition.x / elementSize.x);

        // if (_forceToGetElement)
        // {
        //     x = Mathf.Clamp(x, 0, gridSize.x - 1);
        //     y = Mathf.Clamp(y, 0, gridSize.y - 1);
        // }
        // else if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y)
        // {
        //     return null;
        // }


        // return grid[x][y];
    }

    public GridElement GetGridElement(Vector2Int _gridPosition)
    {
        if (_gridPosition.x < 0 || _gridPosition.x >= gridSize.x || _gridPosition.y < 0 || _gridPosition.y >= gridSize.y)
        {
            return null;
        }

        return grid[_gridPosition.x][_gridPosition.y];
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
