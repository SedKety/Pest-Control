using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dove : FlyingEnemy
{
    public float initTimeTillEgg;
    public float timeTillEgg;

    public override void Start()
    {
        base.Start();
        //StartCoroutine(DropEggs());
    }
    public void DropEgg()
    {

    }

    public IEnumerator DropEggs()
    {
        yield return new WaitForSeconds(initTimeTillEgg);

    }
}
