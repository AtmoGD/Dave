using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTower : AttackTower
{

    [SerializeField] private float damage = 1f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject projectilePrefab = null;
    [field: SerializeField] public List<Transform> ShootPositions { get; private set; } = new List<Transform>();

    private List<Enemy> enemiesInRange = new List<Enemy>();
    private float fireTimer = 0f;

    private void Update()
    {
        fireTimer -= Time.deltaTime;

        if (enemiesInRange.Count > 0 && fireTimer <= 0f)
            Shoot();
    }

    private void Shoot()
    {
        Transform target = enemiesInRange[Random.Range(0, enemiesInRange.Count)].transform;
        GameObject projectile = ObjectPool.Instance.Get(projectilePrefab);
        projectile.GetComponent<ShadowBallController>().Sender = gameObject;

        Transform nearestShootPosition = ShootPositions[0];
        foreach (Transform shootPosition in ShootPositions)
        {
            if (Vector2.Distance(shootPosition.position, target.position) <
                Vector2.Distance(nearestShootPosition.position, target.position))
            {
                nearestShootPosition = shootPosition;
            }
        }

        projectile.transform.position = nearestShootPosition.position;
        ShadowBallController shadowBallController = projectile.GetComponent<ShadowBallController>();
        shadowBallController.UpdateBaseDamage(damage, gameObject);

        projectile.transform.right = target.position - nearestShootPosition.position;
        fireTimer = 1f / fireRate;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            enemiesInRange.Remove(enemy);
        }
    }
}