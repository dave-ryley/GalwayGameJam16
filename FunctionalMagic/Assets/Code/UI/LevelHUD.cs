using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;
    public RectTransform xpBar;

    void Start()
    {
        GameLogic game = GameLogic.GetInstance();
        Player player = game.GetPlayer();

        player.events.AddEventListener("onPlayerXPUpdated", OnPlayerXPUpdated);
        player.events.AddEventListener("onPlayerLevelUp", OnPlayerLevelUp);

        OnPlayerXPUpdated();
        OnPlayerLevelUp();
    }

    #region events

    private void OnPlayerXPUpdated()
    {
        PlayerLevel playerLevel = GameLogic.GetInstance().GetPlayer().level;
        xpText.text = playerLevel.xp + "/" + playerLevel.xpRequired + " XP";
        xpBar.anchorMax = new Vector2((float) playerLevel.xp / playerLevel.xpRequired, 1f);
    }

    private void OnPlayerLevelUp()
    {
        PlayerLevel playerLevel = GameLogic.GetInstance().GetPlayer().level;
        levelText.text = "Level " + playerLevel.level;
    }

    #endregion
}
