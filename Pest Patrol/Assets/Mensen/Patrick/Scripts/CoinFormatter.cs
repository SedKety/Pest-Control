using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinFormatter : MonoBehaviour
{
    public string previousCoins;
    public string currentCoins;
    public void Start()
    {
        previousCoins = GameManager.points.ToString();
        currentCoins = previousCoins;
        Ticker.OnTickAction += OnTick;
    }

    void OnTick()
    {
        currentCoins = GameManager.points.ToString();
        if (currentCoins != previousCoins)
        {
            FormatCoins();
        }
    }
    public void FormatCoins()
    {
        long coins = GameManager.points;
        float decimals = Mathf.Log10(coins);
        string moneyAsText = coins.ToString();
        if (decimals >= 3 && decimals <= 6)
        {
            moneyAsText = (coins / 1000).ToString("F") + "K";
        }
        if (decimals >= 6)
        {
            moneyAsText = (coins / 1000000).ToString("F") + "M";
        }
        gameObject.transform.GetComponent<TMP_Text>().text = moneyAsText;
    }

    public void AddCoins(string input)
    {
        long coinsToAdd = long.Parse(input);
        GameManager.AddPoints(coinsToAdd);
    }
}
