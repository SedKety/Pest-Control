using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum TargettingType
{
    ground,
    walking,
    all,
    none,
}

public enum TowerType
{
    generating,
    fighting,
}
public abstract class Tower : MonoBehaviour
{
    public TargettingType typeToTarget;
    public TowerType typeOfTower;
    public bool canShoot = true;
    public GameObject canvas;

    protected abstract void Start();
    protected abstract void Update();

    protected abstract void OnTick();
    public async Task StunTower(float timeInSeconds)
    {
        canShoot = false;
        var middleMan = timeInSeconds * 1000;
        print((int)middleMan);
        await Task.Delay((int)middleMan);
        canShoot = true;
    }
}
