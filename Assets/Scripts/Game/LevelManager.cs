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
    // private LevelData levelData = null;
    [SerializeField] private LevelData levelData = null;
    public LevelData LevelData { get { return levelData; } }

    [field: SerializeField] public float TimeScale { get; private set; } = 1f;
    public Crystal Crystal { get; private set; } = null;
    public List<IDamagable> activeEnemies = new List<IDamagable>();
    public int EnemyCount { get { return activeEnemies.Count; } }
    [SerializeField] private int debugCount = 0;
    public List<PlaceableObject> placedObjects = new List<PlaceableObject>();
    public List<Tower> Tower { get { return placedObjects.FindAll(x => x is Tower).ConvertAll(x => x as Tower); } }
    public List<FarmTower> FarmTower { get { return placedObjects.FindAll(x => x is FarmTower).ConvertAll(x => x as FarmTower); } }
    public List<AttackTower> AttackTower { get { return placedObjects.FindAll(x => x is AttackTower).ConvertAll(x => x as AttackTower); } }
    [field: SerializeField] public List<CollectedRessource> GatheredRessources { get; private set; } = new List<CollectedRessource>();

    private int currentCycle = 0;

    public float PercentOfActiveEnemies { get { return (float)activeEnemies.Count / (float)EnemyCount; } }

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

    public bool GameEnded { get; private set; } = false;

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
        WorldGrid.Instance.LoadLevel();

        GatherRessource(LevelData.startRessources);

        this.currentCycle = 0;
        this.CurrentCycleState?.Enter(this);

        this.OnCycleChanged?.Invoke(this.CurrentCycleState);

        GameEnded = false;
    }

    public void Update()
    {
        if (GameEnded) return;

        debugCount = activeEnemies.Count;

        CurrentCycleState?.FrameUpdate(Time.deltaTime);
    }



    public void GatherRessource(CollectedRessource _ressource)
    {
        if (GameEnded) return;

        CollectedRessource newRessource = GatheredRessources.Find(x => x.ressource == _ressource.ressource);
        if (newRessource != null)
        {
            newRessource.amount += _ressource.amount;
        }
        else
        {
            newRessource = new CollectedRessource();
            newRessource.ressource = _ressource.ressource;
            newRessource.amount = _ressource.amount;
            GatheredRessources.Add(newRessource);
        }
    }

    public bool HasEnoughRessources(Ressource _ressource, int _amount)
    {
        if (GameEnded) return false;

        List<CollectedRessource> list = GatheredRessources.FindAll(x => x.ressource == _ressource);
        if (list.Count == 0) return false;

        int amount = 0;
        foreach (CollectedRessource ressource in list)
        {
            amount += ressource.amount;
        }

        return amount >= _amount;
    }

    public void RemoveRessources(Ressource _ressource, int _amount)
    {
        if (GameEnded) return;

        List<CollectedRessource> list = GatheredRessources.FindAll(x => x.ressource == _ressource);
        if (list.Count == 0) return;

        int amount = 0;
        foreach (CollectedRessource ressource in list)
        {
            amount += ressource.amount;
        }

        if (amount < _amount) return;

        foreach (CollectedRessource ressource in list)
        {
            if (ressource.amount > _amount)
            {
                ressource.amount -= _amount;
                _amount = 0;
            }
            else
            {
                _amount -= ressource.amount;
                ressource.amount = 0;
            }

            if (_amount == 0) break;
        }

        GatheredRessources.RemoveAll(x => x.amount <= 0);
    }

    public void GatherRessource(List<CollectedRessource> _ressources)
    {
        if (GameEnded) return;

        foreach (CollectedRessource ressource in _ressources)
        {
            GatherRessource(ressource);
        }
    }

    public void NextCycle()
    {
        if (GameEnded) return;

        this.CurrentCycleState?.Exit();

        this.currentCycle++;

        if (this.currentCycle >= this.levelData.cycleStates.Count)
        {
            CrystalFull = true;
        }

        this.CurrentCycleState?.Enter(this);

        this.OnCycleChanged?.Invoke(this.CurrentCycleState);
    }

    public void EndGame()
    {
        if (GameEnded) return;

        GameEnded = true;

        GameManager.Instance.PlayerController.AddRessources(GatheredRessources);
    }

    public void NekromancerDie()
    {
        if (GameEnded) return;

        GameEnded = true;
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
