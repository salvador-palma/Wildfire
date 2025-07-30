using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;
using UnityEngine.UI;

public class LeaderBoardUI : MonoBehaviour
{
    public GameObject leaderboardPanel;
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            if (SteamManager.Initialized)
            {
                
                SteamLeaderboardManager.Instance.DisplayScores(leaderboardPanel);
            }
            else
            {
                Debug.LogError("Steam is not initialized!");
            }
        }
    }
}
