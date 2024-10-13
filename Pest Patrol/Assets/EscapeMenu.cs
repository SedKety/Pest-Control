using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    public GameObject[] uiObjects;
    public LayerMask layer;
    public GameObject[] activeUI;
    //TODO: make this not broken lmao, uiObjects always returns 0 even though I dont see any issues with the expressions and it should return all the UI objects.
    private void Start()
    {
        uiObjects = FindObjectsOfType<GameObject>(true).Where(x => x.layer == (int)layer.value).ToArray();
        activeUI = uiObjects.Where(x => x.activeInHierarchy).ToArray();
    }
    
    public void TurnOffUI()
    {
        var uiToTurnOff = activeUI.Where(x => x != gameObject || x.transform.IsChildOf(gameObject.transform)).ToArray();
        foreach (var ui in uiToTurnOff)
        {
            ui.SetActive(false);
            print(ui.name);
        } 
    }
    
    public void TurnOnUI()
    {
        var uiToTurnOn = activeUI.Where(x => x != gameObject || x.transform.IsChildOf(gameObject.transform)).ToArray();
        foreach (var ui in uiToTurnOn) ui.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    
    public void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        Time.timeScale = 0;
        TurnOffUI();
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        TurnOnUI();
        Time.timeScale = 1;
    }

    public void SettingsButton(GameObject settingsMenu)
    {
        settingsMenu.SetActive(true);
        GameObject button = EventSystem.current.currentSelectedGameObject;
        button.transform.parent.gameObject.SetActive(false);
    }

    public void QuitButton()
    {
        SceneManager.LoadScene("Patrick");
    }
}
