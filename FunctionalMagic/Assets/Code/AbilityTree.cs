using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTree
{
    internal class AbilityTreeNode
    {
        public Ability ability;
        public AbilityTreeNode parent;
        public List<AbilityTreeNode> unlocks;

        public AbilityTreeNode(Ability a, AbilityTreeNode parent, AbilityTree tree)
        {
            this.parent = parent;
            tree.abilityNodesById.Add(a.id, this);
            ability = a;
            unlocks = new List<AbilityTreeNode>();
            foreach(string id in a.unlocksAbilities)
            {
                if(!tree.abilityNodesById.TryGetValue(id, out AbilityTreeNode childNode) && tree.abilitiesById.TryGetValue(id, out Ability childAbility))
                {
                    AbilityTreeNode cNode = new AbilityTreeNode(childAbility, this, tree);
                    unlocks.Add(cNode);
                }
            }
        }
    }

    public Dictionary<string, Ability> abilitiesById;
    internal Dictionary<string, AbilityTreeNode> abilityNodesById;
    internal List<AbilityTreeNode> rootAbilities;

    public AbilityTree(Ability[] abilities)
    {
        abilitiesById = new Dictionary<string, Ability>();
        abilityNodesById = new Dictionary<string, AbilityTreeNode>();
        rootAbilities = new List<AbilityTreeNode>();
        // Create Ability Dictionary
        foreach(Ability ability in abilities)
        {
            abilitiesById.Add(ability.id, ability);
        }
        // Create Root Nodes/Build Tree
        foreach(Ability ability in abilities)
        {
            if(ability.unlockedOnStart)
            {
                ability.unlocked = true;
                rootAbilities.Add(new AbilityTreeNode(ability, null, this));
            }
            else
            {
                ability.unlocked = false;
            }
            ability.currentUpgradeLevel = 0;
        }
    }
}
