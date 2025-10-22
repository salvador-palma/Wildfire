using UnityEngine;
using Steamworks;
using System;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEditor;

public class SteamLeaderboardManager : MonoBehaviour
{
    public static SteamLeaderboardManager Instance;

    private SteamLeaderboard_t currentLeaderboard;
    private bool leaderboardReady = false;
    private CallResult<LeaderboardFindResult_t> findResult;
    private CallResult<LeaderboardScoreUploaded_t> uploadResult;
    private CallResult<LeaderboardScoresDownloaded_t> downloadResult;

    private Dictionary<string, SteamLeaderboard_t> characterLeaderboards;
    void Awake()
    {
        characterLeaderboards = new Dictionary<string, SteamLeaderboard_t>();
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
            FindLeaderboard("HighscoreBase");
            Debug.Log("Steam is initialized!");

        }
    }

    private void FindLeaderboard(string leaderboardName, bool isCharacterLeaderboard = false, Action onFound = null)
    {
        SteamAPICall_t apiCall = SteamUserStats.FindOrCreateLeaderboard(leaderboardName, ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
        findResult = CallResult<LeaderboardFindResult_t>.Create();
        findResult.Set(apiCall, (LeaderboardFindResult_t result, bool failure) =>
        {
            if (!failure && result.m_bLeaderboardFound != 0)
            {
                if (isCharacterLeaderboard)
                {
                    characterLeaderboards[leaderboardName] = result.m_hSteamLeaderboard;
                    onFound();
                    Debug.Log($"Character leaderboard '{leaderboardName}' found and assigned.");
                }
                else
                {
                    currentLeaderboard = result.m_hSteamLeaderboard;
                    leaderboardReady = true;
                    
                    Debug.Log("Main Leaderboard found and assigned.");
                }
            }
            else
            {
                Debug.LogError($"Failed to find leaderboard: {leaderboardName}");
            }
        });
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
    public void DisplayScores(GameObject leaderboardPanel, DynamicText loadingText, string leaderboardName = "HighscoreBase")
    {
        IEnumerator loadingCor = Loading(loadingText);
        StartCoroutine(loadingCor);
        // Download and display scores
        SteamLeaderboard_t leaderboardToUse;
        if (leaderboardName != "HighscoreBase")
        {
            if (!characterLeaderboards.ContainsKey(leaderboardName))
            {
                FindLeaderboard(leaderboardName, true, () =>
                {
                    DisplayScores(leaderboardPanel, loadingText, leaderboardName);
                });
                StopCoroutine(loadingCor);
                return;
            }
            else
            {
                leaderboardToUse = characterLeaderboards[leaderboardName];
                
            }
        }
        else
        {
            leaderboardToUse = currentLeaderboard;
        }

        SteamAPICall_t apiCall = SteamUserStats.DownloadLeaderboardEntries(leaderboardToUse, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 0, 10);
        downloadResult = CallResult<LeaderboardScoresDownloaded_t>.Create();
        downloadResult.Set(apiCall, (LeaderboardScoresDownloaded_t result, bool failure) =>
        {
            if (!failure && result.m_cEntryCount > 0)
            {
                List<LeaderboardEntryData> entries = new List<LeaderboardEntryData>();
                for (int i = 0; i < Math.Min(result.m_cEntryCount, 5); i++)
                {
                    LeaderboardEntryData entryData = GetLeaderboardEntryData(result.m_hSteamLeaderboardEntries, i);
                    entries.Add(entryData);
                }
                CSteamID[] users = new CSteamID[] { SteamUser.GetSteamID() };
                SteamAPICall_t call = SteamUserStats.DownloadLeaderboardEntriesForUsers(leaderboardToUse, users, users.Length);
                CallResult<LeaderboardScoresDownloaded_t> userEntryCall = CallResult<LeaderboardScoresDownloaded_t>.Create();
                userEntryCall.Set(call, (LeaderboardScoresDownloaded_t result, bool failure) =>
                {
                    if (!failure && result.m_cEntryCount > 0)
                    {
                        Debug.Log("User entry downloaded successfully.");
                        GameObject personalRec = leaderboardPanel.transform.Find("PersonalSlot").gameObject;
                        personalRec.SetActive(true);

                        LeaderboardEntryData personalRecData = GetLeaderboardEntryData(result.m_hSteamLeaderboardEntries, 0);


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
                StopCoroutine(loadingCor);
                leaderboardPanel.transform.Find("PersonalSlot").gameObject.SetActive(false);
                DisplayLeaderboardEntries(leaderboardPanel.transform.Find("Slots").gameObject, new List<LeaderboardEntryData>());
                loadingText.SetText(failure ? "Couldn't connect to Steam" : "No entries found.");
            }
        });
    }
    private LeaderboardEntryData GetLeaderboardEntryData(SteamLeaderboardEntries_t entries, int index)
    {
        LeaderboardEntry_t entry;
        int[] details = new int[1];
        bool success = SteamUserStats.GetDownloadedLeaderboardEntry(entries, index, out entry, details, details.Length);
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
        return personalRecData;
    }
    private void DisplayLeaderboardEntries(GameObject leaderboardPanel, List<LeaderboardEntryData> entries)
    {
        GameObject template = leaderboardPanel.transform.GetChild(0).gameObject;
        foreach (Transform child in leaderboardPanel.transform)
        {
            if (child.gameObject.activeSelf)
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
        entryTemplate.transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = "#" + entry.Rank;
        GameObject characterSlot = entryTemplate.transform.Find("CharacterSlot").gameObject;
        string characterName = Character.Instance.characterDatas[entry.characterID].AbilityName;

        Character.Instance.TransformVesselToCharacter(characterSlot, characterName, !Character.Instance.characterDatas[entry.characterID].Unlocked);
    }

    public void UploadScore(int score, int characterID, Action onHighScore = null)
    {

        void UploadScoreToLeaderboard(SteamLeaderboard_t lb, int score, int[] details, Action onHighScore = null)
        {
            SteamAPICall_t apiCall = SteamUserStats.UploadLeaderboardScore(lb, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, details, details.Length);
            uploadResult = CallResult<LeaderboardScoreUploaded_t>.Create();
            uploadResult.Set(apiCall, (LeaderboardScoreUploaded_t result, bool failure) =>
            {
                if (!failure && result.m_bSuccess != 0)
                {
                    if (result.m_bScoreChanged != 0)
                    {
                        if (onHighScore != null)
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

        Debug.Log("Uploading score: " + score + " for character ID: " + characterID);

        if (!leaderboardReady)
        {
            FindLeaderboard("HighscoreBase", false, () => { UploadScore(score, characterID, onHighScore); });
            return;
        }
        else
        {
            UploadScoreToLeaderboard(currentLeaderboard, score, new int[] { characterID }, onHighScore);
        }


        Character.CharacterData data = Character.Instance.characterDatas[characterID];
        if (characterLeaderboards.ContainsKey(data.Name))
        {
            SteamLeaderboard_t characterLeaderboard = characterLeaderboards[data.Name];
            UploadScoreToLeaderboard(characterLeaderboard, score, new int[] { characterID });
            Debug.Log("Character leaderboard for " + data.Name + " found and score uploaded.");
        }
        else
        {
            FindLeaderboard(data.Name, true, () =>
            {
                SteamLeaderboard_t characterLeaderboard = characterLeaderboards[data.Name];
                UploadScoreToLeaderboard(characterLeaderboard, score, new int[] { characterID });
                Debug.Log("Character leaderboard for " + data.Name + " found and score uploaded.");
            });
        }




    }
    
    public static void UnlockAchievment(string title)
    {
        if (SteamManager.Initialized)
        {
            SteamUserStats.GetAchievement(title, out bool Beaten);
            if (!Beaten)
            {
                SteamUserStats.SetAchievement(title);
                SteamUserStats.StoreStats();
            }
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
