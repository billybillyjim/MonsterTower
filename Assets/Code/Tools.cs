using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tools : MonoBehaviour{

    public static int currentTool;
    public static float currentToolCost;
    public static int toolWidth;
    public static int toolHeight;
    public static int toolMinPop;
    public static int toolMaxPop;

    [SerializeField]
    private Sprite selectedButtonSprite;
    [SerializeField]
    private Sprite buttonSprite;

    private Transform currentButton;

    private float[] costArray = new float[80];
    private int[] widthArray = new int[80];
    private int[] heightArray = new int[80];
    private string[] nameArray = new string[80];
    private int[] minPopArray = new int[80];
    private int[] maxPopArray = new int[80];

    [SerializeField]
    private Transform[] buttonArray = new Transform[20];

    [SerializeField]
    private Color humanColor;
    [SerializeField]
    private Color zombieColor;
    [SerializeField]
    private Color witchColor;
    [SerializeField]
    private Color demonColor;
    private int currentPanel;
    private int numOfRoomTypes;

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
        currentPanel = i;
        
        setTool(currentTool);
    }

    public void setTool(int c)
    {
        while(c >= numOfRoomTypes)
        {
            c -= numOfRoomTypes;
        }
        int i = c + (currentPanel * (numOfRoomTypes));
        currentTool = i;
        setToolCost(i);
        setToolWidth(i);
        setToolHeight(i);
        
    }
    void Start()
    {
        
        loadData();
        loadButtons();
        setTool(0);

    }
    private void loadData()
    {
        string file = System.IO.File.ReadAllText("Assets/Resources/BuildingData.txt");
        string[] lines = file.Split("\n"[0]);
       
        
        for(int j = 0; j < lines.Length; j++)
        {
            string[] lineData = (lines[j].Trim()).Split(","[0]);        
               
            nameArray[j] = lineData[0];
            costArray[j] = float.Parse(lineData[1]);
            widthArray[j] = int.Parse(lineData[2]);            
            heightArray[j] = int.Parse(lineData[3]);
            minPopArray[j] = int.Parse(lineData[4]);
            maxPopArray[j] = int.Parse(lineData[5]);

                numOfRoomTypes++;
            
        }
        numOfRoomTypes /= 4;
    }
    private void loadButtons()
    {
        int i = 0;
        foreach (Transform t in gameObject.GetComponentInChildren<Transform>())
        {
            if(t.parent == this.transform)
            {
                buttonArray[i] = t;
                i++;
            }
        }
    }
    public void setButtonAsSelected(int i)
    {
        if(currentButton != null)
        {
            currentButton.GetComponent<Image>().sprite = buttonSprite;
        }
        currentButton = buttonArray[i];
        currentButton.GetComponent<Image>().sprite = selectedButtonSprite;

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
    public int getPopToMoveIn(int i)
    {
        return Random.Range(minPopArray[i], maxPopArray[i]);
    }
    public void setToolAsSelected(int i)
    {

    }
    private int convertRoomToButton(int i)
    {
        return 0;
    }
}
