using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CycleState
{
    [SerializeField] private Cycle cycle = Cycle.Day;
    [SerializeField] private float duration = 60f;

    private string dayActionMap = "Building";
    private string nightActionMap = "Combat";

    public Cycle Cycle => cycle;
    public float Duration => duration;

    private LevelManager levelManager = null;
    private float timeLeft = 0f;

    public virtual void Enter(LevelManager _gameManager)
    {
        this.levelManager = _gameManager;
        this.timeLeft = this.duration;

        switch (this.cycle)
        {
            case Cycle.Day:
                this.levelManager.InputController.ChangeActionMap(this.dayActionMap);
                break;
            case Cycle.Night:
                this.levelManager.InputController.ChangeActionMap(this.nightActionMap);
                break;
        }
    }

    public virtual void FrameUpdate(float _deltaTime)
    {
        this.timeLeft -= _deltaTime;

        if (this.cycle == Cycle.Night)
        {
            if (levelManager.activeEnemies.Count <= 0)
            {
                this.levelManager.NextCycle();
            }
        }
        else
        {
            if (this.timeLeft <= 0f)
            {
                this.levelManager.NextCycle();
            }
        }


    }

    public virtual void Exit()
    {
        this.levelManager = null;
    }
}
