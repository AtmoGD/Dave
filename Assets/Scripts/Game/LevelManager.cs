using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; protected set; }
    public Action<CycleState> OnCycleChanged;
    public Action AddedTower;


    [Header("Level Manager")]
    private LevelData levelData = null;
    [field: SerializeField] public float TimeScale { get; private set; } = 1f;
    public Crystal Crystal { get; private set; } = null;
    public List<IDamagable> activeEnemies = new List<IDamagable>();
    public List<PlaceableObject> placedObjects = new List<PlaceableObject>();
    public List<Tower> Tower { get { return placedObjects.FindAll(x => x is Tower).ConvertAll(x => x as Tower); } }
    public List<FarmTower> FarmTower { get { return placedObjects.FindAll(x => x is FarmTower).ConvertAll(x => x as FarmTower); } }
    public List<AttackTower> AttackTower { get { return placedObjects.FindAll(x => x is AttackTower).ConvertAll(x => x as AttackTower); } }
    [field: SerializeField] public List<CollectedRessource> GatheredRessources { get; private set; } = new List<CollectedRessource>();


    private int enemyCount = 0;
    private int currentCycle = 0;

    public float PercentOfActiveEnemies { get { return (float)activeEnemies.Count / (float)enemyCount; } }

    [field: SerializeField] public bool CrystalFull { get; private set; } = false;

    public CycleState CurrentCycleState
    {
        get
        {
            if (currentCycle < levelData.cycleStates.Count)
                return levelData.cycleStates[currentCycle];
            else
                return levelData.cycleStatesLateGame[currentCycle % levelData.cycleStatesLateGame.Count];
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
        levelData = GameManager.Instance.WorldGrid.LevelData;
        GameManager.Instance.WorldGrid.LoadLevel();


        this.currentCycle = 0;
        this.CurrentCycleState?.Enter(this);

        this.OnCycleChanged?.Invoke(this.CurrentCycleState);
    }

    public void Update()
    {
        CurrentCycleState?.FrameUpdate(Time.deltaTime);
    }

    public void GatherRessource(CollectedRessource _ressource)
    {
        GatheredRessources.Add(_ressource);
    }

    public void GatherRessource(List<CollectedRessource> _ressources)
    {
        foreach (CollectedRessource ressource in _ressources)
        {
            GatherRessource(ressource);
        }
    }

    public void NextCycle()
    {
        this.CurrentCycleState?.Exit();

        this.enemyCount = 0;

        this.currentCycle++;

        if (this.currentCycle >= this.levelData.cycleStates.Count)
        {
            //TO-DO : End of level
            CrystalFull = true;
            print("End of level");
            // return;
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

    public void StopTime()
    {
        TimeScale = 0f;
    }

    public void StartTime()
    {
        TimeScale = 1f;
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
