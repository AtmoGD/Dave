using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionFarming : MinionState
{
    private float farmAmount = 0;

    public override void Enter(Minion _minion)
    {
        base.Enter(_minion);

        if (!minion.TargetFarmTower && minion.TargetTower is FarmTower)
            minion.TargetFarmTower = minion.TargetTower as FarmTower;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        farmAmount += minion.Data.farmSpeed * Time.deltaTime;

        if (farmAmount >= 1f)
        {
            CollectedRessource ressource = new CollectedRessource();
            ressource.ressource = minion.TargetFarmTower.GetRessource();
            ressource.amount = 1;

            minion.AddRessource(ressource);
            farmAmount = 0;
        }

        if (minion.CurrentCarryAmount >= minion.Data.carryCapacity)
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

        minion.Animator.SetFloat("Velocity", 0f);
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
