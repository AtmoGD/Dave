using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyState
{
    public override void Enter(Enemy _enemy)
    {
        base.Enter(_enemy);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        FindTarget();

        if (enemy.Target != null)
            enemy.ChangeState(enemy.MovingState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void FindTarget()
    {
        switch (enemy.Data.prefferedAttackTarget)
        {
            case AttackTarget.Player:
                enemy.Target = enemy.LevelManager.GM.PlayerController.Nekromancer.transform;
                break;
            case AttackTarget.Tower:
                List<Tower> _towers = enemy.LevelManager.Tower;
                enemy.Target = enemy.FindNearestTower(_towers).transform;
                break;
            case AttackTarget.FarmTower:
                List<FarmTower> _farmTowers = enemy.LevelManager.FarmTower;
                enemy.Target = enemy.FindNearestTower(_farmTowers).transform;
                break;
            case AttackTarget.AttackTower:
                List<AttackTower> _attackTowers = enemy.LevelManager.AttackTower;
                enemy.Target = enemy.FindNearestTower(_attackTowers).transform;
                break;
            default:
                break;
        }
    }
}
