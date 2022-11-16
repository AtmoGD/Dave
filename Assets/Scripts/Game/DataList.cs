using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data List", menuName = "Data/DataList")]
public class DataList : ScriptableObject
{
    [SerializeField] private List<Placeable> placeables = new List<Placeable>();
    [SerializeField] private List<MinionData> minions = new List<MinionData>();
    [SerializeField] private List<SkillData> skills = new List<SkillData>();

    public Placeable GetPlaceable(string _id)
    {
        return placeables.Find(x => x.id == _id);
    }

    public MinionData GetMinion(string _id)
    {
        return minions.Find(x => x.id == _id);
    }

    public SkillData GetSkill(string _id)
    {
        return skills.Find(x => x.id == _id);
    }
}
