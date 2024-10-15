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
    public bool isPigeon;
    public float rotationSpeed;
    public Transform[] transforms;
    private void Start()
    {
        originalRot = transform.rotation;
        originalPos = transform.position;
        if (isCar)
        {
            Return();
        }
    }
    private void Update()
    {
        if (isCar)
        {
            if (!hasHit)
            {
                transform.Translate(Vector3.forward * (Time.deltaTime * 30));
                if (isPigeon) transform.Rotate(0, Random.Range(-0.1f, 0.1f), 0);
            }
            else HasHit();
        }

        if (isRat)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime , 0);
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
        if (!gameObject) { return; }
        await Task.Delay(Random.Range(0, 3000));
        if (!gameObject) { return; }

        if (isPigeon)
        {
            ChooseTransform();
            return;
        }
        transform.position = originalPos;
        transform.rotation = originalRot;
        Return();
    }

    public void ChooseTransform()
    {
        transform.position = transforms[Random.Range(0, transforms.Length)].position;
        transform.rotation = transforms[Random.Range(0, transforms.Length)].rotation;
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
    #region async bs
    void OnApplicationQuit()
    {
        #if UNITY_EDITOR
            var constructor = SynchronizationContext.Current.GetType().GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[] { typeof(int) }, null);
            var newContext = constructor.Invoke(new object[] { Thread.CurrentThread.ManagedThreadId });
            SynchronizationContext.SetSynchronizationContext(newContext as SynchronizationContext);
        #endif
    }
    #endregion
}
