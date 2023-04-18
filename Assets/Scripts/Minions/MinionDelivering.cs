using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDelivering : MinionState
{
    float timer = 0f;

    public override void Enter(Minion _minion)
    {
        base.Enter(_minion);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        timer += Time.deltaTime;

        if (timer >= minion.Data.deliverSpeed)
            DeliverResource();

        if (minion.Ressources.Count == 0)
        {
            minion.CurrentFarmAmount = 0;
            minion.TargetTower = minion.TargetFarmTower;
            minion.ChangeState(minion.MovingState);
        }

        minion.Animator.SetFloat("Velocity", 0f);
    }

    public void DeliverResource()
    {
        List<CollectedRessource> ressources = new List<CollectedRessource>();
        ressources.Add(minion.Ressources[0]);
        minion.Ressources.RemoveAt(0);
        minion.SpawnObject(ressources[0].ressource.prefab, minion.SpawnRessourcePoint.position);
        LevelManager.Instance.GatherRessource(ressources);
        timer = 0f;
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
