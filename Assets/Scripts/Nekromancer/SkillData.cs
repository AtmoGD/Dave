using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
    public new string name = "Skill";
    public string description = "Skill description";
    public int manaCosts = 0;
    public List<Cooldown> cooldowns = new List<Cooldown>();

    public virtual bool CanBeUsed(Nekromancer _nekromancer)
    {
        return !_nekromancer.HasCooldown(name);
    }

    public virtual Skill GetSkillInstance()
    {
        return new Skill();
    }
}
