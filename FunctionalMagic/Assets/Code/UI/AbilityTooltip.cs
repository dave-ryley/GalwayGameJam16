using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityTooltip : MonoBehaviour
{
    private const string descAddon = "/nAlso increases movement speed by {0}%.";
    private const string warning = "Warning: Text Missing!";
    [SerializeField] private TextMeshProUGUI abilityName;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private TextMeshProUGUI abilityLevel;
    [SerializeField] private TextMeshProUGUI abilityDescription;
    [SerializeField] private TextMeshProUGUI abilityThisLevelDescription;
    [SerializeField] private TextMeshProUGUI abilityNextLevelDescription;
    [SerializeField] private TextMeshProUGUI abilityLevelRequirement;

    void Start()
    {
        HideTooltip();
    }

    // Start is called before the first frame update
    public void PopulateTooltip(Ability ability, Color color)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        abilityIcon.sprite = ability.image;
        abilityIcon.color = color;
        abilityName.color = color;
        abilityLevel.color = color;
        abilityName.text = ability.abilityName;
        abilityLevel.text = ability.currentUpgradeLevel.ToString() + "/" + ability.upgradeLevels;
        abilityDescription.text = ability.description;
        abilityThisLevelDescription.gameObject.SetActive(ability.currentUpgradeLevel > 0);
        if(ability.currentUpgradeLevel > 0)
        {
            if(ability.descriptionPerUpgradeLevel.Length >= ability.currentUpgradeLevel)
            {
                abilityThisLevelDescription.text = "<b>This Level:</b> " + ability.descriptionPerUpgradeLevel[ability.currentUpgradeLevel - 1] + string.Format(descAddon, ability.currentUpgradeLevel);
            }
            else
            {
                abilityThisLevelDescription.text = "<b>This Level:</b> " + warning;
            }
        }
        abilityNextLevelDescription.gameObject.SetActive(ability.currentUpgradeLevel < ability.upgradeLevels);
        if(ability.currentUpgradeLevel < ability.upgradeLevels)
        {
            if(ability.descriptionPerUpgradeLevel.Length >= ability.currentUpgradeLevel + 1)
            {
                abilityNextLevelDescription.text = "<b>Next Level:</b> " + ability.descriptionPerUpgradeLevel[ability.currentUpgradeLevel] + string.Format(descAddon, ability.currentUpgradeLevel + 1);
            }
            else
            {
                abilityNextLevelDescription.text = "<b>Next Level:</b> " + warning;
            }
        }
        int level = GameLogic.GetInstance().GetPlayer().level.level;
        abilityLevelRequirement.gameObject.SetActive(ability.levelRequirement > level);
        if(ability.levelRequirement > level)
        {
            abilityLevelRequirement.text = "Level Requirement: " + ability.levelRequirement;
        }
    }

    public void HideTooltip()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
