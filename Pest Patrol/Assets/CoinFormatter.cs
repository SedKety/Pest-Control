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
        Debug.Log("format me daddy");
        string coins = GameManager.points.ToString();
        float decimals = Mathf.Log10(float.Parse(coins));
        string moneyAsText = coins;
        if (decimals >= 3 && decimals <= 6)
        {
            moneyAsText = (float.Parse(coins) / 1000).ToString("F") + "K";
        }
        if (decimals >= 6)
        {
            moneyAsText = (float.Parse(coins) / 1000000).ToString("F") + "M";
        }
        gameObject.transform.GetComponent<TMP_Text>().text = moneyAsText;
    }

    public void AddCoins(string input)
    {
        long coinsToAdd = long.Parse(input);
        GameManager.AddPoints(coinsToAdd);
    }
}
