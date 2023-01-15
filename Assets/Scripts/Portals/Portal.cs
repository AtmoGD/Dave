using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IDamagable
{
    [SerializeField] private PortalData data = null;
    [SerializeField] private DamageVisualizer damageVisualizer = null;
    public int Health { get; set; } = 100;
    public int MaxHealth { get { return data.health; } }

    private float cooldown = 0f;

    private void Start()
    {
        Health = data.health;
        LevelManager.Instance.AddEnemy(this);
        SpawnEnemy();
    }

    public void TakeDamage(int _damage, GameObject _sender)
    {
        Health -= _damage;

        DamageVisualizer visualizer = Instantiate(damageVisualizer, transform.position, Quaternion.identity);
        visualizer.SetText(_damage.ToString());
        if (Health <= 0)
        {
            LevelManager.Instance.RemoveEnemy(this);
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
            GameObject enemy = ObjectPool.Instance.Get(randomWave.enemy.prefab);
            enemy.transform.position = transform.position + Random.insideUnitSphere * data.spawnRadius;
        }
        cooldown = randomWave.cooldown;
    }


}
