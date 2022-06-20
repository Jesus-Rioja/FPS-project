using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeWeaponUI : MonoBehaviour
{
    public void ChangeWeaponImage(int index)
    {
        Debug.Log("Quiero cambiar sprite");
        for(int i = 0; i < transform.childCount; i++)
        {
            if(i == index) { transform.GetChild(i).gameObject.SetActive(true); }
            else { transform.GetChild(i).gameObject.SetActive(false); }
        }
    }

}
