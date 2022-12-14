using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CycleState
{
    [SerializeField] private Cycle cycle = Cycle.Day;
    [SerializeField] private float duration = 60f;
    [SerializeField] private bool choosePerk = false;

    public Cycle Cycle => cycle;
    public float Duration => duration;
    public float TimeLeft => timeLeft;
    public float PercentOfTimeLeft => timeLeft / duration;
    public float TimeInCycle => duration - timeLeft;

    private LevelManager levelManager = null;
    private float timeLeft = 0f;

    public virtual void Enter(LevelManager _gameManager)
    {
        this.levelManager = _gameManager;
        this.timeLeft = this.duration;
    }

    public virtual void FrameUpdate(float _deltaTime)
    {
        this.timeLeft -= _deltaTime * this.levelManager.TimeScale;

        if (this.cycle == Cycle.Night)
        {
            if (levelManager.activeEnemies.Count <= 0 && this.timeLeft <= 0)
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
        if (choosePerk)
            GameManager.Instance.PlayerController.PerkPoints++;

        this.levelManager = null;
    }
}
