using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class UpdateText : MonoBehaviour
{
    public static TMP_Text[] coinTexts;
    public static TMP_Text nameText;
    public static Image towerImage;
    public static UpdateText instance;
    private void Start()
    {
        instance = this;
    }
    public void UpdatePanel(Tower tower)
    {
        string name = tower.name.Replace("(Clone)", "");
        nameText.text = name;
        FindAnyObjectByType<UIFoldout>().SetState(true);
    }
}
