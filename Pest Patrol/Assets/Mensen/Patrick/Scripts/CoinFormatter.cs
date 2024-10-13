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

    private void FormatCoins()
    {
        //TODO: make this format correctly, used to work correctly but now just doesnt. we love bipolar code
        var coins = GameManager.points;
        var decimals = Mathf.Log10(coins);
        var moneyAsText = currentCoins;
        
        if (decimals is >= 3 and <= 6)
        {
            moneyAsText = (coins / 1000).ToString("F") + "K";
        }
        if (decimals >= 6)
        {
            moneyAsText = (coins / 1000000).ToString("F") + "M";
        }

        coinText.SetText(moneyAsText);
        coinText.ForceMeshUpdate();

    }
}
