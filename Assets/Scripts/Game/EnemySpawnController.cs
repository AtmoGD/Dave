using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private List<EnemyData> enemyDatas = null;
    [SerializeField] private Vector2 spawnTimeRange = new Vector2(1f, 3f);
    [SerializeField] private float spawnRadius = 50f;

    private float spawnTime = 0f;

    private void Start()
    {
        spawnTime = Random.Range(spawnTimeRange.x, spawnTimeRange.y);
    }

    private void Update()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0f)
        {
            SpawnEnemy();
            spawnTime = Random.Range(spawnTimeRange.x, spawnTimeRange.y);
        }
    }

    private void SpawnEnemy()
    {
        Vector2 spawnPosition = Random.insideUnitCircle * spawnRadius;
        EnemyData enemyData = enemyDatas[Random.Range(0, enemyDatas.Count)];
        Enemy enemy = EnemyPool.Instance.GetEnemy(enemyData);
        enemy.transform.position = spawnPosition;
    }
}
