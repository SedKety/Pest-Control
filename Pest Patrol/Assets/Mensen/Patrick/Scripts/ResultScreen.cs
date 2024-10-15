using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultScreen : MonoBehaviour
{

    public TMP_Text waveRecordText;

    public void Win()
    {
        PauseGame();
        UnlockEndlessMode();
    }

    public void Lose()
    {
        PauseGame();
        SetWaveRecord();
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void SetWaveRecord()
    {
        waveRecordText.text = "Wave Record: " + WaveSystem.wave;
    }

    public void UnlockEndlessMode()
    {
        PlayerPrefs.SetInt("UnlockedEndlessMode", 1);
    }
}
