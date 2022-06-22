using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeWeaponUI : MonoBehaviour
{
    public void ChangeWeaponImage(int index)
    {
        for(int i = 0; i < transform.childCount-1; i++)
        {
            if(i == index) { transform.GetChild(i).gameObject.SetActive(true); }
            else { transform.GetChild(i).gameObject.SetActive(false); }
        }
    }

}
