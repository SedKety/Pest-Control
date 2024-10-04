using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindGameObjectsByLayer : MonoBehaviour
{
    static public GameObject FindGameObjectWithLayer(int layer)
    {
        var arrayGO = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        var go = arrayGO.First(x => x.layer == layer);
        return go;
    }
}
