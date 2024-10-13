using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindGameObjectsByLayer : MonoBehaviour
{
    public static GameObject FindFirstGameObjectWithLayer(LayerMask layer)
    {
        var arrayGo = FindObjectsOfType<GameObject>(true);
        var go = arrayGo.First(x => x.layer == layer);
        return go;
    }

    public static GameObject[] FindAllGameObjectsWithLayer(LayerMask layer)
    {
        var arrayGo = FindObjectsOfType<GameObject>(true);
        var go = arrayGo.Where(x => x.layer == layer)
                                    .OrderBy(x => x.name)
                                    .ToArray();
        return go;
    }
}
