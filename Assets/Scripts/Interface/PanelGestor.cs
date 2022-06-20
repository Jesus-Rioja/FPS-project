using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PanelGestor : MonoBehaviour
{
    [SerializeField] GameObject PauseMenuPanel;
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;

    [SerializeField] AudioSource OnClickButtonClip;
    [SerializeField] AudioSource OnHoverButtonClip;

    public bool onMenu = false;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseButton()
    {
        PlayerMovement.instance.movementAllowed = false;
        onMenu = true;
        Time.timeScale = 0f;
        PauseMenuPanel.SetActive(true);
    }

    public void ActivateWinPanel()
    {
        PlayerMovement.instance.movementAllowed = false;
        Time.timeScale = 0f;
        WinPanel.SetActive(true);
    }

    public void ActivateLosePanel()
    {
        PlayerMovement.instance.movementAllowed = false;
        Time.timeScale = 0f;
        LosePanel.SetActive(true);
    }

    public void ResumeButton()
    {
        PlayerMovement.instance.movementAllowed = true;
        onMenu = false;
        Time.timeScale = 1.0f;
        PauseMenuPanel.SetActive(false);
    }

    public void OnClickButtonSound()
    {
        OnClickButtonClip.Play();
    }

    public void OnHoverButtonSound()
    {
        OnHoverButtonClip.Play();
    }
}
