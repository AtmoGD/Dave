using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionIdle : MinionState
{
    public override void Enter(Minion _minion)
    {
        base.Enter(_minion);

        minion.Animator.SetFloat("Velocity", 0f);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (minion.TargetTower)
        {
            minion.ChangeState(minion.MovingState);
        }
        else
        {
            FindTower();
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

    public void FindTower()
    {
        switch (minion.Data.preferredWork)
        {
            case WorkType.Farming:
                minion.TargetTower = minion.FindFarmTower();
                if (!minion.TargetTower)
                    minion.TargetTower = minion.FindUnBuildTower();

                break;

            case WorkType.Building:
                minion.TargetTower = minion.FindUnBuildTower();
                if (!minion.TargetTower)
                    minion.TargetTower = minion.FindFarmTower();

                break;
        }
    }
}
