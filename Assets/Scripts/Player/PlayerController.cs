using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private Crystal crystal = null;
    [SerializeField] public Nekromancer nekromancer = null;
    [SerializeField] private Nekromancer nekromancerPrefab = null;
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private CursorController cursor = null;
    [SerializeField] private CinemachineVirtualCamera nekromancerCamera = null;
    [SerializeField] private CinemachineVirtualCamera cursorCamera = null;

    public LevelManager LevelManager { get; private set; }
    public WorldGrid WorldGrid { get; private set; }
    public GridElement CurrentGridElement { get; private set; }
    public GridElement LastGridElement { get; private set; }
    public InputController InputController { get; private set; }
    public CursorController Cursor { get { return cursor; } }
    public Crystal Crystal { get => crystal; }

    public void Init(GameManager _levelManager, InputController _inputController)
    {
        LevelManager levelManager = _levelManager as LevelManager;
        if (levelManager != null)
        {
            LevelManager = levelManager;
            WorldGrid = levelManager.WorldGrid;
            LevelManager.OnCycleChanged += ChangeDayTime;
        }

        InputController = _inputController;

        if (!nekromancer)
            nekromancer = Instantiate(nekromancerPrefab, spawnPoint.position, Quaternion.identity);

        nekromancer.Init(this);
        cursor.Init(this);

        crystal.OnCrystalDestroyed += CrystalDestroyed;
    }

    public void CrystalDestroyed()
    {
        print("Crystal destroyed");
    }

    public void ChangeDayTime(CycleState _cycleState)
    {
        switch (_cycleState.Cycle)
        {
            case Cycle.Day:
                nekromancerCamera.Priority = 0;
                cursorCamera.Priority = 1;
                cursor.SetCursorActive(true);
                cursor.OnCursorMoved += UpdateGrid;
                nekromancer.InputController = null;
                break;

            case Cycle.Night:
                nekromancerCamera.Priority = 1;
                cursorCamera.Priority = 0;
                cursor.SetCursorActive(false);
                cursor.OnCursorMoved -= UpdateGrid;
                UpdateGrid(new Vector2(int.MaxValue, int.MaxValue));
                nekromancer.InputController = InputController;
                break;
        }
    }

    private void UpdateGrid(Vector2 _position)
    {
        CurrentGridElement = WorldGrid.GetGridElement(_position);

        if (CurrentGridElement != null)
        {
            if (LastGridElement && LastGridElement != CurrentGridElement)
            {
                LastGridElement.SetElementActive(false);
            }

            CurrentGridElement.SetElementActive(true);
            LastGridElement = CurrentGridElement;
        }
    }
}