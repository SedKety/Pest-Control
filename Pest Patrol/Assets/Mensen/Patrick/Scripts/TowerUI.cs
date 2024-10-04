using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUI : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);
            if (hit.collider.GetComponent<Tower>() != null)
            {
                if (hit.collider.GetComponent<Tower>().interactable)
                {
                    string name = hit.collider.GetComponent<Tower>().name;
                    name = name.Replace("(Clone)", "");
                    UpdateText.instance.UpdatePanel(hit.collider.GetComponent<Tower>());
                }
            }
        }
    }
}
