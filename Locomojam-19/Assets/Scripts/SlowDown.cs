using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : Event
{

    [SerializeField]
    private float slowDownTime = 2;
    private float slowDownTimer = 0;
    private bool triggered = false;
    private float speedToRestore;
    [SerializeField]
    private float lowerPercentage = 0.05f;

    private void Update()
    {
      if (triggered && slowDownTimer < Time.time)
        {
            GameManager.Instance.FixSpeed(speedToRestore);
            Destroy(this);
        }  
    }

    public override void TriggerEvent()
    {
        speedToRestore = GameManager.Instance.LowerSpeed(lowerPercentage);
        slowDownTimer = Time.time + slowDownTime;
        triggered = true;
    }
}
