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

        nekromancer.CurrentSkill = this;
        foreach (Cooldown cooldown in skillData.cooldowns)
            nekromancer.AddCooldown(cooldown.GetCopy());
        // nekromancer.AddCooldown(skillData.cooldown.GetCopy(skillData.name));
        timer = 0f;
    }
    public virtual void FrameUpdate(float _deltaTime)
    {
        timer += _deltaTime;
        // Do stuff
    }

    public virtual void PhysicsUpdate(float _deltaTime)
    {
        // Do stuff
    }

    public virtual void Exit()
    {
        nekromancer.CurrentSkill = null;
        nekromancer = null;
    }
}
