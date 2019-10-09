using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{

    public int x;
    public int y;
    
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool Compare(Point comparePoint)
    {
        if (x == comparePoint.x && y == comparePoint.y)
        {
            return true;
        }

        return false;
    }
}
