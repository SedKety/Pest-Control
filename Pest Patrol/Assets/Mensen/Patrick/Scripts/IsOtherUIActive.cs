using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOtherUIActive : MonoBehaviour
{
    public GameObject sigma;
    
    private void Start()
    {
        Ticker.OnTickAction += OnTick;
    }

    void OnTick()
    {
        gameObject.transform.parent.gameObject.SetActive(!sigma.activeInHierarchy);
    }
}
