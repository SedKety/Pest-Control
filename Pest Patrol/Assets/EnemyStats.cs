using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public TMP_Text previewName;
    public TMP_Text type;
    public TMP_Text damage;
    public TMP_Text health;
    public TMP_Text speed;
    public TMP_Text description;
    public Image image;
    private UIFoldout foldout;

    private void Start()
    {
        foldout = GetComponent<UIFoldout>();
    }

    public void UpdatePanel(Enemy enemy)
    {
        foldout.SetState(true);
        var enemyName = enemy.name.Replace("(Clone)", string.Empty);
        previewName.text = enemyName;
        type.text = enemy.type.ToString();
        damage.text = enemy.damage.ToString();
        health.text = enemy.health.ToString();
        speed.text = enemy.moveSpeed.ToString();
        description.text = enemy.description;
        image.sprite = enemy.sprite;
    }
}
