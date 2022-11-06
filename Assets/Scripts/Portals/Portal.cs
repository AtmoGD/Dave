using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private PortalData data = null;
    [SerializeField] private int health = 100;

    private float cooldown = 0f;

    private void Start()
    {
        health = data.health;
        SpawnEnemy();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        PortalWave randomWave = data.waves[Random.Range(0, data.waves.Count)];

        int amount = Random.Range(randomWave.amountMin, randomWave.amountMax);

        for (int i = 0; i < amount; i++)
        {
            Enemy enemy = EnemyPool.Instance.GetEnemy(randomWave.enemy);
        }
        cooldown = randomWave.cooldown;
    }


}
