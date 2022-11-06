using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] protected Rigidbody2D rb = null;
    [SerializeField] protected Animator animator = null;
    [SerializeField] private EnemyData data = null;
    [SerializeField] private Canvas healthCanvas = null;
    [SerializeField] private Slider healthBar = null;
    public EnemyData Data { get { return data; } }

    public int Health { get; private set; }
    public int Damage { get; private set; }

    public void Init(EnemyData _data)
    {

    }

    private void OnEnable()
    {
        Health = data.health;
        Damage = data.damage;
        healthCanvas.worldCamera = Camera.main;
        UpdateHealthBar();
    }

    public void TakeDamage(int _damage)
    {
        Health -= _damage;
        UpdateHealthBar();

        if (Health <= 0)
            gameObject.SetActive(false);
    }

    private void UpdateHealthBar()
    {
        healthBar.value = (float)Health / data.health;

        healthCanvas.enabled = healthBar.value < 1f;
    }


}
