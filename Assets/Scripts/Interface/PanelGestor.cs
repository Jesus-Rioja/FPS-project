using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PanelGestor : MonoBehaviour
{
    public static PanelGestor instance;

    [SerializeField] GameObject PauseMenuPanel;
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;
    [SerializeField] GameObject WeaponUnlockedPanel;
    [SerializeField] GameObject ControlsPanel;
    [SerializeField] TextMeshProUGUI EnemiesLeftText;

    [SerializeField] AudioSource OnClickButtonClip;
    [SerializeField] AudioSource OnHoverButtonClip;
    [SerializeField] AudioSource GameOverMusic;

    [SerializeField] GameObject[] HUDcomponents;
    [SerializeField] Texture2D cursorTexture;

    public bool onMenu = false;
    int enemiesCounter = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayerMovement.instance.GetComponent<TargetWithLifeThatNotifies>().onDeath.AddListener(ActivateLosePanel);
        SetEnemiesCounterText();
        if(PlayerPrefs.GetInt("FirstPlay", 0) == 0) { ActivateControlsPanel(); }

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(int value)
    {
        SceneManager.LoadScene(value);
    }

    public void PauseButton()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerMovement.instance.movementAllowed = false;
        onMenu = true;
        Time.timeScale = 0f;
        DisableHUD();
        PauseMenuPanel.SetActive(true);
    }

    public void ActivateControlsPanel()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerMovement.instance.movementAllowed = false;
        onMenu = true;
        Time.timeScale = 0f;
        DisableHUD();
        ControlsPanel.SetActive(true);
        PauseMenuPanel.SetActive(false);
    }

    public void DisableControlPanelButton()
    {
        PlayerPrefs.SetInt("FirstPlay", 1);
        PlayerMovement.instance.movementAllowed = true;
        onMenu = false;
        Time.timeScale = 1.0f;
        EnableHUD();
        ControlsPanel.SetActive(false);
    }

    public void ActivateWinPanel()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerMovement.instance.movementAllowed = false;
        Time.timeScale = 0f;
        DisableHUD();
        WinPanel.SetActive(true);
        WeaponUnlockedPanel.SetActive(false);
    }

    public void ActivateLosePanel(TargetWithLife target, TargetWithLife.DeathInfo Info)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerMovement.instance.movementAllowed = false;
        Time.timeScale = 0f;
        DisableHUD();
        GameOverMusic.Play();
        LosePanel.SetActive(true);
    }

    public void ResumeButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMovement.instance.movementAllowed = true;
        onMenu = false;
        Time.timeScale = 1.0f;
        EnableHUD();
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

    public void DisableHUD()
    {
        for(int i = 0; i < HUDcomponents.Length; i++)
        {
            HUDcomponents[i].SetActive(false);
        }
    }

    public void EnableHUD()
    {
        for (int i = 0; i < HUDcomponents.Length; i++)
        {
            HUDcomponents[i].SetActive(true);
        }
    }

    public void displayWeaponUnlockedText(string weaponName)
    {
        StartCoroutine(DisplayTextTimer(weaponName, WeaponUnlockedPanel.GetComponentsInChildren<TextMeshProUGUI>()[1]));
    }

    private IEnumerator DisplayTextTimer(string name, TextMeshProUGUI tmp)
    {
        tmp.text = name;
        WeaponUnlockedPanel.SetActive(true);
        yield return new WaitForSeconds(6f);
        WeaponUnlockedPanel.SetActive(false);
    }

    private void SetEnemiesCounterText()
    {
        EnemiesLeftText.text = "Enemies Left: " + enemiesCounter;
    }

    public void ModifyEnemyCount(bool increment)
    {
        if (increment)
            enemiesCounter++;
        else
            enemiesCounter--;

        SetEnemiesCounterText();
    }

}
