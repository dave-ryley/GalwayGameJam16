﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTreeUI : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrototype;
    private List<AbilityTreeButton> _buttons;
    private Dictionary<string, AbilityTreeButton> _buttonsById;

    private AbilityTreeButton AddButton(AbilityTree.AbilityTreeNode node)
    {
        if(!_buttonsById.ContainsKey(node.ability.id))
        {
            GameObject buttonGO = GameObject.Instantiate(_buttonPrototype, transform);
            buttonGO.SetActive(true);
            AbilityTreeButton button = buttonGO.GetComponent<AbilityTreeButton>();
            button.ability = node.ability;
            _buttonsById.Add(node.ability.id, button);
            _buttons.Add(button);

            if(node.parent == null)
            {
                Transform linkFrom  = button.transform.Find("LinkFrom");
                linkFrom.gameObject.SetActive(false);
            }
            if(node.unlocks.Count == 0)
            {
                Transform linkToGroup  = button.transform.Find("LinkToGroup");
                linkToGroup.gameObject.SetActive(false);
                RectTransform joiner = linkToGroup.Find("Joiner") as RectTransform;
                joiner.sizeDelta = new Vector2(((node.unlocks.Count - 1) * 200f) + 4f, 4f);
            }
            return button;
        }
        return null;
    }

    public void BuildAbilityTree(AbilityTree abilityTree)
    {
        _buttons = new List<AbilityTreeButton>();
        _buttonsById = new Dictionary<string, AbilityTreeButton>();

        int treeBreadth = abilityTree.rootAbilities.Count; // The max count of abilities at any depth
        int treeDepth = 1;
        List<int> breadthPerDepthLevel = new List<int>();
        List<List<AbilityTree.AbilityTreeNode>> depthLevels = new List<List<AbilityTree.AbilityTreeNode>>();
        breadthPerDepthLevel.Add(abilityTree.rootAbilities.Count);

        float initialYOffset = 100f;
        float minXDistanceBetweenButtons = 200f;
        float minYDistanceBetweenButtons = 200f;

        int widestDepth = 0;

        int curTreeDepth = 0;
        // Do the root nodes first and seperately
        List<AbilityTree.AbilityTreeNode> curAbilityTreeBreadthList = new List<AbilityTree.AbilityTreeNode>();
        depthLevels.Add(curAbilityTreeBreadthList);
        List<AbilityTree.AbilityTreeNode> childAbilityTreeBreadthList  = new List<AbilityTree.AbilityTreeNode>();
        for(int i = 0; i < abilityTree.rootAbilities.Count; i++)
        {
            AbilityTree.AbilityTreeNode node = abilityTree.rootAbilities[i];
            curAbilityTreeBreadthList.Add(node);
            foreach(AbilityTree.AbilityTreeNode child in node.unlocks)
            {
                childAbilityTreeBreadthList.Add(child);
            }
            AbilityTreeButton button = AddButton(node);
            RectTransform t = button.transform as RectTransform;
            float xPos = (i * minXDistanceBetweenButtons) - ((breadthPerDepthLevel[curTreeDepth] - 1) * minXDistanceBetweenButtons/2);
            t.anchoredPosition = new Vector2(xPos, initialYOffset + minYDistanceBetweenButtons * curTreeDepth);
        }

        while(childAbilityTreeBreadthList.Count > 0)
        {
            curTreeDepth++;
            treeDepth++;
            curAbilityTreeBreadthList = childAbilityTreeBreadthList;
            depthLevels.Add(curAbilityTreeBreadthList);
            breadthPerDepthLevel.Add(curAbilityTreeBreadthList.Count);
            if(curAbilityTreeBreadthList.Count > treeBreadth)
            {
                widestDepth = curTreeDepth;
                treeBreadth = curAbilityTreeBreadthList.Count;
            }
            childAbilityTreeBreadthList = new List<AbilityTree.AbilityTreeNode>();
            for(int i = 0; i < curAbilityTreeBreadthList.Count; i++)
            {
                AbilityTree.AbilityTreeNode node = curAbilityTreeBreadthList[i];
                foreach(AbilityTree.AbilityTreeNode child in node.unlocks)
                {
                    childAbilityTreeBreadthList.Add(child);
                }
                AbilityTreeButton button = AddButton(node);
                RectTransform t = button.transform as RectTransform;
                float xPos = (i * minXDistanceBetweenButtons) - ((breadthPerDepthLevel[curTreeDepth] - 1) * minXDistanceBetweenButtons/2);
                t.anchoredPosition = new Vector2(xPos, initialYOffset + minYDistanceBetweenButtons * curTreeDepth);
            }
        }

        List<List<Vector2>> spaceUsed = new List<List<Vector2>>();
        float minXOffset = 0f;
        float maxXOffset = 0f;

        // Adjust positions moving up from widest
        for(int i = widestDepth; i < treeDepth; i++)
        {
            List<AbilityTree.AbilityTreeNode> treeNodes = depthLevels[i];
            float childYPos = initialYOffset + minYDistanceBetweenButtons * (i+1);
            foreach(AbilityTree.AbilityTreeNode node in treeNodes)
            {
                _buttonsById.TryGetValue(node.ability.id, out AbilityTreeButton parentButton);
                RectTransform parentTransform = parentButton.transform as RectTransform;
                minXOffset = Mathf.Min(minXOffset, parentTransform.anchoredPosition.x);
                maxXOffset = Mathf.Max(maxXOffset, parentTransform.anchoredPosition.x);
                if(node.unlocks.Count > 0) // Has children to adjust
                {
                    float centerX = parentTransform.anchoredPosition.x;
                    for(int index = 0; index < node.unlocks.Count; index++)
                    {
                        AbilityTree.AbilityTreeNode child = node.unlocks[index];
                        _buttonsById.TryGetValue(child.ability.id, out AbilityTreeButton childButton);
                        RectTransform childTransform = childButton.transform as RectTransform;
                        float xpos = (index * minXDistanceBetweenButtons) - ((node.unlocks.Count - 1) * minXDistanceBetweenButtons/2) + centerX;
                        childTransform.anchoredPosition = new Vector2(xpos, childYPos);
                    }
                }
            }
        }
        // Adjust positions moving down from widest
        for(int i = widestDepth - 1; i >= 0; i--)
        {

        }

        RectTransform thisTransform = transform as RectTransform;
        thisTransform.offsetMax = new Vector2(maxXOffset + 100f, treeDepth * minYDistanceBetweenButtons);
        thisTransform.offsetMin = new Vector2(minXOffset - 100f, 0f);
        Populate();
    }

    public void Populate()
    {
        foreach(AbilityTreeButton button in _buttons)
        {
            button.Populate();
        }
    }
}
