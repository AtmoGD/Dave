using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : GameManager
{
    public Action<CycleState> OnCycleChanged;
    [SerializeField] private LevelData levelData = null;
    public List<IDamagable> activeEnemies = new List<IDamagable>();
    private int currentCycle = 0;

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

    public void AddEnemy(IDamagable enemy)
    {
        this.activeEnemies.Add(enemy);
    }

    public void RemoveEnemy(IDamagable enemy)
    {
        this.activeEnemies.Remove(enemy);
    }
}
