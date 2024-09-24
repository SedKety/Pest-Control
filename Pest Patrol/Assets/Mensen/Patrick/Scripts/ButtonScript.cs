using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public GameObject mainMenu, settings, credits;

    public void Quit()
    {
        Application.Quit();
    }
    public void Settings()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void SwitchScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }
    public void CreditsBack()
    {
        credits.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void SettingsBack()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
    }
}
