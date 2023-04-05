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

        if (!enemy.Target) enemy.ChangeState(enemy.IdleState);

        target = enemy.Target;

        if (target)
        {
            constantlyUpdatePath = true;
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

        if (!target) enemy.ChangeState(enemy.IdleState);

        enemy.MoveController.TargetPosition = target.position;
        enemy.MoveController.Move();

        if ((target.transform.position - enemy.transform.position).magnitude < enemy.Data.attackRange)
        {
            ReachedTarget();
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

    public void ReachedTarget()
    {
        enemy.MoveController.OnPathComplete -= ReachedTarget;

        enemy.MoveController.StopMoving();

        enemy.ChangeState(enemy.AttackingState);
    }
}
