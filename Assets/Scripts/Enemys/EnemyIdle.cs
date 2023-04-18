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

        enemy.Animator.SetFloat("Velocity", 0f);
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
        try
        {
            switch (enemy.Data.prefferedAttackTarget)
            {
                case AttackTarget.Player:
                    enemy.Target = GameManager.Instance.PlayerController.Nekromancer.transform;
                    break;
                case AttackTarget.Tower:
                    Debug.Log("TowerCount: " + LevelManager.Instance.Tower.Count + "");
                    List<Tower> _towers = LevelManager.Instance.Tower;
                    enemy.Target = enemy.FindNearestTower(_towers).transform;
                    break;
                case AttackTarget.FarmTower:
                    List<FarmTower> _farmTowers = LevelManager.Instance.FarmTower;
                    enemy.Target = enemy.FindNearestTower(_farmTowers).transform;
                    break;
                case AttackTarget.AttackTower:
                    List<AttackTower> _attackTowers = LevelManager.Instance.AttackTower;
                    enemy.Target = enemy.FindNearestTower(_attackTowers).transform;
                    break;
                default:
                    break;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }
    }
}
