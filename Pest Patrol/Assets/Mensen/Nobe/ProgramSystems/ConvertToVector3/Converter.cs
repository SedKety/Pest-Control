using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : MonoBehaviour
{
    public static Vector3 ConvertToVector3(Transform transform)
    {
        return transform.localPosition;
    }
}
