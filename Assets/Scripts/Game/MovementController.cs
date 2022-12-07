using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private bool visualizePath = true;
    [SerializeField] protected Rigidbody2D rb = null;
    [SerializeField] protected float speed = 3f;
    [SerializeField] protected float moveThreshold = 0.1f;
    [SerializeField] protected float recalculatePathTime = 0.5f;

    public Vector3 TargetPosition { get; set; }

    private LevelManager levelManager = null;
    private List<Vector2Int> path = new List<Vector2Int>();
    private int pathIndex = 0;
    private Vector2 currentTarget = Vector2.zero;
    private float lastRecalculate = 0f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        levelManager = (LevelManager)GameManager.Instance;
    }

    private void Update()
    {
        UpdatePath();
        Move();

        if (visualizePath)
            VisualizePath();
    }

    public void Move()
    {

    if (path.Count == 0)
            return;

        Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;

        if (Vector2.Distance(transform.position, currentTarget) < moveThreshold)
        {
            pathIndex--;

            CalculateTarget();
        }
    }

    public void UpdatePath()
    {
        if (Time.time - lastRecalculate < recalculatePathTime)
            return;

        GridElement currentGrid = levelManager.WorldGrid.GetGridElement(transform.position, true);
        GridElement targetGrid = levelManager.WorldGrid.GetGridElement(TargetPosition, true);
        path = levelManager.WorldGrid.FindPath(currentGrid.gridPosition, targetGrid.gridPosition);
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
}
