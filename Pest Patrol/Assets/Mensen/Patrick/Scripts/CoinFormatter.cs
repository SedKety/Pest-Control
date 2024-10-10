using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinFormatter : MonoBehaviour
{
    private string previousCoins;
    private string currentCoins;
    public TMP_Text coinText;
    public void Start()
    {
        previousCoins = GameManager.points.ToString();
        currentCoins = previousCoins;
        Ticker.OnTickAction += OnTick;
    }

    private void OnTick()
    {
        currentCoins = GameManager.points.ToString();
        if (currentCoins != previousCoins)
        {
            FormatCoins();
        }
        previousCoins = currentCoins;
    }
    public void FormatCoins()
    {
        var coins = GameManager.points;
        var decimals = Mathf.Log10(coins);
        string moneyAsText = currentCoins;
        print(decimals);
        
        if (decimals is >= 3 and <= 6)
        {
            moneyAsText = (coins / 1000).ToString("F") + "K";
            print(moneyAsText);
        }
        if (decimals >= 6)
        {
            moneyAsText = (coins / 1000000).ToString("F") + "M";
            print(moneyAsText);
        }

        coinText.SetText(moneyAsText);
        coinText.ForceMeshUpdate();

    }
}
