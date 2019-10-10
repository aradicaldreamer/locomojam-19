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
    private Rigidbody2D rigidBody;          // ReferenrigidBody player's rigidbody

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
      base.Update();
      if (slowDownTimer < Time.fixedTime)
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
    
    void OnCollisionEnter2D(Collision2D hit)
    {
        GameManager.Instance.playerSpeed *= lowerPercentage;
    }
}
