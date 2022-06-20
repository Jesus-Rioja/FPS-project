using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKey : MonoBehaviour
{
    public GameObject MainPanel;

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey)
        {
            MainPanel.SetActive(true);
            this.gameObject.SetActive(false);
        }

    }
}
