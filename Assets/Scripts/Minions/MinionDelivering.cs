using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDelivering : MinionState
{
    float timer = 0f;

    public override void Enter(Minion _minion)
    {
        base.Enter(_minion);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        timer += Time.deltaTime;

        if (timer >= minion.Data.deliverSpeed)
        {
            // TO DO: Deliver the goods
            LevelManager.Instance.GatherRessource(minion.Ressources);
            minion.RemoveRessources();

            minion.CurrentFarmAmount = 0;
            minion.TargetTower = minion.TargetFarmTower;
            minion.ChangeState(minion.MovingState);
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
