using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WorldGrid : MonoBehaviour
{
    [field: SerializeField] public LevelData LevelData { get; private set; } = null;
    // [field: SerializeField] public GameObject ObjectParent { get; private set; } = null;
    [SerializeField] private GameObject gridElementPrefab = null;
    [SerializeField] private Vector2 gridElementSize = new Vector2(2f, 1f);
    [SerializeField] private Vector2 isometricRatio = new Vector2(2f, 1f);

    public GridElement[][] Grid { get; private set; } = null;
    public Vector2Int GridSize => LevelData.levelSize;
    public int ElementCount { get; private set; }

    private Vector2 elementSize = Vector2.zero;

    private void Start()
    {
        elementSize = new Vector2(gridElementSize.x * isometricRatio.x, gridElementSize.y * isometricRatio.y);

        InitGrid();
    }

    [ExecuteAlways]
    public void LoadLevel()
    {
        DeleteAllChildren();
        CreateGrid();
        PlaceLevelObjects();
    }

    [ExecuteAlways]
    public void PlaceLevelObjects()
    {
        foreach (PlacedObject placedObject in LevelData.placedObjects)
        {
            PlaceObject(placedObject.placeable, placedObject.gridPosition);
        }
    }

    [ExecuteAlways]
    public void CreateGrid()
    {
        DeleteAllChildren();

        elementSize = new Vector2(gridElementSize.x * isometricRatio.x, gridElementSize.y * isometricRatio.y);

        Grid = new GridElement[GridSize.x][];

        for (int x = 0; x < GridSize.x; x++)
        {
            Grid[x] = new GridElement[GridSize.y];

            for (int y = 0; y < GridSize.y; y++)
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

                Grid[x][y] = gridElement;
                ElementCount++;
            }
        }
    }

    [ExecuteAlways]
    public void InitGrid()
    {
        ElementCount = 0;

        Grid = new GridElement[GridSize.x][];
        for (int x = 0; x < GridSize.x; x++)
        {
            Grid[x] = new GridElement[GridSize.y];
        }

        foreach (Transform child in transform)
        {
            GridElement gridElement = child.GetComponent<GridElement>();
            if (gridElement != null)
            {
                Grid[gridElement.gridPosition.x][gridElement.gridPosition.y] = gridElement;
                ElementCount++;
            }

            PlaceableObject placedObject = child.GetComponent<PlaceableObject>();
            if (placedObject != null)
            {
                foreach (GridElement elem in placedObject.placedOnGridElements)
                {
                    elem.ObjectOnGrid = placedObject.gameObject;
                }
            }
        }


    }

    public void PlaceObject(GameObject _object, List<GridElement> _gridElements)
    {
        foreach (GridElement gridElement in _gridElements)
        {
            gridElement.ObjectOnGrid = _object;
        }

        PlaceableObject placeableObject = _object.GetComponent<PlaceableObject>();
        if (placeableObject != null)
            placeableObject.placedOnGridElements = _gridElements;
    }

    public void PlaceObject(Placeable _object, Vector2Int _gridPosition)
    {
        GridElement gridElement = GetGridElement(_gridPosition);
        GameObject newObj = Instantiate(_object.prefab, gridElement.transform.position + (Vector3)GetObjectOffset(_object), Quaternion.identity, transform);
        List<GridElement> gridElements = GetGridElements(_gridPosition, _object.size);
        PlaceObject(newObj, gridElements);
    }

    public bool IsObjectPlaceable(Placeable _object, List<GridElement> _gridElements)
    {
        foreach (GridElement gridElement in _gridElements)
        {
            if (gridElement.ObjectOnGrid != null)
            {
                return false;
            }
        }

        return true;
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
            x = Mathf.Clamp(x, 0, GridSize.x - 1);
            y = Mathf.Clamp(y, 0, GridSize.y - 1);
        }
        else if (x < 0 || x >= GridSize.x || y < 0 || y >= GridSize.y)
        {
            return null;
        }

        return Grid[x][y];
    }

    public GridElement GetGridElement(Vector2Int _gridPosition)
    {
        if (_gridPosition.x < 0 || _gridPosition.x >= GridSize.x || _gridPosition.y < 0 || _gridPosition.y >= GridSize.y)
        {
            return null;
        }

        return Grid[_gridPosition.x][_gridPosition.y];
    }

    public Vector2 GetObjectOffset(Placeable _object)
    {
        elementSize = new Vector2(gridElementSize.x * isometricRatio.x, gridElementSize.y * isometricRatio.y);

        Vector2 offset = Vector2.zero;

        offset.x = (_object.size.x - 1) * (elementSize.x / 2f);
        // offset.y = (_object.size.x - 1) * -(elementSize.y);

        return offset;
    }

    [ExecuteAlways]
    public void DeleteAllChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        Grid = null;
        ElementCount = 0;
    }
}
