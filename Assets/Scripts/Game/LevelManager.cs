using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : GameManager
{
    public Action<CycleState> OnCycleChanged;
    public Action AddedTower;

    [Header("Level Manager")]
    private LevelData levelData = null;
    public Crystal crystal = null;
    public List<IDamagable> activeEnemies = new List<IDamagable>();
    public List<PlaceableObject> towers = new List<PlaceableObject>();
    public List<FarmTower> FarmTower { get { return towers.FindAll(x => x is FarmTower).ConvertAll(x => x as FarmTower); } }

    // public List<FarmTower> FarmTower {
    //     get {
    //         List<FarmTower> farmTowers = new List<FarmTower>();
    //         foreach (PlaceableObject tower in towers) {
    //             if (tower is FarmTower) {
    //                 farmTowers.Add(tower as FarmTower);
    //             }
    //         }
    //         return  farmTowers;
    //     }
    // }
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

    public new void Start()
    {
        levelData = WorldGrid.LevelData;
        WorldGrid.LoadLevel();
        this.currentCycle = 0;
        this.CurrentCycleState?.Enter(this);

        base.Start();

        this.OnCycleChanged?.Invoke(this.CurrentCycleState);
    }

    public new void Update()
    {
        base.Update();

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

    public void AddCrystal(Crystal crystal)
    {
        if(this.crystal != null)
        {
            Debug.LogError("LevelManager: Crystal already exists");
        }
        
        this.crystal = crystal;
    }

    public void RemoveCrystal(Crystal crystal)
    {
        this.crystal = null;
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
        this.towers.Add(tower);
        this.AddedTower?.Invoke();
    }

    public void RemoveTower(PlaceableObject tower)
    {
        this.towers.Remove(tower);
    }
}
