using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnEnableVideoManager : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropScreen, dropQuality, dropResolution;

    private void OnEnable() 
    {
        dropQuality.value = PlayerPrefs.GetInt("Quality", 2);
        dropScreen.value = PlayerPrefs.GetInt("Screen", 1);
        dropResolution.value = PlayerPrefs.GetInt("Resolution", 3);
    }
}
