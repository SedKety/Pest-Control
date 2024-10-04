using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOtherUIActive : MonoBehaviour
{
    private void Start()
    {
        Ticker.OnTickAction += OnTick;
    }

    public void OnTick()
    {
        if (FindAnyObjectByType<UpdateText>().gameObject.transform.GetChild(1).gameObject.activeSelf)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
