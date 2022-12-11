using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionMoving : MinionState
{
    public override void Enter(Minion _minion)
    {
        base.Enter(_minion);

        Transform _target = minion.TargetTower.GetFreeNeighbour();

        if (_target)
        {
            minion.MoveController.TargetPosition = _target.position;
            minion.MoveController.UpdatePath();
            minion.MoveController.OnPathComplete += ReachedTarget;
        }
        else
        {
            minion.ChangeState(minion.IdleState);
        }
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        minion.MoveController.Move();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();

        minion.MoveController.StopMoving();
    }

    public void ReachedTarget()
    {
        minion.MoveController.OnPathComplete -= ReachedTarget;

        if (!minion.TargetTower.IsBuilt)
        {
            minion.ChangeState(minion.BuildingState);
            return;
        }

        if (minion.TargetTower is FarmTower)
        {
            minion.ChangeState(minion.FarmingState);
            return;
        }

        if (minion.TargetTower is Crystal)
        {
            minion.ChangeState(minion.DeliveringState);
            return;
        }
    }
}
