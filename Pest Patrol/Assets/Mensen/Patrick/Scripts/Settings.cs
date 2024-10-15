using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    public TMP_Dropdown[] resolutionDropdown;
    private Resolution[] resolutions;
    public List<Resolution> resolutionList = new();
    private double currentRefRate;
    public int currentResolutionIndex = 0;
    public string currentRes;
    public Slider audioSlider;
    public TMP_InputField[] inputFields;

    public void EnterGame()
    {
        print("hi");
        SceneManager.LoadScene(1);
    }
    private void Start()
    {
        SetAudio(PlayerPrefs.GetFloat("volume"));
        SetFPS(PlayerPrefs.GetInt("fps").ToString());
        SetMoveSpeed(PlayerPrefs.GetFloat("moveSpeed").ToString());
        SetSensitivity(PlayerPrefs.GetFloat("sens").ToString());
        foreach (var v in resolutionDropdown)
        {
            v.ClearOptions();
        }
        resolutions = Screen.resolutions;
        resolutionList = new List<Resolution>();
        currentRefRate = Screen.currentResolution.refreshRateRatio.value;
        //Debug.LogWarning(resolutions.Length);
        for (var i = 0; i < resolutions.Length; i++)
        {
            var refRate = (float)currentRefRate;
            var rate = (float)resolutions[i].refreshRateRatio.value;
            if (Mathf.Approximately(refRate, rate))
            {
                resolutionList.Add(resolutions[i]);
            }
            else
            {
                Debug.LogWarning("refresh rate not correct");
            }
        }
        var options = new List<string>();
        for (var i = 0; i < resolutionList.Count; i++)
        {
            var resolutionOption = resolutionList[i].width + "x" + resolutionList[i].height + " " + Mathf.Round((float)resolutionList[i].refreshRateRatio.value) + " Hz";
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
        audioSlider.value = value;
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("volume", value);
    }

    public void SetFPS(string value)
    {
        inputFields[0].text = value;
        if (int.Parse(value) <= 0) Application.targetFrameRate = -1;
        else Application.targetFrameRate = int.Parse(value);
        PlayerPrefs.SetInt("fps", int.Parse(value));
    }

    public void SetSensitivity(string value)
    {
        inputFields[1].text = value;
        try
        {
            if (float.Parse(value) <= 0) CameraController.Instance.UpdateMouseSens(1);
            else CameraController.Instance.UpdateMouseSens(float.Parse(value));
        }
        catch { Debug.Log("No camera controller present."); }
        PlayerPrefs.SetFloat("sens", float.Parse(value));
    }
    public void SetMoveSpeed(string value)
    {
        inputFields[2].text = value;
        try
        {
            if (float.Parse(value) <= 0) CameraController.Instance.moveSpeed = 1;
            else CameraController.Instance.moveSpeed = float.Parse(value);
        }
        catch { Debug.Log("No camera controller present"); }
        PlayerPrefs.SetFloat("moveSpeed", float.Parse(value));
    }
}
