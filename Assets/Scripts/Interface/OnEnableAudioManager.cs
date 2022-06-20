using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OnEnableAudioManager : MonoBehaviour
{
    public Slider sliderMaster, sliderMusic, sliderEffects;

    private void OnEnable() 
    {
        sliderMaster.value = PlayerPrefs.GetFloat("MasterVolume", 80f);
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 80f);
        sliderEffects.value = PlayerPrefs.GetFloat("EffectsVolume", 80f);
    }
}
