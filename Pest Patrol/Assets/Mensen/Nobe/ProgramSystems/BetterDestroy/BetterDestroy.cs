using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterDestroy : MonoBehaviour
{
    public static void BDestroy(GameObject objectToDestroy)
    {
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
    }
}