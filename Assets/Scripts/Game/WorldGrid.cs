using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using System;
using Unity.Jobs;

[Serializable]
public class WorldGrid : MonoBehaviour
{
    public static WorldGrid Instance { get; protected set; }
    [field: SerializeField] public List<GameObject> PlacedObjects { get; private set; } = new List<GameObject>();
    [field: SerializeField] public Transform GridElementsParent { get; private set; } = null;
    [field: SerializeField] public Transform ObjectParent { get; private set; } = null;
    [field: SerializeField] public Transform backgroundParent { get; private set; } = null;
    [field: SerializeField] public Pathfinding Pathfinder { get; private set; } = null;
    [SerializeField] private GameObject gridElementPrefab = null;
    [SerializeField] private Vector2 gridElementSize = new Vector2(2f, 1f);
    [SerializeField] private Vector2 isometricRatio = new Vector2(2f, 1f);
    [SerializeField] private bool logErrors = true;

    public GridElement[][] Grid { get; private set; } = null;
    public Vector2Int GridSize
    {
        get
        {
            if (currentGameManager.GameState == GameState.Camp)
                return currentCampManager.CampSize;
            else
                return currentLevelManager.LevelData.levelSize;
        }
    }

    public GameManager currentGameManager
    {
        get
        {
            if (!GameManager.Instance)
                return FindObjectOfType<GameManager>();
            else
                return GameManager.Instance;
        }
    }

    public LevelManager currentLevelManager
    {
        get
        {
            if (!LevelManager.Instance)
                return FindObjectOfType<LevelManager>();
            else
                return LevelManager.Instance;
        }
    }

    public CampManager currentCampManager
    {
        get
        {
            if (!CampManager.Instance)
                return FindObjectOfType<CampManager>();
            else
                return CampManager.Instance;
        }
    }
    public int ElementCount { get; private set; }
    public Vector2 ElementSize => new Vector2(gridElementSize.x * isometricRatio.x, gridElementSize.y * isometricRatio.y);
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        LoadLevel();
    }

    public void FindPath(Vector2Int _startPos, Vector2Int _targetPos, MovementController _controller)
    {

        NativeArray<Pathfinding.GridStruct> gridElems = Pathfinder.ConvertGridToPathNodes(Grid);
        NativeList<int2> pathList = new NativeList<int2>(gridElems.Length, Allocator.Persistent);

        Pathfinding.FindPathJob findPathJob = new Pathfinding.FindPathJob
        {
            startPos = new int2(_startPos.x, _startPos.y),
            targetPos = new int2(_targetPos.x, _targetPos.y),
            gridElements = gridElems,
            gridSize = new int2(GridSize.x, GridSize.y),
            path = pathList
        };

        findPathJob.Run();

        List<Vector2Int> path = new List<Vector2Int>();

        for (int i = 0; i < findPathJob.path.Length; i++)
        {
            path.Add(new Vector2Int(findPathJob.path[i].x, findPathJob.path[i].y));
        }

        findPathJob.path.Dispose();
        findPathJob.gridElements.Dispose();

        _controller.TakePath(path);
    }

    public List<Vector2Int> FindPath(Vector2Int _startPos, Vector2Int _targetPos)
    {
        NativeArray<Pathfinding.GridStruct> gridElems = Pathfinder.ConvertGridToPathNodes(Grid);
        NativeList<int2> pathList = new NativeList<int2>(gridElems.Length, Allocator.Persistent);

        Pathfinding.FindPathJob findPathJob = new Pathfinding.FindPathJob
        {
            startPos = new int2(_startPos.x, _startPos.y),
            targetPos = new int2(_targetPos.x, _targetPos.y),
            gridElements = gridElems,
            gridSize = new int2(GridSize.x, GridSize.y),
            path = pathList
        };

        findPathJob.Run();

        List<Vector2Int> path = new List<Vector2Int>();

        for (int i = 0; i < findPathJob.path.Length; i++)
        {
            path.Add(new Vector2Int(findPathJob.path[i].x, findPathJob.path[i].y));
        }

        findPathJob.path.Dispose();
        findPathJob.gridElements.Dispose();

        return path;
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
        if (currentGameManager.GameState == GameState.Camp)
        {
            foreach (CampObjectData campObject in currentGameManager.PlayerController.PlayerData.placedObjects)
            {
                Placeable placeable = currentGameManager.DataList.GetPlaceable(campObject.id);
                Vector2Int gridPosition = new Vector2Int(campObject.posX, campObject.posY);
                PlaceObject(placeable, gridPosition);
            }

            Instantiate(currentCampManager.campPrefab, backgroundParent);
            return;
        }

        foreach (PlacedObject placedObject in currentLevelManager.LevelData.placedObjects)
        {
            PlaceObject(placedObject.placeable, placedObject.gridPosition);
        }

        Instantiate(currentLevelManager.LevelData.levelPrefab, backgroundParent);
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
        try
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
        catch (Exception e)
        {
            if (logErrors)
            {
                Debug.LogError("Error during InitGrid: " + e);
                Debug.LogError("GridSize: " + GridSize);
            }

            LoadLevel();
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
            placeableObject.GridElement = gridElement;
            placeableObject.PlacedOnGridElements = gridElements;
        }
    }

    public void RemoveObject(GameObject _object)
    {
        if (PlacedObjects.Remove(_object))
        {
            PlaceableObject placeableObject = _object.GetComponent<PlaceableObject>();
            if (placeableObject != null)
            {
                foreach (GridElement gridElement in placeableObject.PlacedOnGridElements)
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

    public GridElement GetGridElement(GridElement _element, Vector2 _dir)
    {
        Vector2 gridPosition = _element.transform.position;

        gridPosition.x += _dir.x * ElementSize.x;
        gridPosition.y += _dir.y * ElementSize.y;

        return GetGridElement(gridPosition, true);
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

        for (int i = backgroundParent.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(backgroundParent.transform.GetChild(i).gameObject);
        }

        Grid = null;
        ElementCount = 0;
        PlacedObjects.Clear();
    }
}
