using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour, IInteractable
{
    [SerializeField] private MinionData data = null;
    [SerializeField] private MovementController movementController = null;

    protected MinionState currentState = null;
    protected MinionIdle idleState = null;
    protected MinionDelivering deliveringState = null;
    protected MinionGoDelivering goDeliveringState = null;
    protected MinionFarming farmingState = null;
    protected MinionGoFarming goFarmingState = null;
    protected MinionBuilding buildingState = null;
    protected MinionGoBuilding goBuildingState = null;

    private LevelManager levelManager = null;
    private Nekromancer master = null;
    private FarmTower farmTower = null;
    private Crystal crystal = null;
    private float currentFarmAmount = 0;
    private void Start()
    {
        levelManager = GameManager.Instance as LevelManager;

        movementController = GetComponent<MovementController>();

        levelManager.OnCycleChanged += OnCycleChanged;

        FindTower();
    }

    public void FindTower() {
        List<FarmTower> farmTowers = levelManager.FarmTower;
        if(farmTowers.Count > 0)
        {
            farmTower = levelManager.FarmTower[Random.Range(0, farmTowers.Count)];
        }
    }

    private void Update()
    {
        if(!farmTower)
            FindTower();

        if(!crystal)
            crystal = levelManager.crystal;

        if (!farmTower || !crystal)
            return;
        // if (!master || !crystal || !farmTower)
        // return;

        if (currentFarmAmount < data.carryCapacity)
        {
            if (Vector3.Distance(transform.position, farmTower.transform.position) > data.distanceThreshold)
            {
                Transform freeNeighbour = farmTower.GetFreeNeighbour();
                if(freeNeighbour)
                    movementController.TargetPosition = freeNeighbour.position;
                else 
                    print("No free neighbour");
                // movingEntity.TargetPosition = farmTower.GetFreeNeighbour().position;
                // movingEntity.Move();
            }
            else
            {
                currentFarmAmount += data.farmSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, crystal.transform.position) > data.distanceThreshold * 3)
            {
                movementController.TargetPosition = crystal.GetFreeNeighbour().position;
                // transform.position = Vector3.MoveTowards(transform.position, crystal.transform.position, data.moveSpeed * Time.deltaTime);
            }
            else
            {
                currentFarmAmount = 0;
            }
        }

        // if (currentFarmAmount < data.carryCapacity)
        // {
        //     if (Vector3.Distance(transform.position, farmTower.transform.position) > data.distanceThreshold)
        //     {
        //         transform.position = Vector3.MoveTowards(transform.position, farmTower.transform.position, data.moveSpeed * Time.deltaTime);
        //     }
        //     else
        //     {
        //         currentFarmAmount += data.farmSpeed * Time.deltaTime;
        //     }
        // }
        // else
        // {
        //     if (Vector3.Distance(transform.position, crystal.transform.position) > data.distanceThreshold * 3)
        //     {
        //         transform.position = Vector3.MoveTowards(transform.position, crystal.transform.position, data.moveSpeed * Time.deltaTime);
        //     }
        //     else
        //     {
        //         currentFarmAmount = 0;
        //     }
        // }
    }


    private void OnCycleChanged(CycleState _cycle)
    {
        if (_cycle.Cycle == Cycle.Night)
        {
            Instantiate(data.portal.prefab, transform.position, Quaternion.identity);
            ((LevelManager)GameManager.Instance).OnCycleChanged -= OnCycleChanged;
            Destroy(gameObject);
        }
    }

    public void Interact(Nekromancer _nekromancer)
    {
        master = _nekromancer;
        // crystal = _nekromancer.PlayerController.Crystal;

        master.OnInteract += MasterInteracted;
        print("Interacting with minion");
    }

    public void MasterInteracted(IInteractable _interactable)
    {
        print("Master interacted");
        if (_interactable is FarmTower)
        {
            print("Master interacted with farm tower");
            farmTower = (FarmTower)_interactable;
            master.OnInteract -= MasterInteracted;
        }
    }

    public void InteractEnd()
    {
        master.OnInteract -= MasterInteracted;
        print("Stopped interacting with minion");
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
