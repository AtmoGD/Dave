using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    [SerializeField] private string name = "Dave";
    [SerializeField] private int level = 1;
    [SerializeField] private int experience = 0;
    [SerializeField] private List<string> collectables = new List<string>();
    [SerializeField] private List<string> equippedItems = new List<string>();
    [SerializeField] private List<string> upgrades = new List<string>();
    [SerializeField] private List<string> unlockedObjects = new List<string>();
    [SerializeField] private List<string> unlockedAttacks = new List<string>();
    [SerializeField] private List<string> unlockedSkills = new List<string>();
    [SerializeField] private List<string> unlockedMinions = new List<string>();
    [SerializeField] private List<string> unlockedRecipes = new List<string>();
    [SerializeField] private List<CampObjectData> placedObjects = new List<CampObjectData>();
}
