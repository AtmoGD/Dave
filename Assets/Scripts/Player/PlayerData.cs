using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    public string name = "Dave";
    public int level = 1;
    public int experience = 0;
    public List<string> collectables = new List<string>();
    public List<string> equippedItems = new List<string>();
    public List<string> upgrades = new List<string>();
    public List<string> unlockedObjects = new List<string>();
    public List<string> unlockedAttacks = new List<string>();
    public List<string> unlockedSkills = new List<string>();
    public List<string> unlockedMinions = new List<string>();
    public List<string> unlockedRecipes = new List<string>();
    public List<CampObjectData> placedObjects = new List<CampObjectData>();
}
