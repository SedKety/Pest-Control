using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Ratter : MonoBehaviour
{
    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool hasHit = false;
    public bool isRat;
    public bool isCar;
    private void Start()
    {
        originalRot = transform.rotation;
        originalPos = transform.position;
        Return();
    }
    private void Update()
    {
        if (!hasHit) transform.Translate((Vector3.forward * Time.deltaTime) * 15);
        else HasHit();

        if (isRat)
        {
            transform.Rotate(0, 50, 0);
            if (isCar)
            {
                StopRotating();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 3)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20) * 3), ForceMode.Impulse);
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
        isRat = true;
        await Task.Delay(1000);
        hasHit = false;

    }

    async void StopRotating()
    {
        await Task.Delay(1000);
        isRat = false;
        transform.rotation = originalRot;
    }

    void OnApplicationQuit()
    {
        #if UNITY_EDITOR
            var constructor = SynchronizationContext.Current.GetType().GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[] { typeof(int) }, null);
            var newContext = constructor.Invoke(new object[] { Thread.CurrentThread.ManagedThreadId });
            SynchronizationContext.SetSynchronizationContext(newContext as SynchronizationContext);
        #endif
    }
}
