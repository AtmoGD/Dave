using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private string dataPath = "player.dave";
    [SerializeField] private PlayerData playerData = null;
    public PlayerData PlayerData => playerData;

    [SerializeField] private InputController inputController = null;
    public InputController InputController => inputController;

    [SerializeField] private Transform spawnPoint = null;

    [field: SerializeField] public CinemachineVirtualCamera NekromancerCamera { get; private set; } = null;
    [field: SerializeField] public CinemachineVirtualCamera BuildingCamera { get; private set; } = null;
    [field: SerializeField] public Nekromancer Nekromancer { get; private set; } = null;
    [field: SerializeField] public PlayerUIController UIController { get; private set; } = null;
    [field: SerializeField] public PlayerBuildController BuildController { get; private set; } = null;
    [field: SerializeField] public PlayerInput PlayerInput { get; private set; } = null;

    public int PerkPoints { get; set; } = 0;
    public List<Perk> Perks { get; private set; } = new List<Perk>();
    public List<Perk> UnPerks { get; private set; } = new List<Perk>();

    private const string combatActionMap = "Combat";
    private const string buildActionMap = "Building";
    private const string uiActionMap = "UI";

    bool stoppedTime = false;

    public void Init()
    {
        LoadData(dataPath);

        UIController.gameObject.SetActive(true);
        UIController.Init();

        BuildController.Init(this);

        Nekromancer.InputController = inputController;
        Nekromancer.Init(this);

        StartCombatMode();
    }

    public void LoadData(string _path)
    {
        try
        {
            playerData = DataLoader.LoadData<PlayerData>(_path);
        }
        catch (System.Exception e)
        {
            playerData = new PlayerData();
            SaveData(_path);
            Debug.Log("Error during load data: " + e);
        }
    }

    public void SaveData(string _path)
    {
        DataLoader.SaveData<PlayerData>(playerData, _path);
    }

    public void ChoosePerkInput(InputAction.CallbackContext _context)
    {
        if (_context.performed && PerkPoints > 0 && LevelManager.Instance.CurrentCycleState.Cycle == Cycle.Day)
        {
            OpenPerksMenu();
            // ChoosePerk();
        }
    }

    // public void ChoosePerk()
    // {
    //     UIController.OpenChoosePerkMenu();
    // }

    public void AddPerk(Perk _perk)
    {
        Perks.Add(_perk);
    }

    public void StartBuildingMode()
    {
        PlayerInput.SwitchCurrentActionMap(buildActionMap);

        NekromancerCamera.Priority = 0;

        BuildingCamera.Priority = 1;
    }

    public void StartCombatMode()
    {
        PlayerInput.SwitchCurrentActionMap(combatActionMap);

        NekromancerCamera.Priority = 1;

        BuildingCamera.Priority = 0;

        Cancel(null);
    }

    public void PlaceObject(Placeable _object)
    {
        BuildController.PlaceObject(_object);
    }

    public void PlaceMinion(MinionData _minionData)
    {
        Vector3 position = Nekromancer.CurrentInteractable.GetTransform().position + Random.insideUnitSphere * _minionData.spawnRadiusAroundTower;
        GameObject placeableObject = Instantiate(_minionData.prefab, position, Quaternion.identity);

        Nekromancer.ResetInteractable();

        Nekromancer.AddCooldown(new Cooldown("Interact", 0.5f));

        Cancel(null);
    }


    public void OpenBuildingsMenu(InputData _input = null)
    {
        UIController.OpenBuildingsMenu();

        PlayerInput.SwitchCurrentActionMap(uiActionMap);

        if (!stoppedTime) LevelManager.Instance.StopTime();
    }

    public void OpenPerksMenu(InputData _input = null)
    {
        UIController.OpenChoosePerkMenu();

        PlayerInput.SwitchCurrentActionMap(uiActionMap);

        if (!stoppedTime) LevelManager.Instance.StopTime();
    }

    public void OpenMinionsMenu(InputData _input = null)
    {
        UIController.OpenMinionsMenu();

        PlayerInput.SwitchCurrentActionMap(uiActionMap);

        if (!stoppedTime) LevelManager.Instance.StopTime();
    }

    public void Cancel(InputData _input = null)
    {
        UIController.CloseMenu();

        Nekromancer.ResetInteractable();

        PlayerInput.SwitchCurrentActionMap(combatActionMap);

        if (stoppedTime) LevelManager.Instance.StartTime();
    }
}