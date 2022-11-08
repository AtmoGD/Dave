using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class PoolData
{
    public EnemyData data = null;
    public int size = 10;
}

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance { get; private set; }

    [SerializeField] public List<PoolData> pools = new List<PoolData>();
    private List<Enemy> enemyList = new List<Enemy>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadAllPools();
    }

    private void LoadAllPools()
    {
        foreach (PoolData pool in pools)
        {
            LoadPool(pool);
        }
    }

    public void LoadPool(PoolData _pool)
    {
        for (int i = 0; i < _pool.size; i++)
        {
            Enemy enemy = Instantiate(_pool.data.enemyPrefab, transform);
            enemy.Init(_pool.data);
            enemy.gameObject.SetActive(false);
            enemyList.Add(enemy);
        }
    }

    public Enemy GetEnemy(EnemyData _enemyData)
    {
        Enemy enemy = enemyList.Find(e => e.Data == _enemyData && !e.gameObject.activeSelf);
        if (enemy == null)
        {
            enemy = Instantiate(_enemyData.enemyPrefab, transform);
            enemyList.Add(enemy);
        }
        enemy.gameObject.SetActive(true);
        return enemy;
    }
}