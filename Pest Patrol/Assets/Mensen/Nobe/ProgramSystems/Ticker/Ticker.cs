using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker : MonoBehaviour
{
    public float tickTime;
    public bool displayTicks;

    float tickerTimer;

    public delegate void TickAction();
    public static event TickAction OnTickAction;
    public void Start()
    {
        StartCoroutine(CustomUpdateLoop_ChainFrames());
    }
    private IEnumerator CustomUpdateLoop_ChainFrames()
    {
        float tickerTimer = 0;

        while (true)
        {
            yield return null;

            tickerTimer += Time.deltaTime;
            if (tickerTimer >= tickTime)
            {
                while (tickerTimer > tickTime)
                {
                    TickEvent();
                    tickerTimer -= tickTime;
                    if(displayTicks)print("tick");
                }
            }
        }

        void TickEvent()
        {
            OnTickAction?.Invoke();
        }
    }
}

