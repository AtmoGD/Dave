using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WorldGrid : MonoBehaviour
{
    [field: SerializeField] public LevelData LevelData { get; private set; } = null;
    [field: SerializeField] public List<GameObject> PlacedObjects { get; private set; } = new List<GameObject>();
    [field: SerializeField] public Transform GridElementsParent { get; private set; } = null;
    [field: SerializeField] public Transform ObjectParent { get; private set; } = null;
    [SerializeField] private GameObject gridElementPrefab = null;
    [SerializeField] private Vector2 gridElementSize = new Vector2(2f, 1f);
    [SerializeField] private Vector2 isometricRatio = new Vector2(2f, 1f);

    public GridElement[][] Grid { get; private set; } = null;
    public Vector2Int GridSize => LevelData.levelSize;
    public int ElementCount { get; private set; }
    public Vector2 ElementSize => new Vector2(gridElementSize.x * isometricRatio.x, gridElementSize.y * isometricRatio.y);

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

        Grid = new GridElement[GridSize.x][];

        for (int x = 0; x < GridSize.x; x++)
        {
            Grid[x] = new GridElement[GridSize.y];

            for (int y = 0; y < GridSize.y; y++)
            {

                Vector2 elementPosition = new Vector2(x * ElementSize.x, y * ElementSize.y);
                if (y % 2 == 1)
                    elementPosition.x += ElementSize.x / 2f;

                GameObject gridElementObject = Instantiate(gridElementPrefab, elementPosition, Quaternion.identity, GridElementsParent);
                gridElementObject.name = "Grid Element " + x + ", " + y;

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

        foreach (Transform child in GridElementsParent.transform)
        {
            GridElement gridElement = child.GetComponent<GridElement>();
            if (gridElement != null)
            {
                Grid[gridElement.gridPosition.x][gridElement.gridPosition.y] = gridElement;
                ElementCount++;
            }
        }
    }

    public void PlaceObject(Placeable _object, Vector2Int _gridPosition)
    {
        GridElement gridElement = GetGridElement(_gridPosition);

        GameObject newObj = Instantiate(_object.prefab, gridElement.transform.position + (Vector3)GetObjectOffset(_object), Quaternion.identity, ObjectParent);

        PlacedObjects.Add(newObj);

        List<GridElement> gridElements = GetGridElements(gridElement.transform.position, _object.size);

        foreach (GridElement elem in gridElements)
            elem.ObjectOnGrid = newObj;

        PlaceableObject placeableObject = newObj.GetComponent<PlaceableObject>();

        if (placeableObject != null)
        {
            placeableObject.gridElement = gridElement;
            placeableObject.placedOnGridElements = gridElements;
        }
    }

    public void RemoveObject(GameObject _object)
    {
        if (PlacedObjects.Remove(_object))
        {
            PlaceableObject placeableObject = _object.GetComponent<PlaceableObject>();
            if (placeableObject != null)
            {
                foreach (GridElement gridElement in placeableObject.placedOnGridElements)
                {
                    gridElement.ObjectOnGrid = null;
                }
            }
            DestroyImmediate(_object);
        }
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
            currentPos.x = x * (ElementSize.x / 2f);
            currentPos.y = x * -(ElementSize.y);

            for (int y = 0; y < _size.y; y++)
            {
                GridElement gridElement = GetGridElement(_gridPosition + currentPos);
                if (gridElement != null)
                {
                    gridElements.Add(gridElement);
                }

                currentPos.x += (ElementSize.x / 2f);
                currentPos.y += ElementSize.y;
            }
        }

        return gridElements;
    }

    public GridElement GetGridElement(Vector2 _worldPosition, bool _forceToGetElement = false)
    {
        Vector2 gridPosition = _worldPosition;
        int y = Mathf.RoundToInt(gridPosition.y / ElementSize.y);
        if (y % 2 == 1)
            gridPosition.x -= ElementSize.x / 2f;

        int x = Mathf.RoundToInt(gridPosition.x / ElementSize.x);

        if (_forceToGetElement)
        {
            x = Mathf.Clamp(x, 0, GridSize.x - 1);
            y = Mathf.Clamp(y, 0, GridSize.y - 1);
        }
        else
        {
            if (x < 0 || x >= GridSize.x || y < 0 || y >= GridSize.y)
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
        Vector2 offset = Vector2.zero;

        offset.x = (_object.size.x - 1) * (ElementSize.x / 2f);

        return offset;
    }

    [ExecuteAlways]
    public void DeleteAllChildren()
    {
        for (int i = GridElementsParent.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(GridElementsParent.transform.GetChild(i).gameObject);
        }

        for (int i = ObjectParent.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(ObjectParent.transform.GetChild(i).gameObject);
        }

        Grid = null;
        ElementCount = 0;
        PlacedObjects.Clear();
    }
}
