using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tools : MonoBehaviour{

    public static int currentTool;
    public static float currentToolCost;
    public static int toolWidth;
    public static int toolHeight;

    private float[] costArray = new float[20];
    private int[] widthArray = new int[20];
    private int[] heightArray = new int[20];

    [SerializeField]
    private Color humanColor;
    [SerializeField]
    private Color zombieColor;
    [SerializeField]
    private Color witchColor;
    [SerializeField]
    private Color demonColor;

    public void setPanelColor(int i)
    {
        if(i == 0)
        {
            gameObject.GetComponent<Image>().color = humanColor;
        }
        else if(i == 1)
        {
            gameObject.GetComponent<Image>().color = zombieColor;
        }
        else if (i == 2)
        {
            gameObject.GetComponent<Image>().color = witchColor;
        }
        else if (i == 3)
        {
            gameObject.GetComponent<Image>().color = demonColor;
        }
        else
        {
            gameObject.GetComponent<Image>().color = Color.white;
        }
        Debug.Log(i);
    }

    public void setTool(int i)
    {
        currentTool = i;
        setToolCost(i);
        setToolWidth(i);
        setToolHeight(i);
    }
    void Start()
    {
        setCosts();
        setWidths();
        setHeights();
        setTool(0);
       
    }
    private void setCosts()
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
        //Suite
        costArray[10] = 1000f;
        //Entertainment
        costArray[11] = 1000f;
    }
    private void setWidths()
    {
        widthArray[0] = 2;
        widthArray[1] = 3;
        widthArray[2] = 1;
        widthArray[3] = 1;
        widthArray[4] = 2;
        widthArray[5] = 1;
        widthArray[6] = 2;
        widthArray[7] = 2;
        widthArray[8] = 1;
        widthArray[9] = 1;
        widthArray[10] = 4;
        widthArray[11] = 3;
    }
    private void setHeights()
    {
        heightArray[0] = 1;
        heightArray[1] = 1;
        heightArray[2] = 1;
        heightArray[3] = 1;
        heightArray[4] = 1;
        heightArray[5] = 1;
        heightArray[6] = 1;
        heightArray[7] = 1;
        heightArray[8] = 1;
        heightArray[9] = 1;
        heightArray[10] = 1;
        heightArray[11] = 2;
    }
    private void setToolCost(int i)
    {
        currentToolCost = costArray[currentTool];
    }
    public void setToolWidth(int i)
    {
        toolWidth = widthArray[i];
    }
    public void setToolHeight(int i)
    {
        toolHeight = heightArray[i];
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
