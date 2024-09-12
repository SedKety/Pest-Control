using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public TMP_Dropdown[] resolutionDropdown;
    private Resolution[] resolutions;
    public List<Resolution> resolutionList = new();
    private double currentRefRate;
    public int currentResolutionIndex = 0;
    public string currentRes;
    private void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        Application.targetFrameRate = PlayerPrefs.GetInt("fps");
        foreach (var v in resolutionDropdown)
        {
            v.ClearOptions();
        }
        resolutions = Screen.resolutions;
        resolutionList = new List<Resolution>();
        currentRefRate = Screen.currentResolution.refreshRateRatio.value;
        Debug.LogWarning(resolutions.Length);
        for (int i = 0; i < resolutions.Length; i++)
        {
            float refRate = (float)currentRefRate;
            float rate = (float)resolutions[i].refreshRateRatio.value;
            if (Mathf.Approximately(refRate, rate))
            {
                resolutionList.Add(resolutions[i]);
            }
            else
            {
                Debug.LogWarning("refresh rate not correct");
            }
        }
        List<string> options = new List<string>();
        for (int i = 0; i < resolutionList.Count; i++)
        {
            string resolutionOption = resolutionList[i].width + "x" + resolutionList[i].height + " " + Mathf.Round((float)resolutionList[i].refreshRateRatio.value) + " Hz";
            options.Add(resolutionOption);
            if (resolutionList[i].width == Screen.width && resolutionList[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
            foreach (var v in resolutionDropdown)
            {
                v.RefreshShownValue();
            }
        }
        foreach (var v in resolutionDropdown)
        {
            v.AddOptions(options);
            v.value = currentResolutionIndex;
            v.RefreshShownValue();
        }
        SetResolution(resolutionList.Count - 1);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutionList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
        foreach (var v in resolutionDropdown)
        {
            v.value = resolutionIndex;
            v.RefreshShownValue();
        }
        PlayerPrefs.SetInt("resolution", resolutionIndex);
    }
    public void SetAudio(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("volume", value);
    }

    public void SetFPS(string value)
    {
        if (int.Parse(value) <= 0) Application.targetFrameRate = -1;
        else Application.targetFrameRate = int.Parse(value);
        PlayerPrefs.SetInt("fps", int.Parse(value));
    }
}
