using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private float Timer = 0;
   
    [SerializeField] GameObject Canvas;
    [SerializeField] TMP_Text TimerDisplay;

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Debug.Log("me quiero ir a pause");
            if (!Canvas.GetComponent<PanelGestor>().onMenu)
            {
                Canvas.GetComponent<PanelGestor>().PauseButton();;
            }
            else
            {
                Canvas.GetComponent<PanelGestor>().ResumeButton();
            }
        }


        Timer += Time.deltaTime;
        DisplayTime(Timer);

    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        TimerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
