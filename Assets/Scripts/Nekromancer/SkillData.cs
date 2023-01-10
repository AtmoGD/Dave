using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SkillData : ScriptableObject
{
    public string id = "";
    public new string name = "Skill";
    public string description = "Skill description";
    public int manaCosts = 0;
    public List<Cooldown> cooldowns = new List<Cooldown>();

    public virtual bool CanBeUsed(Nekromancer _nekromancer)
    {
        return !_nekromancer.HasCooldown(cooldowns[0].name) && _nekromancer.CurrentMana >= manaCosts;
    }

    public virtual Skill GetSkillInstance()
    {
        return new Skill();
    }
}
