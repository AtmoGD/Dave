using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHit : EnemyState
{
    private float getHitTimer = 0f;

    public override void Enter(Enemy _enemy)
    {
        base.Enter(_enemy);

        getHitTimer = 0f;

        enemy.MoveController.StopMoving();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        getHitTimer += Time.deltaTime;

        if (getHitTimer >= enemy.Data.getHitTime)
        {
            enemy.ChangeState(enemy.MovingState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
