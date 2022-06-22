using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWithLifeThatNotifies : TargetWithLife
{
    AudioSource damageSound;

    private void Awake()
    {
        damageSound = GetComponent<AudioSource>();
    }

    public interface IDeathNotifiable
    {
        public void NotifyDeath();
    }

    protected override void CheckStillAlive()
    {
        if (damageSound != null/* && !damageSound.isPlaying*/) { damageSound.Play(); }

        if (Life <= 0)
        {
            base.CheckStillAlive();
            GetComponent<IDeathNotifiable>().NotifyDeath();
        }
    }

}
