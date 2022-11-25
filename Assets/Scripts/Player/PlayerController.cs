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
            Vector3 objectOffset = LevelManager.WorldGrid.GetObjectOffset(CurrentPlaceable);

            CurrentPlaceableVizualizer.transform.position = CurrentGridElement.transform.position + objectOffset;

            List<GridElement> newGridElements = WorldGrid.GetGridElements(CurrentGridElement.transform.position, CurrentPlaceable.size);

            bool isPlaceable = LevelManager.WorldGrid.IsObjectPlaceable(CurrentPlaceable, newGridElements);

            VizualizerAnimator.SetBool("IsPlaceable", isPlaceable);

            foreach (GridElement gridElement in CurrentPlaceableGridElements)
            {
                gridElement.SetElementActive(false);
            }

            CurrentPlaceableGridElements.Clear();

            CurrentPlaceableGridElements = newGridElements;

            foreach (GridElement gridElement in CurrentPlaceableGridElements)
            {
                if (gridElement == CurrentGridElement)
                    continue;

                gridElement.SetElementActive(true);
            }
        }
    }

    public void PlaceObject(Placeable _object)
    {
        CurrentPlaceable = _object;
        Vector3 objectOffset = LevelManager.WorldGrid.GetObjectOffset(CurrentPlaceable);

        CurrentPlaceableVizualizer = Instantiate(CurrentPlaceable.preview, CurrentGridElement.transform.position + objectOffset, Quaternion.identity);
        VizualizerAnimator = CurrentPlaceableVizualizer.GetComponent<Animator>();
    }



    private void Interact(InputData _inputData)
    {
        if (CurrentPlaceable &&
        CurrentGridElement &&
        LevelManager.WorldGrid.IsObjectPlaceable(CurrentPlaceable, CurrentPlaceableGridElements))
        {
            Vector3 objectOffset = LevelManager.WorldGrid.GetObjectOffset(CurrentPlaceable);

            GameObject newObject = Instantiate(CurrentPlaceable.prefab, CurrentGridElement.transform.position + objectOffset, Quaternion.identity);

            foreach (GridElement gridElement in CurrentPlaceableGridElements)
            {
                gridElement.ObjectOnGrid = newObject;
            }

            Destroy(CurrentPlaceableVizualizer);

            CurrentPlaceableVizualizer = null;
            CurrentPlaceable = null;
            CurrentPlaceableGridElements.Clear();
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