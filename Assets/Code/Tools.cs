using UnityEngine;
using System.Collections;

public class Tools : MonoBehaviour{

    public static int currentTool;
    public static float currentToolCost;

    public void setTool(int i)
    {
        currentTool = i;
        setToolCost(i);
    }

    private void setToolCost(int i)
    {
        if(i == 1)
        {
            currentToolCost = 100f;
        }
        else if(i == 2)
        {
            currentToolCost = 200f;
        }
    }

	public int getCurrentTool()
    {
        return currentTool;
    }
}
