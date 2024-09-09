using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Tower : MonoBehaviour
{
    public GameObject[] enemies;
    void Shoot(int damage)
    {
        foreach (var enemy in enemies)
        {
            //if (enemy.distance < hitBox) {
            //  enemy.health -= damage;
            //}
        }
    }
}
