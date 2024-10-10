using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class IsOtherUIActive : MonoBehaviour
{
    public GameObject uiObject;
    public UIFoldout foldout;
    private void Start()
    {
        foldout = gameObject.transform.parent.gameObject.GetComponent<UIFoldout>();
    }

    void Update()
    {
        if (uiObject.activeInHierarchy)
        {
            foldout.SetState(!uiObject.activeInHierarchy);
        }
    }
}
