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

    private float timeLeft = 0f;

    public virtual void Enter(LevelManager _gameManager)
    {
        if (this.cycle == Cycle.Day)
            this.duration *= GameManager.Instance.PlayerController.Nekromancer.DayTimeMultiplier;

        this.timeLeft = this.duration;
    }

    public virtual void FrameUpdate(float _deltaTime)
    {
        this.timeLeft -= _deltaTime * LevelManager.Instance.TimeScale;

        if (this.cycle == Cycle.Night)
        {
            if (LevelManager.Instance.activeEnemies.Count <= 0 && this.timeLeft <= 0)
            {
                LevelManager.Instance.NextCycle();
            }
        }
        else
        {
            if (this.timeLeft <= 0f)
            {
                LevelManager.Instance.NextCycle();
            }
        }
    }

    public virtual void Exit()
    {
        if (choosePerk)
            GameManager.Instance.PlayerController.PerkPoints++;
    }
}
