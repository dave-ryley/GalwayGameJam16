using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterSheet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI attributeText;
    [SerializeField] private TextMeshProUGUI perkText;
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI dexterityText;
    [SerializeField] private TextMeshProUGUI constitutionText;
    [SerializeField] private TextMeshProUGUI intelligenceText;
    [SerializeField] private TextMeshProUGUI wisdomText;
    [SerializeField] private TextMeshProUGUI charismaText;
    [SerializeField] private GameObject [] addAttributeButtons;
    [SerializeField] private AbilityTreeUI abilityTree;
    private bool abilityTreeBuilt = false;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        GameLogic game = GameLogic.GetInstance();
        game.OpenFullScreenPanel();
        gameObject.SetActive(true);
        game.player.events.AddEventListener("onPlayerLevelUp", Populate);
        game.player.events.AddEventListener("onPlayerAttributeAdded", Populate);
        game.player.events.AddEventListener("onPlayerAbilityAdded", Populate);
        Populate();
    }

    public void Close()
    {
        GameLogic game = GameLogic.GetInstance();
        game.CloseFullScreenPanel();
        gameObject.SetActive(false);
        game.player.events.RemoveEventListener("onPlayerLevelUp", Populate);
        game.player.events.RemoveEventListener("onPlayerAttributeAdded", Populate);
        game.player.events.RemoveEventListener("onPlayerAbilityAdded", Populate);
    }

    private void Populate()
    {
        GameLogic game = GameLogic.GetInstance();
        PlayerLevel playerLevel = game.player.level;
        attributeText.text = "Attributes: " + playerLevel.unusedAttributes;
        perkText.text = "Perks: " + playerLevel.unusedPerks;

        strengthText.text = playerLevel.strength.ToString();
        dexterityText.text = playerLevel.dexterity.ToString();
        constitutionText.text = playerLevel.constitution.ToString();
        intelligenceText.text = playerLevel.intelligence.ToString();
        wisdomText.text = playerLevel.wisdom.ToString();
        charismaText.text = playerLevel.charisma.ToString();

        bool hasUnusedAttributes = playerLevel.unusedAttributes > 0;
        foreach(GameObject button in addAttributeButtons)
        {
            button.SetActive(hasUnusedAttributes);
        }
        if(abilityTreeBuilt)
        {
            abilityTree.Populate();
        }
        else
        {
            abilityTree.BuildAbilityTree(playerLevel.abilityTree);
            abilityTreeBuilt = true;
        }
    }
}
