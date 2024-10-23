using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockEndlessMode : MonoBehaviour
{
    public GameObject toggle;
    public void Start()
    {
        toggle = transform.GetChild(0).gameObject;
        var endlessModeUnlocked = PlayerPrefs.GetInt("UnlockedEndlessMode");
        var unlocked = endlessModeUnlocked == 1;
        toggle.SetActive(unlocked);
    }
}
