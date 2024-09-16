using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    public float timeToDestroy;
    public void Start()
    {
        StartCoroutine(DestroyObject());
    }
    public IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
}
