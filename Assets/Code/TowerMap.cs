using UnityEngine;
using System.Collections.Generic;

public class TowerMap : MonoBehaviour {

    [SerializeField]
    private int towerWidth;
    [SerializeField]
    private int[,] desireChart = new int[50,50];
    [SerializeField]
    private int towerHeight;
    private float sizeRatio;
    public GameObject building;
    [SerializeField]
    private Building[,] towerMap;
    [SerializeField]
    private List<Sprite> buildingSpriteList = new List<Sprite>();

	// Use this for initialization
	void Start () {
        towerHeight = 200;
        towerWidth = 100;
        sizeRatio = .5f;
	    towerMap = new Building[towerWidth,towerHeight];

        loadSpriteList();
        loadDesireChart();
        createTower();
    }

	//Makes the whole tower
    private void createTower()
    {
        for(int i = 0; i < towerWidth; i++)
        {
            for(int j = 0; j < towerHeight; j++)
            {
                          
                GameObject t = (GameObject)Instantiate(building, new Vector3(i * sizeRatio, j * sizeRatio), Quaternion.identity);
                GameObject b = t.gameObject;
                towerMap[i, j] = b.GetComponent<Building>();
                if (j < 3)
                {
                    b.GetComponent<Building>().setSprite(buildingSpriteList[3]);
                    b.GetComponent<Building>().setIsOccupied(true);
                }
                b.GetComponent<Building>().Init(i, j);
            }
        }
    }
    //Loads Sprites
    //TODO: Make it not suck
    private void loadSpriteList()
    {
        
        Sprite[] e = Resources.LoadAll<Sprite>("Buildings/Offices");
        Sprite l = Resources.Load("Buildings/Lobby", typeof(Sprite)) as Sprite;
        Sprite p = Resources.Load("Buildings/EmptyHalf", typeof(Sprite)) as Sprite;
        Sprite d = Resources.Load("Buildings/Dirt", typeof(Sprite)) as Sprite;
        Sprite r = Resources.Load("Buildings/Restaurant", typeof(Sprite)) as Sprite;
        
        buildingSpriteList.AddRange(e);
        //#3
        buildingSpriteList.Add(d);
        //#4
        buildingSpriteList.Add(r);
        //#5
        buildingSpriteList.Add(l);
        
        buildingSpriteList.Add(p);        

    }

    //Builds at given coordinates using the current tool.
    public void build(int x, int y)
    {
        if(checkIfBuildable(x,y) && GameRun.cash >= Tools.currentToolCost){
            if(Tools.currentTool == 0)
            {
                towerMap[x, y].setSprite(buildingSpriteList[Random.Range(0,3)], Tools.currentTool);
            }
            else
            {
                towerMap[x, y].setSprite(buildingSpriteList[Tools.currentTool], Tools.currentTool);
            }            
            
            GameRun.chargeMoney(Tools.currentToolCost);
            occupy(x, y);
            setDesirability(towerMap[x, y]);
        }       
    }
    //Checks to see if you can build somewhere based on the current tool.
    //TODO:Make it simple
    private bool checkIfBuildable(int x, int y)
    {
        if(Tools.currentTool == 0 || Tools.currentTool == 4)
        {
            if(x != towerWidth && 
                !towerMap[x + 1, y].getIsOccupied() &&
                !towerMap[x, y].getIsOccupied() &&
                towerMap[x, y - 1].getIsOccupied() &&
                towerMap[x + 1, y - 1].getIsOccupied())
            {
                return true;
            }
        }
        else if(Tools.currentTool == 1)
        {
            if(!towerMap[x,y].getIsOccupied() &&
                towerMap[x, y - 1].getIsOccupied())
            {
                return true;
            }
        }

        return false;
    }

    //Fills in used spaces.
    private void occupy(int x, int y)
    {
        if(Tools.currentTool == 0 || Tools.currentTool == 4)
        {
            towerMap[x, y].setIsOccupied(true);
            towerMap[x + 1, y].setIsOccupied(true);
        }
        else if(Tools.currentTool == 1)
        {
            towerMap[x, y].setIsOccupied(true);
        }
    }

    private void setDesirability(Building b)
    {
        int x;
        int y;
        b.getCoordinates(out x, out y);

        float desire = 0;

        Building[,] neighbors = getNeighbors(x, y);
        foreach(Building building in neighbors)
        {
            if(building != null)
            {
                desire += desireChart[b.getBuildingType(), building.getBuildingType()];
            }
        }
        b.setDesirability(desire);
        Debug.Log(desire);
    }

    public Building[,] getTowerMap()
    {
        return towerMap;
    }

    //TODO:Make not suck
    public Building[,] getNeighbors(int x, int y)
    {
        Building[,] neighbors = new Building[7,7];

        neighbors[3, 3] = towerMap[x, y];
        for(int i = -3; i < 4; i++)
        {
            for(int j = -3; j < 4; j++)
            {   
                if(towerMap[x + i, x + j] != null && (i + j) >= -3 && (i + j <= 2))
                {
                    neighbors[3 + i, 3 + j] = towerMap[x + i, y + j];
                   // Debug.Log((3 + i) + ", " + (3 + j) + ":" + (x + i) + ", " + (y + j));
                }
                
            }
        }
        /*
        neighbors[2, 3] = towerMap[x - 1, y];
        neighbors[3, 2] = towerMap[x, y - 1];
        neighbors[4, 3] = towerMap[x + 1, y];
        neighbors[3, 4] = towerMap[x, y + 1];

        neighbors[3, 1] = towerMap[x, y - 2];
        neighbors[4, 2] = towerMap[x + 1, y - 1];
        neighbors[5, 3] = towerMap[x + 2, y];
        neighbors[4, 4] = towerMap[x + 1, y + 1];
        neighbors[3, 5] = towerMap[x, y + 2];
        neighbors[2, 4] = towerMap[x - 1, y + 1];
        neighbors[1, 3] = towerMap[x - 2, y];
        neighbors[2, 2] = towerMap[x - 1, y - 1];

        neighbors[3, 0] = towerMap[x, y - 3];
        neighbors[2, 1] = towerMap[x - 1, y - 2];
        neighbors[1, 2] = towerMap[x - 2, y - 1];
        neighbors[0, 3] = towerMap[x - 3, y];
        neighbors[1, 4] = towerMap[x - 2, y + 1];
        neighbors[2, 5] = towerMap[x - 1, y + 2];
        neighbors[3, 6] = towerMap[x, y + 3];
        neighbors[4, 5] = towerMap[x + 1, y + 2];
        neighbors[5, 4] = towerMap[x + 2, y + 1];
        neighbors[6, 3] = towerMap[x + 3, y];
        neighbors[5, 2] = towerMap[x + 2, y - 1];
        neighbors[4, 1] = towerMap[x + 1, y - 2];
        */

        return neighbors;
    }
    private void loadDesireChart()
    {
        string file = System.IO.File.ReadAllText("Assets/Resources/BuildingDesireChart.csv");
        string[] lines = file.Split("\n"[0]);
        string[] lineData = (lines[0].Trim()).Split(","[0]);
        for(int i = 0; i < lines.Length; i++)
        {
            for(int j = 0; j < lineData.Length; j++)
            {
                desireChart[i, j] = int.Parse(lineData[j]);
            }
        }
    }
    
}
