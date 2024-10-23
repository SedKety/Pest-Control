using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveCounter : MonoBehaviour
{
    private TMP_Text textObject;
    private void Start()
    {
        textObject = GetComponent<TMP_Text>();
        Ticker.OnTickAction += OnTick;
    }

    private void OnTick()
    {
        textObject.text = "Wave: " + WaveSystem.wave;
    }
}
