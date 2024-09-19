using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    void Start()
    {
        Collider[] col = Physics.OverlapBox(transform.position, GetComponent<BoxCollider>().size / 2.5f);
        if (col.Length > 2)
        {
            transform.parent.GetComponent<PathTile>().snapPoints.Remove(gameObject.transform);
            Destroy(gameObject);
        }
    }
}
