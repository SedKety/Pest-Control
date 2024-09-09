using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;
    public int points;

    private void Awake()
    {
        Instance = this;
    }
    public void ModifyPoints(int amount)
    {
        if ((points + amount) > 0)
        {
            points += amount;
        }
        else
        {
            return;
        }
    }
}
