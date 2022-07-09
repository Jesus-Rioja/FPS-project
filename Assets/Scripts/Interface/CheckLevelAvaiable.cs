using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CheckLevelAvaiable : MonoBehaviour
{
    Button button;
    EventTrigger eventTrigger;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        eventTrigger = GetComponent<EventTrigger>();

        button.interactable = PlayerPrefs.GetInt("UnlockLevel2", 0) != 0;
        eventTrigger.enabled = PlayerPrefs.GetInt("UnlockLevel2", 0) != 0;

    }
}
