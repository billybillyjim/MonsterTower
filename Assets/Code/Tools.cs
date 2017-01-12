using UnityEngine;
using System.Collections;

public class Tools : MonoBehaviour{

    public static int currentTool;
    public static float currentToolCost;
    public static int toolWidth;

    public void setTool(int i)
    {
        currentTool = i;
        setToolCost(i);
        setToolWidth(i);
    }

    private void setToolCost(int i)
    {
        if(i == 0)
        {
            currentToolCost = 100f;
        }
        else if(i == 4)
        {
            currentToolCost = 200f;
        }
    }
    public void setToolWidth(int i)
    {
        if(i == 0 || i == 4 || i == 6 || i == 7){
            toolWidth = 2;
        }
        else if(i == 2 || i == 3 || i == 5 || i == 9)
        {
            toolWidth = 1;
        }
        else if(i == 1)
        {
            toolWidth = 3;
        }
    }

	public int getCurrentTool()
    {
        return currentTool;
    }
    public int getToolWidth()
    {
        return toolWidth;
    }
}
