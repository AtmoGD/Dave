using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    protected Nekromancer nekromancer = null;
    protected SkillData skillData = null;
    protected float timer = 0f;
    public virtual void Enter(Nekromancer _nekromancer, SkillData _skillData)
    {
        nekromancer = _nekromancer;
        skillData = _skillData;

        foreach (Cooldown cooldown in skillData.cooldowns)
            nekromancer.AddCooldown(cooldown.GetCopy());

        timer = 0f;
    }
    public virtual void FrameUpdate(float _deltaTime)
    {
        timer += _deltaTime;
    }

    public virtual void PhysicsUpdate(float _deltaTime)
    {

    }

    public virtual void Exit()
    {
        nekromancer = null;
    }
}
