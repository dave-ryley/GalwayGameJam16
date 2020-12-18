using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private float XP_SCALE = 1.25f;
    private Player _player;

    public AbilityTree abilityTree;
    public int level = 1;
    public int xp = 0;
    public int xpRequired = 100;

    public int strength = 8;
    public int dexterity = 8;
    public int constitution = 8;
    public int intelligence = 8;
    public int wisdom = 8;
    public int charisma = 8;

    public int unusedAttributes = 0;
    public int unusedPerks = 0;

    public void SetUp(Player player, Ability[] abilities)
    {
        _player = player;
        abilityTree = new AbilityTree(abilities);
    }

    public void AddXP(int amount)
    {
        xp += amount;

        if(xp >= xpRequired)
        {
            int amountOver = xp - xpRequired;
            LevelUp();
            AddXP(amountOver);
        }
    }

    public void LevelUp()
    {
        level++;
        xp = 0;
        xpRequired = (int) (xpRequired * XP_SCALE);
        unusedAttributes++;
        unusedPerks++;
        _player.events.DispatchEvent("onPlayerLevelUp");
    }

    public void AddStrengthPoint()
    {
        strength++;
        unusedAttributes--;
        _player.events.DispatchEvent("onPlayerAttributeAdded");
    }

    public void AddDexterityPoint()
    {
        dexterity++;
        unusedAttributes--;
        _player.events.DispatchEvent("onPlayerAttributeAdded");
    }

    public void AddConstitutionPoint()
    {
        constitution++;
        unusedAttributes--;
        _player.events.DispatchEvent("onPlayerAttributeAdded");
    }

    public void AddIntelligencePoint()
    {
        intelligence++;
        unusedAttributes--;
        _player.events.DispatchEvent("onPlayerAttributeAdded");
    }

    public void AddWisdomPoint()
    {
        wisdom++;
        unusedAttributes--;
        _player.events.DispatchEvent("onPlayerAttributeAdded");
    }

    public void AddCharismaPoint()
    {
        charisma++;
        unusedAttributes--;
        _player.events.DispatchEvent("onPlayerAttributeAdded");
    }

    public void AttemptAquireAbility(Ability ability)
    {
        Debug.Log("Attempt to Aquire Ability" + ability.name);
        if(unusedPerks > 0
            && ability.unlocked
            && ability.currentUpgradeLevel < ability.upgradeLevels
            && ability.levelRequirement <= level)
        {
            unusedPerks--;
            ability.currentUpgradeLevel++;
            foreach(string id in ability.unlocksAbilities)
            {
                if(abilityTree.abilitiesById.TryGetValue(id, out Ability unlockedAbility))
                {
                    unlockedAbility.unlocked = true;
                }
            }
            _player.events.DispatchEvent("onPlayerAbilityAdded");
        }
    }
}
