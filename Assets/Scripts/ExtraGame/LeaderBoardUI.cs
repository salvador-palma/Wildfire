using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;
using UnityEngine.UI;

public class LeaderBoardUI : MonoBehaviour
{
    public GameObject leaderboardPanel;
    public DynamicText loadingText;
    public DynamicText titleText;

    public void OpenLeaderboardPanel()
    {

        MetaMenuUI.Instance.ToggleMenu(leaderboardPanel.transform.parent.gameObject);
        SteamLeaderboardManager.Instance.DisplayScores(leaderboardPanel, loadingText, "HighscoreBase");
    }
    public void LoadLeaderboard(GameObject leaderboardPanel, DynamicText loadingText, string leaderboardName)
    {
        SteamLeaderboardManager.Instance.DisplayScores(leaderboardPanel, loadingText, leaderboardName);
    }   

    public void CloseLeaderboardPanel()
    {

        MetaMenuUI.Instance.ToggleMenu(leaderboardPanel.transform.parent.gameObject);

    }
}
