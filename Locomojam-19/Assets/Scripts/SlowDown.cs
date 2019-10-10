using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : Event
{

    [SerializeField]
    private float slowDownTime = 2;
    private float slowDownTimer = 0;
    private float speedToRestore = 0;
    [SerializeField]
    private float lowerPercentage = .05f;

    protected void Update()
    {
      base.Update();
      if (slowDownTimer < Time.time)
        {
            EndEvent();
        }  
    }

    protected override void RunEvent()
    {
        speedToRestore = GameManager.Instance.LowerSpeed(lowerPercentage);
        slowDownTimer = Time.time + slowDownTime;
    }

    protected override void EndEvent()
    {
        GameManager.Instance.FixSpeed(speedToRestore);
        speedToRestore = 0;//reset speedtorestore
    }
}
