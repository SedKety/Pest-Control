using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultScreen : MonoBehaviour
{
    public void SetWaveRecord()
    {
        GetComponent<TMP_Text>().text = "Wave Record: " + WaveSystem.wave;
    }

    public void UnlockEndlessMode()
    {
        PlayerPrefs.SetInt("UnlockedEndlessMode", 1);
    }
}
