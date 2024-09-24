using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGameObjectsByLayer : MonoBehaviour
{
    static public GameObject[] FindGameObjectsWithLayer(int layer)
    {
        var arrayGO = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        var listGO = new List<GameObject>();
        for (int i = 0; i < arrayGO.Length; i++)
        {
            if (arrayGO[i].layer == layer) { listGO.Add(arrayGO[i]); }
        }
        if (listGO.Count == 0) { return null; }
        return listGO.ToArray();
    }
}
