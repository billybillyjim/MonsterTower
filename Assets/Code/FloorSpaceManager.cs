using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloorSpaceManager{

    //Converts world position coordinates to a floor level. Floors above ground start at 1, below start at -1, so the rules change at ground level.
	public static int convertPositionToFloor(float f)
    {
        int i = 0;
        float k = 0f;

        if (f < 40)
        {
            k = (f) - 41f;
            k = Mathf.Round(k);
            i = (int)k;
        }
        else
        {
            k = (f) - 40f;
            k = Mathf.Round(k);
            i = (int)k;
        }
        return i;
    }

    public static float convertFloorToPosition(int i)
    {
        float f = 0;

        if(i == 1)
        {
            f = 41f;
        }
        else  if(i > 0)
        {
            f = (float)i + 40f;
        }
        else
        {
            f = (float)i + 41f;
        }

        return f;
    }
    public static int convertPositionToFloorIfEqual(float f)
    {
        int i = (int)f;
        float k = f;
        Debug.Log(i + ", " + k);
        if(Mathf.Approximately(i, k))
        {
            if (f < 40)
            {
                k = (f) - 41f;
                k = Mathf.Round(k);
                i = (int)k;
            }
            else
            {
                k = (f) - 40f;
                k = Mathf.Round(k);
                i = (int)k;
            }
            return i;
        }
        else
        {
            return 0;
        }
    }
}
