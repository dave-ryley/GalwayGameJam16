using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Create New Ability", order = 1)]
public class Ability : ScriptableObject
{
    public enum AbilityType { Ability, Upgrade, Passive, Unique };

    public string id;
    public string abilityName;
    public Sprite image;
    public int upgradeLevels;
    public string [] descriptionPerUpgradeLevel;
    public int levelRequirement;
    public string [] unlocksAbilities;
    public bool unlockedOnStart = false;
    public AbilityType abilityType;
    public int currentUpgradeLevel = 0;
    public bool unlocked;
}
