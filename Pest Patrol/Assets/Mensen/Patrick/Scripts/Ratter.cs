using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        originalRot = transform.rotation;
        originalPos = transform.position;
        if (isCar)
        {
            StartCoroutine("Return");
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
            else StartCoroutine("HasHit");
        }

        if (isRat)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime , 0);
            if (isCar)
            {
                StartCoroutine("StopRotating");
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

    public IEnumerator Return()
    {
        yield return new WaitForSeconds(3);
        yield return new WaitForSeconds(Random.Range(0.01f, 3));
        
        transform.position = originalPos;
        transform.rotation = originalRot;
        StartCoroutine("Return");
    }

    public void ChooseTransform()
    {
        transform.position = transforms[Random.Range(0, transforms.Length)].position;
        transform.rotation = transforms[Random.Range(0, transforms.Length)].rotation;
        StartCoroutine("Return");
    }
    public IEnumerator HasHit()
    {
        isRat = true;
        yield return new WaitForSeconds(1);
        hasHit = false;

    }

    public IEnumerator StopRotating()
    {
        yield return new WaitForSeconds(1);
        isRat = false;
        transform.rotation = originalRot;
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
