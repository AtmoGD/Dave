using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    [SerializeField] public string name = "Dave";
    [SerializeField] public int level = 1;
    [SerializeField] public int experience = 0;
    [SerializeField] public bool firstStart = true;
    [SerializeField] public bool firstLevelStart = true;
    [SerializeField] public List<string> collectables = new List<string>();
    [SerializeField] public List<string> equippedItems = new List<string>();
    [SerializeField] public List<string> upgrades = new List<string>();
    [SerializeField] public List<string> unlockedObjects = new List<string>();
    [SerializeField] public List<string> unlockedSkills = new List<string>();
    [SerializeField] public List<string> equippedSkills = new List<string>();
    [SerializeField] public List<string> unlockedMinions = new List<string>();
    [SerializeField] public List<string> unlockedRecipes = new List<string>();
    [SerializeField] public List<CampObjectData> placedObjects = new List<CampObjectData>();

    public PlayerData()
    {
        name = "Dave";
        level = 1;
        experience = 0;
        firstStart = true;
        firstLevelStart = true;
        collectables = new List<string>();
        equippedItems = new List<string>();
        upgrades = new List<string>();
        unlockedObjects = new List<string>();
        unlockedSkills = new List<string>();
        unlockedSkills.Add("nekro002");
        equippedSkills = new List<string>();
        equippedSkills.Add("nekro002");
        equippedSkills.Add("nekro003");
        unlockedMinions = new List<string>();
        unlockedRecipes = new List<string>();
        placedObjects = new List<CampObjectData>();
    }
}
