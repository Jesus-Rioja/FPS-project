using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pressButton : MonoBehaviour
{
    public string ButtonToPress;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(ButtonToPress))
        {
            Debug.Log("Pulsamos enter");
            this.gameObject.GetComponent<Button>().onClick.Invoke();
        }

    }
}
