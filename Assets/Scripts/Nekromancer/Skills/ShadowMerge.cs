using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMerge : Skill
{
    private ShadowMergeData shadowMergeData = null;
    private Vector3 direction = Vector3.zero;
    private float distanceLeft = 0f;

    public override void Enter(Nekromancer _nekromancer, SkillData _skillData)
    {
        base.Enter(_nekromancer, _skillData);

        shadowMergeData = (ShadowMergeData)_skillData;

        direction = nekromancer.CurrentInput.MoveDir.normalized;
        nekromancer.col.isTrigger = true;

        CheckTargetPosition();
    }

    public override void FrameUpdate(float _deltaTime)
    {
        base.FrameUpdate(_deltaTime);

        if (distanceLeft <= 0f)
            nekromancer.ChangeSkill();
    }

    public override void PhysicsUpdate(float _deltaTime)
    {
        base.PhysicsUpdate(_deltaTime);

        if (timer < shadowMergeData.delay)
            return;

        Vector3 movement = direction * shadowMergeData.speed * _deltaTime;
        movement = Vector3.ClampMagnitude(movement, distanceLeft);
        nekromancer.rb.MovePosition(nekromancer.transform.position + movement);
        distanceLeft -= movement.magnitude;
    }

    public override void Exit()
    {
        nekromancer.col.isTrigger = false;

        base.Exit();
    }

    private void CheckTargetPosition()
    {
        Vector2 origin = nekromancer.transform.position;
        Vector2 target = origin + ((Vector2)direction * shadowMergeData.distance);

        if (Physics2D.OverlapCircle(target, shadowMergeData.collisionRadius, shadowMergeData.collisionMask))
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, shadowMergeData.distance, shadowMergeData.collisionMask);
            distanceLeft = hit.distance;
        }
        else
        {
            distanceLeft = shadowMergeData.distance;
        }
    }
}
