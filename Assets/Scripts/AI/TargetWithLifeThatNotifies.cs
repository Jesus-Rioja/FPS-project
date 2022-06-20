using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWithLifeThatNotifies : TargetWithLife
{
    public interface IDeathNotifiable
    {
        public void NotifyDeath();
    }

    protected override void CheckStillAlive()
    {
        if (Life <= 0)
        {
            base.CheckStillAlive();
            GetComponent<IDeathNotifiable>().NotifyDeath();
        }
    }

}
