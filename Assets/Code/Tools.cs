using UnityEngine;
using System.Collections;

public class Tools : MonoBehaviour{

    public static int currentTool;


    public void setTool(int i)
    {
        currentTool = i;
    }

	public int getCurrentTool()
    {
        return currentTool;
    }
}
