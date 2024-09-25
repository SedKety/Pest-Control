using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LookToCamera : MonoBehaviour
{


    public void ChangeText(string name)
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = name;
    }

    public void TurnOffCanvas()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        transform.rotation = Quaternion.LookRotation((transform.position - Camera.main.transform.position));
    }
}
