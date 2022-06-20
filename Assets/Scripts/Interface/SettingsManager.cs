using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider sliderMaster, sliderMusic, sliderEffects;
    public TMPro.TMP_Dropdown ResolutionsDropdown;
    private Resolution[] Resolutions;

    void Awake()
    {
        getResolutions();
        InitSettings();
    }

    private void InitSettings()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 80f));
        SetEffectsVolume(PlayerPrefs.GetFloat("EffectsVolume", 80f));
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 80f));

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", 2));
        SetScreen(PlayerPrefs.GetInt("Screen", 1));
        SetResolution(PlayerPrefs.GetInt("Resolution", 3));

    }

    public void SetMasterVolume(float percentage)
    {
        float db = 20 * Mathf.Log10(percentage / 100);
        //Debug.Log("Master");
        masterMixer.SetFloat("MasterParam", db);
        PlayerPrefs.SetFloat("MasterVolume", percentage);
        PlayerPrefs.Save();
    }

    public void SetEffectsVolume(float percentage)
    {
        float db = 20 * Mathf.Log10(percentage / 100);
        //Debug.Log("Efectos");
        masterMixer.SetFloat("EffectsParam", db);
        PlayerPrefs.SetFloat("EffectsVolume", percentage);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float percentage)
    {
        float db = 20 * Mathf.Log10(percentage / 100);
        //Debug.Log("Music");
        masterMixer.SetFloat("MusicParam", db);
        PlayerPrefs.SetFloat("MusicVolume", percentage);
        PlayerPrefs.Save();
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
        PlayerPrefs.Save();
    }

    public void SetScreen (int screenIndex)
    {
        switch (screenIndex)
        {
            case 0:
                Screen.fullScreen = false;
                break;

            case 1:
                Screen.fullScreen = true;
                break;

            default:
                Screen.fullScreen = false; //PONER A TRUE EN LA BUIILD FINAL
                break;
        }

        PlayerPrefs.SetInt("Screen", screenIndex);
        PlayerPrefs.Save();
        
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = Resolutions[(Resolutions.Length-1) - resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        PlayerPrefs.SetInt("Resolution", resolutionIndex);
        PlayerPrefs.Save();
    }

    private void getResolutions()
    {
        Resolutions = Screen.resolutions;

        ResolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        string option = Resolutions[Resolutions.Length-1].width + " x " + Resolutions[Resolutions.Length-1].height;
        options.Add(option);

        for(int i = Resolutions.Length-2; i >= 0 ; i--)
        {
            if(Resolutions[i].width != Resolutions[i+1].width || Resolutions[i].height != Resolutions[i+1].height)
            {
                option = Resolutions[i].width + " x " + Resolutions[i].height;
                options.Add(option);
            }
            
        }

        ResolutionsDropdown.AddOptions(options);
        ResolutionsDropdown.RefreshShownValue();
    }
}
