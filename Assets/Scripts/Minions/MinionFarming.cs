using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionFarming : MinionState
{
    public override void Enter(Minion _minion)
    {
        base.Enter(_minion);

        if (!minion.TargetFarmTower && minion.TargetTower is FarmTower)
            minion.TargetFarmTower = minion.TargetTower as FarmTower;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        minion.CurrentFarmAmount += minion.Data.farmSpeed * Time.deltaTime;
        minion.CurrentFarmAmount = Mathf.Clamp(minion.CurrentFarmAmount, 0, minion.Data.carryCapacity);

        if (minion.CurrentFarmAmount >= minion.Data.carryCapacity)
        {
            if (minion.LevelManager.Crystal)
            {
                minion.TargetTower = minion.LevelManager.Crystal;
                minion.ChangeState(minion.MovingState);
            }
            else
            {
                minion.ChangeState(minion.IdleState);
                Debug.Log("No Crystal");
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
