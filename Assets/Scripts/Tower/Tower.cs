using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building, IDamagable
{
    [SerializeField] protected TowerData towerData = null;
    [SerializeField] protected List<Transform> neighbourGridElements = new List<Transform>();
    [SerializeField] protected DamageVisualizer damageVisualizerPrefab = null;

    public int Health { get; private set; }
    public int MaxHealth { get { return towerData.health; } }
    public float BuildPercentage { get; private set; } = 0f;
    public bool IsBuilt { get { return BuildPercentage >= 1f; } }

    private void Awake()
    {
        Health = towerData.health;
    }

    public override void Start()
    {
        base.Start();

        BuildPercentage = 1f;

        LevelManager.AddTower(this);
    }

    public virtual void TakeDamage(int _damage, GameObject _sender)
    {
        Health -= _damage;

        DamageVisualizer damageVisualizer = Instantiate(damageVisualizerPrefab, transform.position, Quaternion.identity);
        damageVisualizer.SetText(_damage.ToString());

        if (Health <= 0)
            Die();
    }

    public Transform GetFreeNeighbour()
    {
        List<Transform> freeNeighbours = StaticLib.ShuffleList<Transform>(neighbourGridElements);

        foreach (Transform neighbour in freeNeighbours)
        {
            GridElement gridElement = GameManager.Instance.WorldGrid.GetGridElement(neighbour.position);
            if (gridElement != null && gridElement.ObjectOnGrid == null)
                return neighbour;
        }

        return null;
    }

    public void WorkOnTower(float _workAmount)
    {
        _workAmount /= towerData.buildTime;

        BuildPercentage += _workAmount;

        if (BuildPercentage >= 1f)
            BuildPercentage = 1f;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
