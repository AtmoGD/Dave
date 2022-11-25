using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : PlaceableObject, IDamagable, IInteractable
{
    [SerializeField] private TowerData towerData = null;

    public int Health { get; private set; }

    private void Awake()
    {
        Health = towerData.health;
    }

    public virtual void TakeDamage(int _damage)
    {
        Health -= _damage;
        if (Health <= 0)
            Die();
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
}
