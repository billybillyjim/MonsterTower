using UnityEngine;
using System.Collections;

public class Tools : MonoBehaviour{

    public static int currentTool;
    public static float currentToolCost;
    public static int toolWidth;
    private float[] costArray = new float[20];

    public void setTool(int i)
    {
        currentTool = i;
        setToolCost(i);
        setToolWidth(i);
    }
    void Start()
    {
        //Office
        costArray[0] = 100f;
        //Restaurant
        costArray[1] = 100f;
        //Elevator
        costArray[2] = 100f;
        //Dirt
        costArray[3] = 100f;
        //Cafe
        costArray[4] = 100f;
        //Hotel
        costArray[5] = 100f;
        //2BedHotel
        costArray[6] = 100f;
        //Condo
        costArray[7] = 100f;
        //Stairs
        costArray[8] = 100f;
        //Empty
        costArray[9] = 100f;
       
    }

    private void setToolCost(int i)
    {
        currentToolCost = costArray[currentTool];
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
