using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterAnimator : MonoBehaviour
{
    Animator animator;
    Vector3 oldPosition = Vector3.zero;
    //AudioSource[] audioSources;
    private void Awake()
    {
        oldPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
        //audioSources = GetComponents<AudioSource>();
    }
    Vector3 smoothedVelocity = Vector3.zero;
    void Update()
    {
        Vector3 currentWorldVelocity = (transform.position - oldPosition) / Time.deltaTime;
        Vector3 currentLocalVelocity = transform.InverseTransformDirection(currentWorldVelocity);

        if (currentLocalVelocity.magnitude > 1f)
        {
            /*if (!audioSources[1].isPlaying)
            {
                audioSources[1].pitch = Random.Range(0.7f, 1f);
                audioSources[1].volume = Random.Range(0.4f, 0.7f);
                audioSources[1].Play();
            }*/

            animator.SetFloat("ForwardVelocity", currentLocalVelocity.z);
            animator.SetFloat("HorizontalVelocity", currentLocalVelocity.x);
        }
        else
        {
            //audioSources[1].Stop();
            animator.SetFloat("ForwardVelocity", 0f);
            animator.SetFloat("HorizontalVelocity", 0f);
        }
        oldPosition = transform.position;
    }
}