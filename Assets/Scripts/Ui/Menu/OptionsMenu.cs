using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public TMP_Dropdown resolutionsDropdown;
    private List<Resolution> uniqueResolutions; 

    private void Start()
    {
        ResolutionSetup();
    }

    private void ResolutionSetup()
    {
        Resolution[] resolutions = Screen.resolutions;
        uniqueResolutions = new List<Resolution>();
        
        var highestRefreshRates = new Dictionary<string, Resolution>();

        foreach (var res in resolutions)
        {
            string resKey = $"{res.width}x{res.height}";

            if (!highestRefreshRates.ContainsKey(resKey) || highestRefreshRates[resKey].refreshRate < res.refreshRate)
            {
                highestRefreshRates[resKey] = res;
            }
        }

        uniqueResolutions = highestRefreshRates.Values.ToList();
        // Sort the resolutions by area (width * height) and then by width to ensure correct order from lowest to highest
        uniqueResolutions.Sort((a, b) => a.width * a.height == b.width * b.height ? a.width.CompareTo(b.width) : (a.width * a.height).CompareTo(b.width * b.height));

        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            string option = $"{uniqueResolutions[i].width}x{uniqueResolutions[i].height}";
            options.Add(option);

            if (uniqueResolutions[i].width == Screen.currentResolution.width &&
                uniqueResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }
    public void SetVolume(float volume)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolume(volume);
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found!");
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = uniqueResolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}