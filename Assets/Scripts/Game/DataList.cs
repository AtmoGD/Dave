using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data List", menuName = "Data/DataList")]
public class DataList : ScriptableObject
{
    [field: SerializeField] public List<Placeable> Placeables { get; private set; } = new List<Placeable>();
    [field: SerializeField] public List<MinionData> Minions { get; private set; } = new List<MinionData>();
    [field: SerializeField] public List<SkillData> Skills { get; private set; } = new List<SkillData>();
    [field: SerializeField] public List<Perk> Perks { get; private set; } = new List<Perk>();

    public Placeable GetPlaceable(string _id)
    {
        return Placeables.Find(x => x.id == _id);
    }

    public MinionData GetMinion(string _id)
    {
        return Minions.Find(x => x.id == _id);
    }

    public SkillData GetSkill(string _id)
    {
        return Skills.Find(x => x.id == _id);
    }

    public Perk GetPerk(string _id)
    {
        return Perks.Find(x => x.id == _id);
    }
}
