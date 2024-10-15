using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DifficultySelection : MonoBehaviour
{
    
    public void SelectDifficulty(int difficulty)
    {
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }

    public void EndlessMode(Toggle toggle)
    {
        var value = toggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("EndlessMode", value);
        
    }

    public void ChooseMap(int sceneIndex)
    {
        FindAnyObjectByType<Settings>().selectedScene = sceneIndex;
    }
    public void Start()
    {
        var endlessModeUnlocked = PlayerPrefs.GetInt("UnlockedEndlessMode");
        var unlocked = endlessModeUnlocked == 1;
        GetComponentInChildren<Toggle>().interactable = unlocked;
    }
}
