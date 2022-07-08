using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject AudioPanel;
    public GameObject VideoPanel;
    public GameObject SettingsPanel;
    public GameObject CreditsPanel;

    [SerializeField] AudioSource OnClickButtonClip;
    [SerializeField] AudioSource OnHoverButtonClip;


    #region Panel Activation
    public void ActivatePanel(string panelToBeActivated)
    {
        MainPanel.SetActive(panelToBeActivated.Equals(MainPanel.name));
        SettingsPanel.SetActive(panelToBeActivated.Equals(SettingsPanel.name));
        AudioPanel.SetActive(panelToBeActivated.Equals(AudioPanel.name));
        VideoPanel.SetActive(panelToBeActivated.Equals(VideoPanel.name));
        CreditsPanel.SetActive(panelToBeActivated.Equals(CreditsPanel.name));
    }

    #endregion 
    
    public void OnClickButtonSound()
    {
        OnClickButtonClip.Play();
    }

    public void OnHoverButtonSound()
    {
        OnHoverButtonClip.Play();
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void UnlockAllWeapons()
    {
        PlayerPrefs.SetInt("Machine gun", 1);
        PlayerPrefs.SetInt("Shotgun", 1);
        PlayerPrefs.SetInt("Grenade launcher", 1);
        PlayerPrefs.SetInt("Flamethrower", 1);
    }

    public void DeleteAllWeapons()
    {
        PlayerPrefs.SetInt("Machine gun", 1);
        PlayerPrefs.SetInt("Shotgun", 0);
        PlayerPrefs.SetInt("Grenade launcher", 0);
        PlayerPrefs.SetInt("Flamethrower", 0);
    }

    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }


}
