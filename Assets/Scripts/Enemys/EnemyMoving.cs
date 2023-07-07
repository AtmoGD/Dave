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

        enemy.Animator.SetFloat("Velocity", 1f);

        Flip();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();

        float targetX = enemy.MoveController.TargetPosition.x > enemy.transform.position.x ? 1f : -1f;
        enemy.transform.localScale = new Vector3(targetX, 1f, 1f);
    }

    public void ReachedTarget()
    {
        enemy.MoveController.OnPathComplete -= ReachedTarget;

        enemy.MoveController.StopMoving();

        enemy.ChangeState(enemy.AttackingState);
    }

    public void Flip()
    {
        // float targetX = enemy.MoveController.TargetPosition.x > enemy.transform.position.x ? 1f : -1f;
        // float currentX = enemy.transform.localScale.x;
        // float newX = Mathf.Lerp(currentX, targetX, enemy.Data.flipSpeed * Time.deltaTime);
        // enemy.transform.localScale = new Vector3(newX, 1f, 1f);

        enemy.transform.localScale = new Vector3(enemy.MoveController.TargetPosition.x > enemy.transform.position.x ? 1f : -1f, 1f, 1f);
    }
}
