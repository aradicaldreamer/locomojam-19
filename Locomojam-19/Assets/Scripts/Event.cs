using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour
{

    private float disappearTime;
    private float disappearTimer;
    private Point location;
    protected bool eventTriggered;

    private void Awake()
    {
        disappearTime = Random.Range(12, 20);
        disappearTimer = Time.time + disappearTime;
        eventTriggered = false;
    }

    protected void Update()
    {
        if (disappearTimer < Time.time && !eventTriggered)
        {
            EndEvent();
            GameManager.Instance.RemoveEvent(this);
        }
    }

    protected abstract void RunEvent();

    protected abstract void EndEvent();

    public void TriggerEvent()
    {
        if (!eventTriggered)
        {
            RunEvent();
            eventTriggered = true;
        }
    }

    public void SetLocation(Point newLocation)
    {
        location = new Point(newLocation.x, newLocation.y);
    }

    public Point GetLocation()
    {
        return location;
    }
}
