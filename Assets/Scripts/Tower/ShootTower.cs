using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTower : AttackTower {

    [SerializeField] private float damage = 1f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject projectilePrefab = null;

    private List<Enemy> enemiesInRange = new List<Enemy>();
    private float fireTimer = 0f;

    private void Update() {
        fireTimer -= Time.deltaTime;

        if (enemiesInRange.Count > 0 && fireTimer <= 0f) 
            Shoot();
    }

    private void Shoot() {
        Transform target = enemiesInRange[Random.Range(0, enemiesInRange.Count)].transform;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        ShadowBallController shadowBallController = projectile.GetComponent<ShadowBallController>();
        shadowBallController.UpdateBaseDamage(damage);

        projectile.transform.right = target.position - transform.position;
        fireTimer = 1f / fireRate;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy) {
            enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy) {
            enemiesInRange.Remove(enemy);
        }
    }
}