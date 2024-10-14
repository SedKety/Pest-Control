using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class EscapeMenu : MonoBehaviour
{
    public delegate void ToggleUI(bool state);
    public static event ToggleUI OnEscape;
    public GameObject[] uiObjects;
    public LayerMask layer;
    public GameObject[] activeUI;
    private void Start()
    {
        activeUI = uiObjects.Where(x => x.activeInHierarchy).ToArray();
        OnEscape += SetUIActive;
    }
    
    public void SetUIActive(bool state)
    {
        foreach (var ui in activeUI) ui.SetActive(state);
        gameObject.transform.GetChild(0).gameObject.SetActive(!state);
    }
    
    public void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        Time.timeScale = 0;
        OnEscape?.Invoke(false);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        OnEscape?.Invoke(true);
        Time.timeScale = 1;
    }

    public void SettingsButton(GameObject settingsMenu)
    {
        settingsMenu.SetActive(true);
        var button = EventSystem.current.currentSelectedGameObject;
        button.transform.parent.gameObject.SetActive(false);
    }

    public void QuitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
