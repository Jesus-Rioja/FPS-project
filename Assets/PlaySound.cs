using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] AudioSource[] Audio;

    public void PlayAudio(int index)
    {
        Audio[index].Play();
    }
}
