using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamagable
{
    [field: SerializeField] public EnemyData Data { get; private set; } = null;
    [field: SerializeField] public MovementController MoveController { get; private set; } = null;
    [field: SerializeField] public DamageVisualizer DamageVisualizer { get; private set; } = null;
    [field: SerializeField] public Animator Animator { get; private set; } = null;
    [field: SerializeField] public List<CollectedRessource> DropRessources { get; private set; } = new List<CollectedRessource>();

    public int Health { get; private set; }
    public int MaxHealth { get { return Data.health; } }
    public bool IsAlive { get { return Health > 0; } }
    public int Damage { get; private set; }

    public EnemyState CurrentState { get; private set; } = null;
    public EnemyIdle IdleState { get; private set; } = new EnemyIdle();
    public EnemyMoving MovingState { get; private set; } = new EnemyMoving();
    public EnemyAttacking AttackingState { get; private set; } = new EnemyAttacking();
    public EnemyGetHit GetHitState { get; private set; } = new EnemyGetHit();

    // public LevelManager LevelManager { get; private set; } = null;
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
        // LevelManager = LevelManager.Instance;
        LevelManager.Instance.AddEnemy(this);

        ChangeState(IdleState);

        active = true;
    }

    private void OnDisable()
    {
        LevelManager.Instance.RemoveEnemy(this);

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

    public void TakeDamage(int _damage, GameObject _sender)
    {
        Health -= _damage;

        DamageVisualizer visualizer = Instantiate(DamageVisualizer, transform.position, Quaternion.identity);
        visualizer.SetText(_damage.ToString());

        ChangeState(GetHitState);

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
                }

                LevelManager.Instance.GatherRessource(DropRessource);
            }
        }

        gameObject.SetActive(false);
    }

    public Tower FindNearestTower<T>(List<T> _tower) where T : Tower
    {
        Tower nearestTower = null;

        float nearestDistance = Mathf.Infinity;

        print(_tower.Count);
        if (_tower.Count == 0) return null;

        for (int i = 0; i < _tower.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, _tower[i].transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTower = _tower[i];
            }
        }

        // foreach (Tower tower in _tower)
        // {
        //     float distance = Vector3.Distance(transform.position, tower.transform.position);

        //     if (distance < nearestDistance)
        //     {
        //         nearestDistance = distance;
        //         nearestTower = tower;
        //     }
        // }

        return nearestTower;
    }

    public void Attack()
    {
        ApplyDamage();
    }

    public void ApplyDamage()
    {
        if (!active || !IsAlive) return;

        Nekromancer nekromancer = Target.GetComponent<Nekromancer>();
        if (nekromancer && (nekromancer.transform.position - transform.position).magnitude < Data.attackRange)
        {
            nekromancer.TakeDamage(Damage, this.gameObject);
            return;
        }

        Tower tower = Target.GetComponent<Tower>();
        if (tower)
        {
            tower.TakeDamage(Damage, this.gameObject);
            return;
        }
    }
}
