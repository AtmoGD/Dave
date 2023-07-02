using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMerge : Skill
{
    private ShadowMergeData shadowMergeData = null;
    private Vector2 lastInput = Vector2.zero;
    private Vector3 direction = Vector3.zero;
    private float distanceLeft = 0f;

    public override void Enter(Nekromancer _nekromancer, SkillData _skillData)
    {
        base.Enter(_nekromancer, _skillData);

        shadowMergeData = (ShadowMergeData)_skillData;

        lastInput = nekromancer.CurrentInput.MoveDir;
        direction = lastInput.normalized;
        // nekromancer.Cols.ForEach(x => x.isTrigger = true);#
        nekromancer.col.enabled = false;
        nekromancer.col.isTrigger = true;

        CheckTargetPosition();

        nekromancer.IsInputBlocked = true;
        nekromancer.CurrentInput.MoveDir = Vector2.zero;

        nekromancer.skinAnimator.gameObject.SetActive(false);
        nekromancer.Model.gameObject.SetActive(false);

        nekromancer.rb.velocity = Vector2.zero;

        Vector2 origin = nekromancer.transform.position + (Vector3)shadowMergeData.mergePrefabOffset;
        Vector2 target = origin + ((Vector2)direction * distanceLeft);

        GameObject shadowMergeOrigin = GameObject.Instantiate(shadowMergeData.shadowMergePrefab, origin, Quaternion.identity);
        GameObject shadowMergeTarget = GameObject.Instantiate(shadowMergeData.shadowMergePrefab, target, Quaternion.identity);

        nekromancer.teleportEmitter.Play();
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
        nekromancer.CurrentInput.MoveDir = lastInput;

        nekromancer.col.isTrigger = false;
        nekromancer.col.enabled = true;
        // nekromancer.Cols.ForEach(x => { x.isTrigger = false;  x.enabled = true; });

        nekromancer.IsInputBlocked = false;

        nekromancer.skinAnimator.gameObject.SetActive(true);
        nekromancer.Model.gameObject.SetActive(true);

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
