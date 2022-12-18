using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private bool visualizePath = true;

    [SerializeField] protected Rigidbody2D rb = null;
    [SerializeField] protected Animator animator = null;
    [SerializeField] private EnemyData data = null;
    [SerializeField] private MovementController movementController = null;
    public EnemyData Data { get { return data; } }
    public int Health { get; private set; }
    public int MaxHealth { get { return data.health; } }
    public int Damage { get; private set; }

    private LevelManager levelManager = null;
    private Nekromancer nekromancer;

    private List<Vector2Int> path = new List<Vector2Int>();
    private int pathIndex = 0;

    Vector2 currentTarget = Vector2.zero;
    float lastRecalculate = 0f;

    List<GridElement> gridElements = new List<GridElement>();

    public void Init(EnemyData _data)
    {

    }

    private void OnEnable()
    {
        Health = data.health;
        Damage = data.damage;
        levelManager = (LevelManager)GameManager.Instance;
        levelManager.AddEnemy(this);
        nekromancer = levelManager.PlayerController.Nekromancer;
        currentTarget = transform.position;
    }

    private void OnDisable()
    {
        levelManager.RemoveEnemy(this);
    }

    private void Update()
    {
        UpdatePath();
        MoveToPath();

        if (visualizePath)
            VisualizePath();
    }

    public void MoveToNekromancer()
    {
        Vector2 direction = (nekromancer.transform.position - transform.position).normalized;
        rb.velocity = direction * data.speed;
    }

    public void MoveToPath()
    {
        if (path.Count == 0)
            return;

        Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;
        rb.velocity = direction * data.speed;

        if (Vector2.Distance(transform.position, currentTarget) < data.moveThreshold)
        {
            pathIndex--;

            CalculateTarget();
        }
    }

    public void VisualizePath()
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector2Int tile = new Vector2Int(path[i].x, path[i].y);
            Vector2 pos = levelManager.WorldGrid.GetGridElement(tile).transform.position;

            Vector2Int nextTile = new Vector2Int(path[i + 1].x, path[i + 1].y);
            Vector2 nextPos = levelManager.WorldGrid.GetGridElement(nextTile).transform.position;

            Debug.DrawLine(pos, nextPos, Color.red);
        }
    }

    public void UpdatePath()
    {
        if (Time.time - lastRecalculate < data.recalculatePathTime)
            return;

        GridElement enemyGrid = levelManager.WorldGrid.GetGridElement(transform.position, true);
        GridElement nekromancerGrid = levelManager.WorldGrid.GetGridElement(nekromancer.transform.position, true);
        path = levelManager.WorldGrid.FindPath(enemyGrid.gridPosition, nekromancerGrid.gridPosition);
        pathIndex = path.Count - 1;
        lastRecalculate = Time.time;
        CalculateTarget();
    }

    public void CalculateTarget()
    {
        if (pathIndex > 0)
        {
            Vector2 newTarget = levelManager.WorldGrid.GetGridElement(path[pathIndex]).transform.position;
            Vector2 secondNewTarget = levelManager.WorldGrid.GetGridElement(path[pathIndex - 1]).transform.position;
            currentTarget = (newTarget + secondNewTarget) / 2;
        }
        else
        {
            currentTarget = nekromancer.transform.position;
        }
    }

    public void TakeDamage(int _damage)
    {
        Health -= _damage;

        if (Health <= 0)
            gameObject.SetActive(false);
    }
}
