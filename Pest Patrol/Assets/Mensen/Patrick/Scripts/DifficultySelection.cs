using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DifficultySelection : MonoBehaviour
{
    public GameObject[] previews;
    private GameObject currentlySelectedDifficulty;
    private GameObject currentlySelectedLevel;
    public GameObject toggle;
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
        if (currentlySelectedLevel != null)
        {
            if (currentlySelectedLevel != EventSystem.current.currentSelectedGameObject)
            {
                currentlySelectedLevel.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
        }
        currentlySelectedLevel = EventSystem.current.currentSelectedGameObject;
        currentlySelectedLevel.transform.GetChild(0).GetComponent<Image>().color = Color.black;
        foreach (var preview in previews) preview.GetComponent<Image>().color = Color.clear;
        previews[previews.Length - sceneIndex].GetComponent<Image>().color = Color.black;
        if (sceneIndex == 0) sceneIndex = 1;
        FindAnyObjectByType<Settings>().selectedScene = sceneIndex;
    }
}
