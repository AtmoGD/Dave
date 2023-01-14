using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : EnemyState
{
    Transform target;
    bool constantlyUpdatePath = false;

    public override void Enter(Enemy _enemy)
    {
        base.Enter(_enemy);

        Tower tower = enemy.Target.GetComponent<Tower>();
        if (tower)
        {
            target = tower.GetFreeNeighbour();
            constantlyUpdatePath = false;
        }
        else
        {
            target = enemy.Target;
            constantlyUpdatePath = true;
        }

        if (target)
        {
            enemy.MoveController.TargetPosition = target.position;
            enemy.MoveController.UpdatePath();
            enemy.MoveController.OnPathComplete += ReachedTarget;
        }
        else
        {
            enemy.ChangeState(enemy.IdleState);
        }
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        enemy.MoveController.TargetPosition = target.position;
        enemy.MoveController.Move();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void ReachedTarget()
    {
        enemy.MoveController.OnPathComplete -= ReachedTarget;

        enemy.MoveController.StopMoving();

        enemy.ChangeState(enemy.AttackingState);
    }
}
