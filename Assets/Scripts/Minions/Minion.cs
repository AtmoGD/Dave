using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour, IInteractable
{
    [field: SerializeField] public MinionData Data { get; private set; } = null;
    [field: SerializeField] public MovementController MoveController { get; private set; } = null;

    public MinionState CurrentState = null;
    public MinionIdle IdleState { get; private set; } = new MinionIdle();
    public MinionMoving MovingState { get; private set; } = new MinionMoving();
    public MinionDelivering DeliveringState { get; private set; } = new MinionDelivering();
    public MinionFarming FarmingState { get; private set; } = new MinionFarming();
    public MinionBuilding BuildingState { get; private set; } = new MinionBuilding();

    public LevelManager LevelManager { get; private set; } = null;

    public Nekromancer Master { get; set; } = null;
    public FarmTower TargetFarmTower { get; set; } = null;
    public Tower TargetTower { get; set; } = null;
    public float CurrentFarmAmount { get; set; } = 0f;

    private void Start()
    {
        LevelManager = GameManager.Instance as LevelManager;

        MoveController = GetComponent<MovementController>();

        ChangeState(IdleState);
    }

    private void Update()
    {
        CurrentState?.FrameUpdate();

        if (LevelManager.CurrentCycleState.Cycle == Cycle.Night)
            EvaluateTurnIntoPortal();
    }

    private void FixedUpdate()
    {
        CurrentState?.PhysicsUpdate();
    }

    public void ChangeState(MinionState _newState)
    {
        CurrentState?.Exit();

        CurrentState = _newState;

        CurrentState?.Enter(this);
    }

    public void EvaluateTurnIntoPortal()
    {
        float chance = Data.chanceToTurnIntoPortal.Evaluate(LevelManager.CurrentCycleState.TimeInCycle) * 100;
        float roll = Random.Range(0, 100);

        if (roll < chance)
        {
            Instantiate(Data.portal.prefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void Interact(Nekromancer _nekromancer)
    {
        Master = _nekromancer;

        Master.OnInteract += MasterInteracted;
        print("Interacting with minion");
    }

    public void MasterInteracted(IInteractable _interactable)
    {
        print("Master interacted");
        if (_interactable is FarmTower)
        {
            print("Master interacted with farm tower");
            TargetTower = (FarmTower)_interactable;
            Master.OnInteract -= MasterInteracted;
        }
    }

    public void InteractEnd()
    {
        Master.OnInteract -= MasterInteracted;
        print("Stopped interacting with minion");
    }

    public FarmTower FindFarmTower()
    {
        List<FarmTower> farmTowers = LevelManager.FarmTower;

        farmTowers.RemoveAll(tower => !tower.IsBuilt);

        return GetRandomTower(farmTowers);
    }

    public Tower FindUnBuildTower()
    {
        List<Tower> towers = LevelManager.Tower;

        towers.RemoveAll(tower => tower.IsBuilt);

        return GetRandomTower(towers);
    }

    public T GetRandomTower<T>(List<T> tower) where T : Tower
    {
        if (tower.Count == 0)
            return null;

        int randomIndex = Random.Range(0, tower.Count);

        return tower[randomIndex];
    }

    public Transform GetTransform()
    {
        return transform;
    }
}