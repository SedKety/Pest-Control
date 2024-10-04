using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dove : FlyingEnemy
{
    public float initTimeTillEgg;
    public float timeTillEgg;
    public GameObject eggGO;
    public int eggDropChancePercentage;
    public override void Start()
    {
        base.Start();
        StartCoroutine(DropEggs());
    }
    public IEnumerator DropEggs()
    {
        yield return new WaitForSeconds(initTimeTillEgg);
        while (gameObject.activeSelf)
        {
            if (CanDropEgg())
            {
                Instantiate(eggGO, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(timeTillEgg);
            }
            yield return new WaitForSeconds(timeTillEgg);
        }

    }

    private bool CanDropEgg()
    {
        if (eggDropChancePercentage >= Random.Range(0, 100))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
