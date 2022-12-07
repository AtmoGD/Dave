using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : PlaceableObject, IDamagable, IInteractable
{
    [SerializeField] protected TowerData towerData = null;
    [SerializeField] protected List<Transform> neighbourGridElements = new List<Transform>();

    public int Health { get; private set; }

    private void Awake()
    {
        Health = towerData.health;
    }

    public override void Start()
    {
        base.Start();

        levelManager.AddTower(this);
    }

    public virtual void TakeDamage(int _damage)
    {
        Health -= _damage;
        if (Health <= 0)
            Die();
    }

    public Transform GetFreeNeighbour()
    {
        List<Transform> freeNeighbours = StaticLib.ShuffleList<Transform>(neighbourGridElements);

        foreach (Transform neighbour in freeNeighbours)
        {
            GridElement gridElement = levelManager.WorldGrid.GetGridElement(neighbour.position);
            if(gridElement != null && gridElement.ObjectOnGrid == null)
                return neighbour;
        }

        return null;
    }



    public void Interact(Nekromancer _nekromancer)
    {

    }

    public void InteractEnd()
    {

    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
