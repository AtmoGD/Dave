using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; protected set; }
    public Action<CycleState> OnCycleChanged;
    public Action AddedTower;

    [field: SerializeField] public GameManager GM { get; private set; } = null;

    [Header("Level Manager")]
    private LevelData levelData = null;
    [SerializeField] private float timeScale = 1f;
    public float TimeScale { get { return timeScale; } }
    public Crystal Crystal { get; private set; } = null;
    public List<IDamagable> activeEnemies = new List<IDamagable>();
    public List<PlaceableObject> placedObjects = new List<PlaceableObject>();
    public List<Tower> Tower { get { return placedObjects.FindAll(x => x is Tower).ConvertAll(x => x as Tower); } }
    public List<FarmTower> FarmTower { get { return placedObjects.FindAll(x => x is FarmTower).ConvertAll(x => x as FarmTower); } }
    public List<AttackTower> AttackTower { get { return placedObjects.FindAll(x => x is AttackTower).ConvertAll(x => x as AttackTower); } }

    private int enemyCount = 0;
    private int currentCycle = 0;

    public float PercentOfActiveEnemies { get { return (float)activeEnemies.Count / (float)enemyCount; } }

    public CycleState CurrentCycleState
    {
        get
        {
            if (currentCycle < levelData.cycleStates.Count)
                return levelData.cycleStates[currentCycle];
            else
                return null;
        }
    }

    public bool IsDay { get { return CurrentCycleState.Cycle == Cycle.Day; } }
    public bool IsNight { get { return CurrentCycleState.Cycle == Cycle.Night; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void Start()
    {
        levelData = GM.WorldGrid.LevelData;
        GM.WorldGrid.LoadLevel();


        this.currentCycle = 0;
        this.CurrentCycleState?.Enter(this);

        // base.Start();

        this.OnCycleChanged?.Invoke(this.CurrentCycleState);
    }

    public void Update()
    {
        // base.Update();

        CurrentCycleState?.FrameUpdate(Time.deltaTime);
    }

    public void NextCycle()
    {
        this.CurrentCycleState?.Exit();

        this.enemyCount = 0;

        this.currentCycle++;

        if (this.currentCycle >= this.levelData.cycleStates.Count)
        {
            //TO-DO : End of level
            print("End of level");
            return;
        }

        this.CurrentCycleState?.Enter(this);

        this.OnCycleChanged?.Invoke(this.CurrentCycleState);
    }

    public void SetCrystal(Crystal crystal)
    {
        if (this.Crystal != null)
        {
            Debug.LogError("LevelManager: Crystal already exists");
        }

        this.Crystal = crystal;
    }

    public void RemoveCrystal(Crystal crystal)
    {
        this.Crystal = null;
    }

    public void AddEnemy(IDamagable enemy)
    {
        this.activeEnemies.Add(enemy);
        this.enemyCount++;
    }

    public void RemoveEnemy(IDamagable enemy)
    {
        this.activeEnemies.Remove(enemy);
    }

    public void AddTower(PlaceableObject tower)
    {
        this.placedObjects.Add(tower);
        this.AddedTower?.Invoke();
    }

    public void RemoveTower(PlaceableObject tower)
    {
        this.placedObjects.Remove(tower);
    }
}
