using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePosition : MonoBehaviour
{
    public Vector3 movementVector;
    public float deathTimer;
    public void Start()
    {
        StartCoroutine(DieAfterTime());
    }
    void Update()
    {
        transform.Translate(movementVector * Time.deltaTime);
    }
    private IEnumerator DieAfterTime()
    {
        yield return new WaitForSeconds(deathTimer);
        Destroy(gameObject);
    }
}
