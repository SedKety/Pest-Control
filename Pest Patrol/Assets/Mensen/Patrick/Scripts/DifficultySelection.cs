using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DifficultySelection : MonoBehaviour
{
    public void SelectDifficulty(int difficulty)
    {
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }
}
