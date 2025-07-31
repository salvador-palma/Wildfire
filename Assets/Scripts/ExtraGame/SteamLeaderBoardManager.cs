using UnityEngine;
using Steamworks;
using System;
using System.Collections.Generic;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor.U2D.Animation;
using System.Collections;

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
        Debug.Log("Steam test");
        if (!SteamManager.Initialized)
        {
            Debug.Log("Steam is NOT initialized!");
            return;
        }
        else
        {
            Debug.Log("Steam is initialized!");
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


    }

    private IEnumerator Loading(DynamicText loadingText)
    {
        int i = 0;
        string defaultText = "Fetching Data";
        while (true)
        {
            loadingText.SetText(defaultText);
            loadingText.GetComponent<TextMeshProUGUI>().text += new string('.', i % 3);
            i++;
            if (i > 3) i = 0;
            yield return new WaitForSeconds(1f);
        }
    }
    public void DisplayScores(GameObject leaderboardPanel, DynamicText loadingText)
    {
        IEnumerator loadingCor = Loading(loadingText);
        StartCoroutine(loadingCor);
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
                CSteamID[] users = new CSteamID[] { SteamUser.GetSteamID() };
                SteamAPICall_t call = SteamUserStats.DownloadLeaderboardEntriesForUsers(currentLeaderboard, users, users.Length);
                CallResult<LeaderboardScoresDownloaded_t> userEntryCall = CallResult<LeaderboardScoresDownloaded_t>.Create();
                userEntryCall.Set(call, (LeaderboardScoresDownloaded_t result, bool failure) =>
                {
                    if (!failure && result.m_cEntryCount > 0)
                    {
                        Debug.Log("User entry downloaded successfully.");
                        GameObject personalRec = leaderboardPanel.transform.Find("PersonalSlot").gameObject;
                        LeaderboardEntry_t entry;
                        int[] details = new int[1];
                        bool success = SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, 0, out entry, details, details.Length);
                        LeaderboardEntryData personalRecData = new LeaderboardEntryData();
                        personalRecData.Rank = entry.m_nGlobalRank;
                        personalRecData.Score = entry.m_nScore;
                        personalRecData.PlayerName = SteamFriends.GetFriendPersonaName(entry.m_steamIDUser);
                        personalRecData.SteamID = entry.m_steamIDUser;
                        personalRecData.characterID = details[0];
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
                                    personalRecData.AvatarSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                                }
                            }
                        }
                        else
                        {
                            personalRecData.AvatarSprite = null;
                        }
                        personalRec.SetActive(true);
                        
                        ChangeRecordVessel(personalRec, personalRecData);
                    }
                    else
                    {
                       leaderboardPanel.transform.Find("PersonalSlot").gameObject.SetActive(false);
                        Debug.LogError("Failed to download user entry.");
                    }
                        
                
                });
                StopCoroutine(loadingCor);
                loadingText.SetTextDirect("");
                DisplayLeaderboardEntries(leaderboardPanel.transform.Find("Slots").gameObject, entries);
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

            ChangeRecordVessel(entryTemplate, entry);

            entryTemplate.SetActive(true);
        }
    }
    private void ChangeRecordVessel(GameObject entryTemplate, LeaderboardEntryData entry)
    {
        entryTemplate.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = GameUI.RoundsToTime(entry.Score);
        entryTemplate.transform.Find("Nickname").GetComponent<TextMeshProUGUI>().text = entry.PlayerName;
        entryTemplate.transform.Find("Avatar").GetComponent<UnityEngine.UI.Image>().sprite = entry.AvatarSprite;
        entryTemplate.transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = "#"+entry.Rank;
        GameObject characterSlot = entryTemplate.transform.Find("CharacterSlot").gameObject;
        string characterName = Character.Instance.characterDatas[entry.characterID].AbilityName;
        Character.Instance.TransformVesselToCharacter(characterSlot, characterName);
    }

    public void UploadScore(int score, int characterID, Action onHighScore = null)
    {
        Debug.Log("Uploading score: " + score + " for character ID: " + characterID);
        if (!leaderboardReady)
        {
            Debug.LogError("Leaderboard is not ready.");
            return;
        }

        SteamAPICall_t apiCall = SteamUserStats.UploadLeaderboardScore(
            currentLeaderboard,
            ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest,
            score,
            new int[] { characterID },
            1
        );
        uploadResult = CallResult<LeaderboardScoreUploaded_t>.Create();
        uploadResult.Set(apiCall, (LeaderboardScoreUploaded_t result, bool failure) =>
        {
            if (!failure && result.m_bSuccess != 0)
            {
                if (result.m_bScoreChanged != 0)
                {
                    if( onHighScore != null)
                        onHighScore.Invoke();
                    Debug.Log("Score uploaded and is a new personal best!");
                }
                else
                {
                    Debug.Log("Score uploaded, but did not beat previous best.");
                }
            }
            else
            {
                Debug.LogError("Failed to upload score.");
            }
        });
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
