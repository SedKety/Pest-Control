using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class IsOtherUIActive : MonoBehaviour
{
    public GameObject uiObject;
    public UIFoldout foldout;

    void Update()
    {
        if (uiObject.activeInHierarchy)
        {
            foldout.SetState(!foldout.isOn);
            
        }
    }
}
