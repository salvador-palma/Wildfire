using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DropdownScript : MonoBehaviour
{
    TMP_Dropdown dropdown;

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown windowModeDropdown;
    public TMP_Dropdown fpsCapDropdown;

    public Resolution[] resolutions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();

        foreach(string lang in Translator.getLanguagesAvailable())
        {
            TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData(lang);
            dropdown.options.Add(newOption);
        }
        dropdown.RefreshShownValue();
        dropdown.value = Translator.getCurrentLanguageID();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);


        PopulateResolutionDropdown();
        PopulateWindowModeDropdown();
        PopulateFPSCapDropdown();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        windowModeDropdown.onValueChanged.AddListener(SetWindowMode);
        fpsCapDropdown.onValueChanged.AddListener(SetFPSCap);
    }

    private void OnDropdownValueChanged(int value)
    {   
        // Change language
        Translator.changeLanguage(dropdown.options[value].text);
        Debug.Log("Language changed to " + dropdown.options[value].text);
 
    }

    // GRAPHICS SETTINGS
    void SetWindowMode(int index)
    {
        switch (index)
        {
            case 0: Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; break;
            case 1: Screen.fullScreenMode = FullScreenMode.Windowed; break;
            case 2: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
        }
    }
    void SetResolution(int index)
    {
        Resolution selectedRes = resolutions[index];
        Screen.SetResolution(selectedRes.width, selectedRes.height, Screen.fullScreenMode);
    }

    void SetFPSCap(int index)
    {
        string selectedOption = fpsCapDropdown.options[index].text;
        Application.targetFrameRate = selectedOption == "Unlimited" ? -1 : int.Parse(selectedOption);
    }

    void PopulateResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutions = resolutions.ToList().Distinct().ToArray();
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (Resolution res in resolutions)
        {
            options.Add(res.width + "x" + res.height );
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.RefreshShownValue();
    }

    void PopulateWindowModeDropdown()
    {
        windowModeDropdown.ClearOptions();
        windowModeDropdown.AddOptions(new List<string> { "Fullscreen", "Windowed", "Borderless" });
        windowModeDropdown.value = (int)Screen.fullScreenMode;
        windowModeDropdown.RefreshShownValue();
    }

    void PopulateFPSCapDropdown()
    {
        fpsCapDropdown.ClearOptions();
        List<string> options = new List<string> { "30", "60", "120", "144", "165", "240", "Unlimited" };
        fpsCapDropdown.AddOptions(options);
        fpsCapDropdown.value = options.IndexOf(Application.targetFrameRate == -1 ? "Unlimited" : Application.targetFrameRate.ToString());
        fpsCapDropdown.RefreshShownValue();
    }

    int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return 0;
    }

    

}
