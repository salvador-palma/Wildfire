using UnityEngine;
using Steamworks;
using System;
using System.Collections.Generic;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;

public class SteamLeaderboardManager : MonoBehaviour
{
    public static SteamLeaderboardManager Instance;

    private SteamLeaderboard_t currentLeaderboard;
    private bool leaderboardReady = false;
    private CallResult<LeaderboardFindResult_t> findResult;
    private CallResult<LeaderboardScoreUploaded_t> uploadResult;
    private CallResult<LeaderboardScoresDownloaded_t> downloadResult;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam is not initialized!");
            return;
        }
        else
        {
            SteamAPICall_t apiCall = SteamUserStats.FindLeaderboard("HighscoreBase");
            findResult = CallResult<LeaderboardFindResult_t>.Create();
            findResult.Set(apiCall, (LeaderboardFindResult_t result, bool failure) =>
            {
                if (!failure && result.m_bLeaderboardFound != 0)
                {
                    currentLeaderboard = result.m_hSteamLeaderboard;
                    leaderboardReady = true;
                    Debug.Log("Leaderboard found and assigned.");
                }
                else
                {
                    Debug.LogError("Failed to find leaderboard.");
                }
            });
        }
    }

    void Update()
    {
        if (SteamManager.Initialized)
            SteamAPI.RunCallbacks();

        if (Input.GetKeyDown(KeyCode.L))
        {

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SteamAPICall_t apiCall = SteamUserStats.UploadLeaderboardScore(currentLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, 8, new int[] { 0 }, 1);
            uploadResult = CallResult<LeaderboardScoreUploaded_t>.Create();
            uploadResult.Set(apiCall, (LeaderboardScoreUploaded_t result, bool failure) =>
            {
                if (!failure && result.m_bSuccess != 0)
                {
                    Debug.Log("Score uploaded successfully.");
                }
                else
                {
                    Debug.LogError("Failed to upload score.");
                }
            });
            //Debug.Log("Score uploaded to leaderboard.");
        }
    }


    public void DisplayScores(GameObject leaderboardPanel)
    {

        

        // Download and display scores
        SteamAPICall_t apiCall = SteamUserStats.DownloadLeaderboardEntries(currentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 0, 10);
        downloadResult = CallResult<LeaderboardScoresDownloaded_t>.Create();
        downloadResult.Set(apiCall, (LeaderboardScoresDownloaded_t result, bool failure) =>
        {
            if (!failure && result.m_cEntryCount > 0)
            {
                List<LeaderboardEntryData> entries = new List<LeaderboardEntryData>();
                for (int i = 0; i < result.m_cEntryCount; i++)
                {
                    LeaderboardEntryData entryData = new LeaderboardEntryData();

                    LeaderboardEntry_t entry;
                    int[] details = new int[10];
                    SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, i, out entry, details, details.Length);

                    entryData.Rank = entry.m_nGlobalRank;
                    entryData.Score = entry.m_nScore;
                    entryData.PlayerName = SteamFriends.GetFriendPersonaName(entry.m_steamIDUser);
                    entryData.SteamID = entry.m_steamIDUser;
                    entryData.characterID = details[0];

                    // Get Steam avatar as a Sprite
                    int avatarInt = SteamFriends.GetLargeFriendAvatar(entry.m_steamIDUser);
                    if (avatarInt != -1)
                    {
                        uint width, height;
                        if (SteamUtils.GetImageSize(avatarInt, out width, out height) && width > 0 && height > 0)
                        {
                            byte[] image = new byte[4 * width * height];
                            if (SteamUtils.GetImageRGBA(avatarInt, image, (int)(4 * width * height)))
                            {
                                Texture2D texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                                texture.LoadRawTextureData(image);
                                texture.Apply();
                                entryData.AvatarSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                            }
                        }
                    }
                    else
                    {
                        entryData.AvatarSprite = null;
                    }
                    // Add the entry data to the list
                    entries.Add(entryData);
                }

                DisplayLeaderboardEntries(leaderboardPanel, entries);
            }
            else
            {
                Debug.LogError("Failed to download leaderboard entries.");
            }
        });
    }

    private void DisplayLeaderboardEntries(GameObject leaderboardPanel, List<LeaderboardEntryData> entries)
    {
        GameObject template = leaderboardPanel.transform.GetChild(0).gameObject;
        foreach (Transform child in leaderboardPanel.transform)
        {
            if (child.gameObject.activeInHierarchy)
                Destroy(child.gameObject);
        }

        foreach (LeaderboardEntryData entry in entries)
        {
            GameObject entryTemplate = Instantiate(template, leaderboardPanel.transform);

            entryTemplate.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = entry.Score.ToString();
            entryTemplate.transform.Find("Nickname").GetComponent<TextMeshProUGUI>().text = entry.PlayerName;
            entryTemplate.transform.Find("Avatar").GetComponent<UnityEngine.UI.Image>().sprite = entry.AvatarSprite;
            
            entryTemplate.SetActive(true);
        }
    }

 
}

// ---------------------------------------------------------
// DATA STRUCT FOR ENTRIES
// ---------------------------------------------------------
[Serializable]
public class LeaderboardEntryData
{
    public int Rank;
    public int Score;
    public string PlayerName;
    public CSteamID SteamID;
    public Sprite AvatarSprite; // For the avatar image
    public int characterID; // your extra fields like characterID, difficulty, etc.
}
