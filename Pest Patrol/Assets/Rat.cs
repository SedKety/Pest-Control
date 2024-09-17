using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Rat : MonoBehaviour
{
    private Vector3 originalPos;
    private bool hasHit;
    private void Start()
    {
        originalPos = transform.position;
        Return();
    }
    private void Update()
    {
        if (!hasHit) transform.Translate((Vector3.forward * Time.deltaTime) * 15);
        else HasHit();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 3)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(10, 20), Random.Range(10, 20), Random.Range(10, 20) * 3), ForceMode.Impulse);
        }
    }

    async void Return()
    {
        await Task.Delay(3000);
        await Task.Delay(Random.Range(0, 3000));
        transform.position = originalPos;
        Return();
    }

    async void HasHit()
    {
        await Task.Delay(1000);
        hasHit = false;
    }
}
