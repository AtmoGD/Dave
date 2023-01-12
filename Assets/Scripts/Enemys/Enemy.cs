using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamagable
{
    [field: SerializeField] public EnemyData Data { get; private set; } = null;
    [field: SerializeField] public MovementController MoveController { get; private set; } = null;
    // [field: SerializeField] public GameObject DiePrefab { get; private set; } = null;
    [field: SerializeField] public List<CollectedRessource> DropRessources { get; private set; } = new List<CollectedRessource>();

    public int Health { get; private set; }
    public int MaxHealth { get { return Data.health; } }
    public int Damage { get; private set; }

    public EnemyState CurrentState { get; private set; } = null;
    public EnemyIdle IdleState { get; private set; } = new EnemyIdle();
    public EnemyMoving MovingState { get; private set; } = new EnemyMoving();
    public EnemyAttacking AttackingState { get; private set; } = new EnemyAttacking();

    public LevelManager LevelManager { get; private set; } = null;
    public Transform Target { get; set; } = null;

    bool active = false;

    private void Awake()
    {
        active = false;
    }

    private void OnEnable()
    {
        Health = Data.health;
        Damage = Data.damage;
        LevelManager = LevelManager.Instance;
        LevelManager.AddEnemy(this);

        ChangeState(IdleState);

        active = true;
    }

    private void OnDisable()
    {
        LevelManager.RemoveEnemy(this);

        active = false;
    }

    private void Update()
    {
        if (!active) return;

        CurrentState?.FrameUpdate();
    }

    private void FixedUpdate()
    {
        if (!active) return;

        CurrentState?.PhysicsUpdate();
    }

    public void ChangeState(EnemyState _newState)
    {
        CurrentState?.Exit();

        CurrentState = _newState;

        CurrentState?.Enter(this);
    }

    public void TakeDamage(int _damage)
    {
        Health -= _damage;

        if (Health <= 0) Die();
    }

    public void Die()
    {
        if (DropRessources.Count > 0)
        {
            foreach (CollectedRessource DropRessource in DropRessources)
            {
                for (int i = 0; i < DropRessource.amount; i++)
                {
                    Vector2 randomPos = (Vector2)transform.position + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                    Instantiate(DropRessource.ressource.prefab, randomPos, Quaternion.identity);
                    LevelManager.Instance.GatherRessource(DropRessource);
                }
            }
        }

        gameObject.SetActive(false);
    }

    public Tower FindNearestTower<T>(List<T> _tower) where T : Tower
    {
        Tower nearestTower = null;

        float nearestDistance = Mathf.Infinity;

        foreach (Tower tower in _tower)
        {
            float distance = Vector3.Distance(transform.position, tower.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTower = tower;
            }
        }

        return nearestTower;
    }

    public void Attack()
    {

    }

    public void ApplyDamage()
    {

    }
}
