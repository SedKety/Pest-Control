using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static Unity.VisualScripting.Member;

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
    public EnemyType typeToTarget;
    public TowerType typeOfTower;
    public bool canShoot = true;
    public bool interactable = false;
    public GameObject canvas;

    protected abstract void Start();
    protected abstract void Update();

    protected abstract void OnTick();

    public virtual void OnDestroy()
    {
        Ticker.OnTickAction -= OnTick;
    }

    public async void MakeInteractable()
    {
        await Task.Delay(300);
        interactable = true;
    }
}
