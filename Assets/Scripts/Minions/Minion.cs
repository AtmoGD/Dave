using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour, IInteractable
{
    [SerializeField] private MinionData data = null;

    private void Start()
    {
        ((LevelManager)GameManager.Instance).OnCycleChanged += OnCycleChanged;
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
        print("Interact");
    }
}
