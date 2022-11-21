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
    [SerializeField] public List<string> collectables = new List<string>();
    [SerializeField] public List<string> equippedItems = new List<string>();
    [SerializeField] public List<string> upgrades = new List<string>();
    [SerializeField] public List<string> unlockedObjects = new List<string>();
    [SerializeField] public List<string> unlockedSkills = new List<string>();
    [SerializeField] public List<string> equippedSkills = new List<string>();
    [SerializeField] public List<string> unlockedMinions = new List<string>();
    [SerializeField] public List<string> unlockedRecipes = new List<string>();
    [SerializeField] public List<CampObjectData> placedObjects = new List<CampObjectData>();
}
