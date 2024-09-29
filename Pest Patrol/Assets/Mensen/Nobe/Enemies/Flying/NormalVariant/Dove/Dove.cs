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
        print("StartSpawningEggs");
        while (gameObject.activeSelf)
        {
            print("Tries to drop egg");
            if (CanDropEgg())
            {
                print("Succesfully Dropped an egg");
                Instantiate(eggGO, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(timeTillEgg);
            }
            print("Failed, will retry in " + timeTillEgg + "seconds");
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
