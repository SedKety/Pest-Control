using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DifficultySelection : MonoBehaviour
{
    private GameObject currentlySelectedDifficulty;
    public void SelectDifficulty(int difficulty)
    {
        if (currentlySelectedDifficulty != null)
        {
            if (currentlySelectedDifficulty != EventSystem.current.currentSelectedGameObject)
            {
                currentlySelectedDifficulty.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
        }
        currentlySelectedDifficulty = EventSystem.current.currentSelectedGameObject;
        currentlySelectedDifficulty.transform.GetChild(0).GetComponent<Image>().color = Color.black;
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
