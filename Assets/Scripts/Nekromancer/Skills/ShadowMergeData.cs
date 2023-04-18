using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShadowMerge", menuName = "Nekromancer/Skills/ShadowMerge")]
public class ShadowMergeData : SkillData
{
    public float speed = 10f;
    public float distance = 5f;
    public float delay = 0.3f;
    public int stackSize = 3;
    public LayerMask collisionMask;
    public float collisionRadius = 0.5f;
    public GameObject shadowMergePrefab = null;
    public Vector2 mergePrefabOffset = Vector2.zero;

    public override bool CanBeUsed(Nekromancer _nekromancer)
    {
        bool canBeUsed = base.CanBeUsed(_nekromancer);
        bool hasEnoughStacks = _nekromancer.CountCooldowns(cooldowns[1].name) < stackSize;
        bool hasIputDir = _nekromancer.CurrentInput.MoveDir != Vector2.zero;

        return canBeUsed && hasEnoughStacks && hasIputDir;
    }

    public override Skill GetSkillInstance()
    {
        return new ShadowMerge();
    }
}
