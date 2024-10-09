using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
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
    public RenderTexture renderTexture;
    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {

    }

    protected virtual void OnTick()
    {

    }

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
