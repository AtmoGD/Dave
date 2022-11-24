using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private string dataPath = "player.dave";
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private InputController inputController = null;
    [SerializeField] public Nekromancer nekromancer = null;
    [SerializeField] private Nekromancer nekromancerPrefab = null;
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private CursorController cursor = null;
    [SerializeField] private CinemachineVirtualCamera nekromancerCamera = null;
    [SerializeField] private CinemachineVirtualCamera cursorCamera = null;
    [SerializeField] private PlayerUIController UIController = null;

    public LevelManager LevelManager { get; private set; }
    public PlayerData PlayerData => playerData;
    public WorldGrid WorldGrid { get; private set; }
    public GridElement CurrentGridElement { get; private set; }
    public GridElement LastGridElement { get; private set; }
    public CursorController Cursor { get { return cursor; } }
    public Placeable CurrentPlaceable { get; private set; }
    public GameObject CurrentPlaceableVizualizer { get; private set; }
    public Animator VizualizerAnimator { get; private set; }
    public List<GridElement> CurrentPlaceableGridElements { get; private set; }

    private const string dayActionMap = "Building";
    private const string nightActionMap = "Combat";

    public void Init(GameManager _levelManager)
    {
        playerData = DataLoader.LoadData<PlayerData>(dataPath);
        if (playerData == null)
        {
            playerData = new PlayerData();
            DataLoader.SaveData(playerData, dataPath);
        }

        LevelManager levelManager = _levelManager as LevelManager;
        if (levelManager != null)
        {
            LevelManager = levelManager;
            WorldGrid = levelManager.WorldGrid;
            LevelManager.OnCycleChanged += ChangeDayTime;
        }

        if (!nekromancer)
            nekromancer = Instantiate(nekromancerPrefab, spawnPoint.position, Quaternion.identity);

        CurrentPlaceable = null;
        CurrentPlaceableGridElements = new List<GridElement>();
        nekromancer.InputController = inputController;
        nekromancer.Init(this);
    }

    public void LoadData(string _path)
    {
        playerData = DataLoader.LoadData<PlayerData>(_path);
        if (playerData == null)
        {
            playerData = new PlayerData();
            DataLoader.SaveData(playerData, _path);
        }
    }

    public void SaveData(string _path)
    {
        DataLoader.SaveData<PlayerData>(playerData, _path);
    }

    public void ChangeDayTime(CycleState _cycleState)
    {
        switch (_cycleState.Cycle)
        {
            case Cycle.Day:
                StartDay();
                break;

            case Cycle.Night:
                StartNight();
                break;
        }
    }

    private void StartDay()
    {
        inputController.ChangeActionMap(dayActionMap);

        nekromancerCamera.Priority = 0;
        nekromancer.InputController = null;

        cursorCamera.Priority = 1;
        cursor.SetCursorActive(true);

        cursor.OnCursorMoved += UpdateGrid;

        inputController.OnInteract += Interact;
        inputController.OnOpenBuildMenu += OpenBuildingsMenu;
        inputController.OnOpenMinionMenu += OpenMinionsMenu;
        inputController.OnCancel += Cancel;

    }

    private void StartNight()
    {
        inputController.ChangeActionMap(nightActionMap);

        nekromancerCamera.Priority = 1;
        nekromancer.InputController = inputController;

        cursorCamera.Priority = 0;
        cursor.SetCursorActive(false);

        // This will reset the selected GridElement
        UpdateGrid(new Vector2(int.MaxValue, int.MaxValue));

        cursor.OnCursorMoved -= UpdateGrid;

        inputController.OnInteract -= Interact;
        inputController.OnOpenBuildMenu -= OpenBuildingsMenu;
        inputController.OnOpenMinionMenu -= OpenMinionsMenu;
        inputController.OnCancel -= Cancel;

        // Close menus if the night startet while they were open
        Cancel(null);
    }

    private void UpdateGrid(Vector2 _position)
    {
        Vector3 worldPos;
        StaticLib.GetWorldPosition(_position, out worldPos);

        CurrentGridElement = WorldGrid.GetGridElement(worldPos, true);

        if (CurrentGridElement != null)
        {
            if (LastGridElement && LastGridElement != CurrentGridElement)
            {
                LastGridElement.SetElementActive(false);
            }

            CurrentGridElement.SetElementActive(true);
            LastGridElement = CurrentGridElement;
        }
        else
        {
            if (LastGridElement)
                LastGridElement.SetElementActive(false);
        }

        if (CurrentPlaceable)
        {
            CurrentPlaceableVizualizer.transform.position = CurrentGridElement.transform.position;

            VizualizerAnimator.SetBool("IsPlaceable", IsObjectPlaceable());

            foreach (GridElement gridElement in CurrentPlaceableGridElements)
            {
                gridElement.SetElementActive(false);
            }

            CurrentPlaceableGridElements.Clear();

            CurrentPlaceableGridElements = GetGridElementsInRange(CurrentGridElement.gridPosition, CurrentPlaceable.size);

            foreach (GridElement gridElement in CurrentPlaceableGridElements)
            {
                gridElement.SetElementActive(true);
            }
        }
    }

    public void PlaceObject(Placeable _object)
    {
        CurrentPlaceable = _object;

        // if (!CurrentGridElement) return;

        CurrentPlaceableVizualizer = Instantiate(CurrentPlaceable.preview, CurrentGridElement.transform.position, Quaternion.identity);
        VizualizerAnimator = CurrentPlaceableVizualizer.GetComponent<Animator>();

        // if (CurrentGridElement)
        // {
        //     if (CurrentGridElement.objectOnGrid == null)
        //     {
        //         CurrentGridElement.IndicateIsPlaceable();
        //     }
        //     else
        //     {
        //         CurrentGridElement.IndicateIsNotPlaceable();
        //     }
        // }
    }

    public List<GridElement> GetGridElementsInRange(Vector2Int _pos, Vector2Int _size)
    {
        List<GridElement> gridElements = new List<GridElement>();

        // int xStart = _pos.x;
        // int xEnd = _pos.x + _size.x;

        // int yStart = _pos.y;
        // int yEnd = _pos.y + _size.y;

        // int xMiddle = xStart + (_size.x / 2);
        // int yMiddle = yStart + (_size.y / 2);

        // int collums = _size.x;
        // int rows = _size.y;

        // int currentX = _pos.x;
        // int currentY = _pos.y;

        // for (int x = 0; x < collums; x++)
        // {
        //     for (int y = 0; y < rows; y++)
        //     {
        //         Vector2Int pos = new Vector2Int(currentX, currentY + y);
        //         GridElement gridElement = WorldGrid.GetGridElement(pos);
        //         if (gridElement)
        //         {
        //             gridElements.Add(gridElement);
        //         }

        //         currentY++;
        //     }

        //     currentX++;
        //     currentY--;
        // }



        // for (int x = _pos.x; x < _pos.x + _size.x; x++)
        // {
        //     for (int y = _pos.y; y < _pos.y + _size.y; y++)
        //     {
        //         gridElements.Add(WorldGrid.GetGridElement(x, y));
        //     }
        // }



        // Vector2Int currentPos = _pos;
        // for (int x = _pos.x + 1; x < _pos.x + _size.x; x++)
        // {
        //     currentPos.x = x;
        //     for (int y = _pos.y; y < _pos.y + _size.y; y++)
        //     {
        //         currentPos.y = y;
        //         gridElements.Add(WorldGrid.GetGridElement(currentPos));
        //     }
        // }

        // for (int i = -range; i <= range; i++)
        // {
        //     for (int j = -range; j <= range; j++)
        //     {
        //         Vector2Int gridPos = new Vector2Int(pos.x + i, pos.y + j);
        //         GridElement gridElement = WorldGrid.GetGridElement(gridPos);
        //         if (gridElement)
        //         {
        //             gridElements.Add(gridElement);
        //         }
        //     }
        // }

        // for (int x = pos.x - range; x <= pos.x + range; x++)
        // {
        //     for (int y = pos.y - range; y <= pos.y + range; y++)
        //     {
        //         Vector2Int gridPos = new Vector2Int(x, y);
        //         GridElement gridElement = WorldGrid.GetGridElement(gridPos);
        //         if (gridElement)
        //         {
        //             gridElements.Add(gridElement);
        //         }
        //     }
        // }

        return gridElements;
    }

    public bool IsObjectPlaceable()
    {
        Vector2Int size = CurrentPlaceable.size;
        GridElement[][] grid = LevelManager.WorldGrid.Grid;
        Vector2Int gridPos = CurrentGridElement.gridPosition;

        List<GridElement> gridElements = GetGridElementsInRange(gridPos, size);

        foreach (GridElement gridElement in gridElements)
        {
            if (gridElement.objectOnGrid != null)
            {
                return false;
            }
        }

        return true;

        // if (size > 1)
        // {
        //     size--;
        //     for (int i = 0; i < size; i++)
        //     {
        //         for (int j = 0; j < size; j++)
        //         {
        //             if (grid[gridPos.x + i][gridPos.y + j].objectOnGrid != null)
        //             {
        //                 return false;
        //             }
        //         }
        //     }
        // }
        // else if (CurrentGridElement.objectOnGrid == null)
        // {
        //     return true;
        // }
        // else
        // {
        //     return false;
        // }

        // return true;
    }

    private void Interact(InputData _inputData)
    {
        if (CurrentPlaceable && CurrentGridElement && IsObjectPlaceable())
        {
            GameObject newObject = Instantiate(CurrentPlaceable.prefab, CurrentGridElement.transform.position, Quaternion.identity);
            CurrentGridElement.objectOnGrid = newObject;

            Destroy(CurrentPlaceableVizualizer);
            CurrentPlaceable = null;

        }
    }

    private void OpenBuildingsMenu(InputData _input)
    {
        UIController.OpenBuildingsMenu();
    }

    private void OpenMinionsMenu(InputData _input)
    {
        UIController.OpenMinionsMenu();
    }

    private void Cancel(InputData _input)
    {
        UIController.CLoseAllMenus();
        CurrentPlaceable = null;
    }
}