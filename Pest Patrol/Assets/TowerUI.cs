using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUI : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("clicked mouse 0");
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);

            if (hit.collider.GetComponent<Tower>() != null)
            {
                string name = hit.collider.GetComponent<Tower>().name;
                name = name.Replace("(Clone)", "");
                hit.collider.GetComponent<Tower>().canvas.GetComponent<LookToCamera>().ChangeText(name);
                hit.collider.GetComponent<Tower>().canvas.SetActive(!hit.collider.GetComponent<Tower>().canvas.activeSelf);
            }
        }
    }
}
