using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// TODO: Add tooltip
public class AbilityTreeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private AbilityTreeUI parentUI;
    public Ability ability;
    public TextMeshProUGUI currentLevelText;
    public Image image;
    public Button button;
    public GameObject linkToGroup;
    public RectTransform joiner;

    public Color normalColorAbility;
    public Color highlightedColorAbility;
    public Color pressedColorAbility;

    public Color normalColorUpgrade;
    public Color highlightedColorUpgrade;
    public Color pressedColorUpgrade;

    public Color normalColorPassive;
    public Color highlightedColorPassive;
    public Color pressedColorPassive;

    public Color normalColorUnique;
    public Color highlightedColorUnique;
    public Color pressedColorUnique;

    public Color normalColorDisabled;
    public Color highlightedColorDisabled;
    public Color pressedColorDisabled;

    public void OnClickButton()
    {
        GameLogic.GetInstance().GetPlayer().level.AttemptAquireAbility(ability);

        var pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.deselectHandler);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        parentUI.OnEnterAbilityButton(ability, GetActualColor());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        parentUI.OnExitAbilityButton();
    }

    public void Setup(AbilityTreeUI parentUI, Ability ability)
    {
        this.ability = ability;
        this.parentUI = parentUI;
    }

    public Color GetActualColor()
    {
        switch(ability.abilityType)
            {
            case Ability.AbilityType.Ability:
                return normalColorAbility;
            case Ability.AbilityType.Upgrade:
                return normalColorUpgrade;
            case Ability.AbilityType.Unique:
                return normalColorUnique;
            default:
                return normalColorPassive;
            }
    }

    public ColorBlock GetColorBlock()
    {
        ColorBlock colors = new ColorBlock();
        colors.colorMultiplier = 1;
        PlayerLevel playerLevel = GameLogic.GetInstance().GetPlayer().level;
        if(ability.unlocked && ability.levelRequirement <= playerLevel.level)
        {
            switch(ability.abilityType)
            {
                case Ability.AbilityType.Ability:
                    colors.normalColor = normalColorAbility;
                    colors.highlightedColor = highlightedColorAbility;
                    colors.pressedColor = pressedColorAbility;
                    break;
                case Ability.AbilityType.Upgrade:
                    colors.normalColor = normalColorUpgrade;
                    colors.highlightedColor = highlightedColorUpgrade;
                    colors.pressedColor = pressedColorUpgrade;
                    break;
                case Ability.AbilityType.Unique:
                    colors.normalColor = normalColorUnique;
                    colors.highlightedColor = highlightedColorUnique;
                    colors.pressedColor = pressedColorUnique;
                    break;
                default:
                    colors.normalColor = normalColorPassive;
                    colors.highlightedColor = highlightedColorPassive;
                    colors.pressedColor = pressedColorPassive;
                    break;
            }
        }
        else
        {
            colors.normalColor = normalColorDisabled;
            colors.highlightedColor = highlightedColorDisabled;
            colors.pressedColor = pressedColorDisabled;
        }
        return colors;
    }

    public void Populate()
    {
        image.sprite = ability.image;
        currentLevelText.text = ability.currentUpgradeLevel.ToString() + "/" + ability.upgradeLevels.ToString();
        ColorBlock colors = GetColorBlock();
        colors.selectedColor = colors.highlightedColor;
        button.colors = colors;
        currentLevelText.color = button.colors.normalColor;
    }
}
