using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionMoving : MinionState
{
    public override void Enter(Minion _minion)
    {
        base.Enter(_minion);

        Transform _target = minion.TargetTower.GetFreeNeighbour();

        minion.SoundEmitter.Play();

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

        minion.Animator.SetFloat("Velocity", 1f);

        Flip();
    }

    public void Flip()
    {
        float targetX = minion.MoveController.TargetPosition.x > minion.transform.position.x ? 1f : -1f;
        float currentX = minion.transform.localScale.x;
        float newX = Mathf.Lerp(currentX, targetX, minion.Data.flipSpeed * Time.deltaTime);
        minion.transform.localScale = new Vector3(newX, 1f, 1f);
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();

        minion.MoveController.StopMoving();

        minion.SoundEmitter.Stop();
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
