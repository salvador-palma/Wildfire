using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    static string version = "V1.6";
    public static string device = "PC"; //Mobile, PC, Web

    [SerializeField] Button CreditsButton;
    private void Awake()
    {
        Debug.LogError("Player Log Create");


        if (PlayerPrefs.GetInt(version, 0) == 0)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt(version, 1);
            string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
            foreach (string filePath in filePaths)
            {
                try
                {
                    Debug.Log("Deleting: " + filePath);
                    File.Delete(filePath);
                }
                catch
                {
                    Debug.Log("Error Deleting File: " + filePath);
                }
            }


        }
        if (PlayerPrefs.GetInt("V1.6-CharPatchFixing", 0) == 0)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("V1.6-CharPatchFixing", 1);
            PlayerPrefs.SetInt(version, 1);
        }



        if (GameVariables.GetVariable("JunoReady") != 1)
        {
            CreditsButton.gameObject.SetActive(false);
        }


    }
    public void LoadGameScene()
    {

        SceneManager.LoadScene("MetaGame");
    }

    public void StartFadeOut()
    {

        GetComponent<Animator>().Play("MenuFadeout");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void CreditsRoll()
    {
        GetComponent<Animator>().Play("CreditsRoll");
    }







}
