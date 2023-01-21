using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : EnemyState
{
    float timeSinceLastAttack = 0;

    public override void Enter(Enemy _enemy)
    {
        base.Enter(_enemy);

        timeSinceLastAttack = enemy.Data.attackSpeed / 2;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (enemy.Target == null)
        {
            enemy.ChangeState(enemy.IdleState);
            return;
        }

        timeSinceLastAttack += Time.deltaTime;

        if ((enemy.Target.transform.position - enemy.transform.position).magnitude > enemy.Data.attackRange)
        {
            enemy.ChangeState(enemy.MovingState);
            return;
        }


        if (timeSinceLastAttack > enemy.Data.attackSpeed)
        {
            enemy.Attack();
            timeSinceLastAttack = 0;
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
