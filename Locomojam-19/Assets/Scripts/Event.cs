using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour
{

    private float disappearTime;
    private float disappearTimer;

    private void Awake()
    {
        disappearTime = Random.Range(12, 20);
        disappearTimer = Time.time + disappearTime;
    }

    private void Update()
    {
        if (disappearTimer > Time.time)
        {
            GameManager.Instance.RemoveEvent(this);
        }
    }

    public abstract void TriggerEvent();
}
