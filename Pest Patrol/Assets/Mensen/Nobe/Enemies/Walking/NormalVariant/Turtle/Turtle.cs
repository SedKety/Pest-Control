using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : TankEnemy
{
    public GameObject shell;
    protected override void OnShieldBreak()
    {
        shell.SetActive(false);
    }
}
