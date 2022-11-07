using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour, IInteractable
{
    [SerializeField] private MinionData data = null;

    private Nekromancer master = null;
    [SerializeField] private FarmTower farmTower = null;
    private Crystal crystal = null;
    private float currentFarmAmount = 0;

    private void Start()
    {
        ((LevelManager)GameManager.Instance).OnCycleChanged += OnCycleChanged;
    }

    private void Update()
    {
        if (!master || !crystal || !farmTower)
            return;

        if (currentFarmAmount < data.carryCapacity)
        {
            if (Vector3.Distance(transform.position, farmTower.transform.position) > data.distanceThreshold)
            {
                transform.position = Vector3.MoveTowards(transform.position, farmTower.transform.position, data.moveSpeed * Time.deltaTime);
            }
            else
            {
                currentFarmAmount += data.farmSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, crystal.transform.position) > data.distanceThreshold)
            {
                transform.position = Vector3.MoveTowards(transform.position, crystal.transform.position, data.moveSpeed * Time.deltaTime);
            }
            else
            {
                currentFarmAmount = 0;
            }
        }
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
        crystal = _nekromancer.PlayerController.Crystal;

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
}
