using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionUnlock : MonoBehaviour
{
    public UnityEvent<string> NewWeaponUnlockedEvent;
    [SerializeField] string WeaponToUnlock;
    AudioSource unlockSound;

    private void Start()
    {
        if(PlayerPrefs.GetInt(WeaponToUnlock, 0) == 1) { Destroy(this.GetComponentInParent<Transform>().gameObject); }
        unlockSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("colisionnn");
            PlayerPrefs.SetInt(WeaponToUnlock, 1);
            NewWeaponUnlockedEvent.Invoke(WeaponToUnlock);
            unlockSound.Play();
            Invoke("InvokeDestroy", 1f);
        }
    }

    void InvokeDestroy()
    {
        Destroy(this.GetComponentInParent<Transform>().gameObject);
    }
}
