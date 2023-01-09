using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementController : MonoBehaviour
{
    public Action OnPathComplete;

    [SerializeField] private bool visualizePath = true;
    [SerializeField] protected Rigidbody2D rb = null;
    [SerializeField] protected float speed = 3f;
    [SerializeField] protected float moveThreshold = 0.1f;
    [SerializeField] protected float recalculatePathTime = 0.5f;
    [SerializeField] protected bool constantlyUpdatePath = false;
    [SerializeField] protected bool smoothenPath = true;

    public Vector3 TargetPosition { get; set; }

    private LevelManager levelManager = null;
    private List<Vector2Int> path = new List<Vector2Int>();
    private int pathIndex = 0;
    private Vector2 currentTarget = Vector2.zero;
    private float lastRecalculate = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        levelManager = LevelManager.Instance;
    }

    private void Update()
    {
        if (constantlyUpdatePath)
        {
            if (Time.time - lastRecalculate > recalculatePathTime)
            {
                UpdatePath();
                lastRecalculate = Time.time;
            }
        }
    }

    public void Move()
    {
        if (path.Count == 0)
            return;

        Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed * LevelManager.Instance.TimeScale;

        if (Vector2.Distance(transform.position, currentTarget) < moveThreshold)
        {
            pathIndex--;

            if (pathIndex < 0)
            {
                path.Clear();
                OnPathComplete?.Invoke();
                return;
            }

            CalculateTarget();
        }
    }

    public void StopMoving()
    {
        path.Clear();
        rb.velocity = Vector2.zero;
    }

    public void UpdatePath()
    {
        GridElement currentGrid = GameManager.Instance.WorldGrid.GetGridElement(transform.position, true);
        GridElement targetGrid = GameManager.Instance.WorldGrid.GetGridElement(TargetPosition, false);

        if (!targetGrid)
        {
            Debug.LogError("Target grid is null");
            return;
        }

        GameManager.Instance.WorldGrid.FindPath(currentGrid.gridPosition, targetGrid.gridPosition, this);
    }

    public void TakePath(List<Vector2Int> newPath)
    {
        path = newPath;
        pathIndex = path.Count - 1;

        CalculateTarget();
    }

    public void CalculateTarget()
    {
        if (pathIndex >= 0)
        {
            Vector2 newTarget = GameManager.Instance.WorldGrid.GetGridElement(path[pathIndex]).transform.position;
            if (smoothenPath && pathIndex > 0)
            {
                Vector2 secondNewTarget = GameManager.Instance.WorldGrid.GetGridElement(path[pathIndex - 1]).transform.position;
                currentTarget = (newTarget + secondNewTarget) / 2;
            }
            else
                currentTarget = newTarget;
        }
    }

    private void OnDrawGizmos()
    {
        if (!visualizePath)
            return;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector2Int tile = new Vector2Int(path[i].x, path[i].y);
            Vector2 pos = GameManager.Instance.WorldGrid.GetGridElement(tile).transform.position;

            Gizmos.DrawSphere(pos, 0.1f);

            Vector2Int nextTile = new Vector2Int(path[i + 1].x, path[i + 1].y);
            Vector2 nextPos = GameManager.Instance.WorldGrid.GetGridElement(nextTile).transform.position;

            Debug.DrawLine(pos, nextPos, Color.red);
        }
    }
}
