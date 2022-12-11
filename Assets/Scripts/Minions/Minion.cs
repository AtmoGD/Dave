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

// private void Update()
// {
//     CurrentState?.FrameUpdate();
//     // if(!farmTower)
//     //     FindTower();

//     // if(!crystal)
//     //     crystal = levelManager.Crystal;

//     // if (!farmTower || !crystal)
//     //     return;
//     // // if (!master || !crystal || !farmTower)
//     // // return;

//     // if (currentFarmAmount < data.carryCapacity)
//     // {
//     //     if (Vector3.Distance(transform.position, farmTower.transform.position) > data.distanceThreshold)
//     //     {
//     //         Transform freeNeighbour = farmTower.GetFreeNeighbour();
//     //         if(freeNeighbour)
//     //             movementController.TargetPosition = freeNeighbour.position;
//     //         else 
//     //             print("No free neighbour");
//     //         // movingEntity.TargetPosition = farmTower.GetFreeNeighbour().position;
//     //         // movingEntity.Move();
//     //     }
//     //     else
//     //     {
//     //         currentFarmAmount += data.farmSpeed * Time.deltaTime;
//     //     }
//     // }
//     // else
//     // {
//     //     if (Vector3.Distance(transform.position, crystal.transform.position) > data.distanceThreshold * 3)
//     //     {
//     //         movementController.TargetPosition = crystal.GetFreeNeighbour().position;
//     //         // transform.position = Vector3.MoveTowards(transform.position, crystal.transform.position, data.moveSpeed * Time.deltaTime);
//     //     }
//     //     else
//     //     {
//     //         currentFarmAmount = 0;
//     //     }
//     // }

//     // if (currentFarmAmount < data.carryCapacity)
//     // {
//     //     if (Vector3.Distance(transform.position, farmTower.transform.position) > data.distanceThreshold)
//     //     {
//     //         transform.position = Vector3.MoveTowards(transform.position, farmTower.transform.position, data.moveSpeed * Time.deltaTime);
//     //     }
//     //     else
//     //     {
//     //         currentFarmAmount += data.farmSpeed * Time.deltaTime;
//     //     }
//     // }
//     // else
//     // {
//     //     if (Vector3.Distance(transform.position, crystal.transform.position) > data.distanceThreshold * 3)
//     //     {
//     //         transform.position = Vector3.MoveTowards(transform.position, crystal.transform.position, data.moveSpeed * Time.deltaTime);
//     //     }
//     //     else
//     //     {
//     //         currentFarmAmount = 0;
//     //     }
//     // }
// }

// private void OnCycleChanged(CycleState _cycle)
// {
//     if (_cycle.Cycle == Cycle.Night)
//     {
//         Instantiate(Data.portal.prefab, transform.position, Quaternion.identity);
//         ((LevelManager)GameManager.Instance).OnCycleChanged -= OnCycleChanged;
//         Destroy(gameObject);
//     }
// }


// public void FindTower()
// {
//     List<FarmTower> farmTowers = LevelManager.FarmTower;
//     if (farmTowers.Count > 0)
//     {
//         TargetTower = LevelManager.FarmTower[Random.Range(0, farmTowers.Count)];
//     }
// }

