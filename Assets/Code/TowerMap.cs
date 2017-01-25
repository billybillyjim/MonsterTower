using UnityEngine;
using System.Collections.Generic;

public class TowerMap : MonoBehaviour {

    [SerializeField]
    private int towerWidth;
    [SerializeField]
    private float[,] desireChart = new float[50,50];
    [SerializeField]
    private int towerHeight;
    private float sizeRatio;
    public GameObject building;
    [SerializeField]
    private Building[,] towerMap;
    //[SerializeField]
    //private List<Sprite> buildingSpriteList = new List<Sprite>();
    [SerializeField]
    private List<List<Sprite>> buildingSpritesList = new List<List<Sprite>>();
    [SerializeField]
    private List<Building> elevatorList = new List<Building>();
    public GameObject witch;
    [SerializeField]
    private int totalPopulation;
    private enum type { Office, Restaurant, Elevator, Dirt, Cafe, Hotel, HotelTwoBed, Condo, Stairs, Empty };

    // Use this for initialization
    void Start () {
        towerHeight = 140;
        towerWidth = 100;
        sizeRatio = .5f;
	    towerMap = new Building[towerWidth,towerHeight];
        
        loadSpriteList();
        loadDesireChart();
        createTower();
        //InvokeRepeating("SpawnWitch", 10, 1);
        
    }
    private void SpawnWitch()
    {
        GameObject w = (GameObject)Instantiate(witch, new Vector3(2, 20, -2), Quaternion.identity);
        GameObject g = w.gameObject;
        
        g.GetComponent<Character>().Init(towerMap[6, 41]);
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
                if (j == 40)
                {
                    b.GetComponent<Building>().setSprite(buildingSpritesList[3][0]);
                    

                }
                if (j > 15 && j < 40)
                {
                    b.GetComponent<Building>().setSprite(buildingSpritesList[3][1]);
                    
                }
                if (j == 15)
                {
                    b.GetComponent<Building>().setSprite(buildingSpritesList[3][2]);
                    
                }
                if (j < 15)
                {
                    b.GetComponent<Building>().setSprite(buildingSpritesList[3][3]);
                    
                }
                b.GetComponent<Building>().Init(i, j);
                if(j > 40)
                {
                    b.GetComponent<Building>().setFloor(j - 40);
                }
                else
                {
                    b.GetComponent<Building>().setFloor(j - 41);
                    b.GetComponent<Building>().setDirt(true);
                }
            }
        }
    }
    //Loads Sprites
    //TODO: Make it not suck
    private void loadSpriteList()
    {

        List<Sprite> allSprites = new List<Sprite>();
        List<Sprite> officeSprites = new List<Sprite>();
        List<Sprite> restaurantSprites = new List<Sprite>();
        List<Sprite> hotelSprites = new List<Sprite>();
        List<Sprite> hotel2BedSprites = new List<Sprite>();
        List<Sprite> condoSprites = new List<Sprite>();
        List<Sprite> stairSprites = new List<Sprite>();
        List<Sprite> cafeSprites = new List<Sprite>();
        List<Sprite> dirtSprites = new List<Sprite>();
        List<Sprite> elevatorSprites = new List<Sprite>();
        List<Sprite> emptySprites = new List<Sprite>();
        List<Sprite> hotelSuiteSprites = new List<Sprite>();
        List<Sprite> entertainmentSprites = new List<Sprite>();

        allSprites.AddRange(Resources.LoadAll<Sprite>("Buildings"));

        foreach (Sprite s in allSprites)
        {
            char n = s.name[0];
            if (n == 'o')
            {
                officeSprites.Add(s);
            }
            else if(n == 'r')
            {
                restaurantSprites.Add(s);
            }
            else if (n == 'w')
            {
                cafeSprites.Add(s);
            }
            else if (n == 'd')
            {
                dirtSprites.Add(s);
            }
            else if(n == 'e')
            {
                elevatorSprites.Add(s);
            }
            else if (n == 'c')
            {
                condoSprites.Add(s);
            }
            else if (n == 'h')
            {
                hotelSprites.Add(s);
            }
            else if (n == 't')
            {
                hotel2BedSprites.Add(s);
            }
            else if (n == 's')
            {
                stairSprites.Add(s);
            }
            else if (n == '1')
            {
                emptySprites.Add(s);
            }
            else if (n == 'q')
            {
                hotelSuiteSprites.Add(s);
            }
            else if(n == 'E')
            {
                entertainmentSprites.Add(s);
            }

        }
        //0
        buildingSpritesList.Add(officeSprites);
        //1
        buildingSpritesList.Add(restaurantSprites);
        //2
        buildingSpritesList.Add(elevatorSprites);
        //3
        buildingSpritesList.Add(dirtSprites);
        //4
        buildingSpritesList.Add(cafeSprites);
        //5
        buildingSpritesList.Add(hotelSprites);
        //6
        buildingSpritesList.Add(hotel2BedSprites);
        //7
        buildingSpritesList.Add(condoSprites);
        //8
        buildingSpritesList.Add(stairSprites);
        //9
        buildingSpritesList.Add(emptySprites);
        //10
        buildingSpritesList.Add(hotelSuiteSprites);
        //11
        buildingSpritesList.Add(entertainmentSprites);

    }

    //Builds at given coordinates using the current tool.
    public void build(int x, int y)
    {
        if(checkIfBuildable(x,y, Tools.toolHeight) && GameRun.cash >= Tools.currentToolCost && Tools.currentTool != 9){

            towerMap[x, y].setSprite(buildingSpritesList[Tools.currentTool][0], Tools.currentTool);
          
                GameRun.chargeMoney(Tools.currentToolCost);
                occupy(x, y);
                setDesirability(towerMap[x, y]);
            if(Tools.currentTool == 2)
            {
                elevatorList.Add(towerMap[x, y]);
            }
       
        }
        if(Tools.currentTool == 9)
        {
            if(towerMap[x,y].getBuildingType() != 9)
            {
                bulldoze(x, y);
               
                GameRun.chargeMoney(Tools.currentToolCost);
                
                setDesirability(towerMap[x, y]);
            }            
        }       
    }
    //Checks to see if you can build somewhere based on the current tool.
    private bool checkIfBuildable(int x, int y, int height)
    {

            for (int i = 0; i < Tools.toolWidth; i++)
            {
                for (int j = 0; j < Tools.toolHeight; j++)
                {
                    if (!towerMap[x + i, y + j].getIsOccupied())
                    {

                    }
                    else
                    {
                        return false;
                    }
                if (towerMap[x, y].getFloor() > 0)
                {
                    if (x != towerWidth &&
                    x + i <= towerWidth &&
                    (towerMap[x + i, y - 1].getIsOccupied() || towerMap[x + i, y - 1].getDirt()) || towerMap[x + i, y - 1].getBuildingType() == 9)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
                else if(towerMap[x,y].getFloor() < 0)
                {
                    if(x != towerWidth &&
                        x + i <= towerWidth &&
                        towerMap[x + i, y + Tools.toolHeight].getIsOccupied() || towerMap[x + i, y + Tools.toolHeight].getBuildingType() == 9)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }

        return true;
    }

    //Fills in used spaces.
    private void occupy(int x, int y)
    {
        for(int i = 0; i < Tools.toolWidth; i++)
        {
            for(int j = 0; j < Tools.toolHeight; j++)
            {
                towerMap[x + i, y + j].setIsOccupied(true);
            }            
        }

    }
    private void bulldoze(int x, int y)
    {
        int chance = Random.Range(0, 50);
        int w = towerMap[x, y].getWidth();
        int h = towerMap[x, y].getHeight();
        if(towerMap[x,y].getBuildingType() == 2)
        {

        }
        if (chance > 45)
        {
            for (int i = 0; i < w; i++)
            {
                for(int j = 0; j < h; j++)
                {
                    towerMap[x + i, y + j].setIsOccupied(false);
                    towerMap[x + i, y + j].setSprite(buildingSpritesList[Tools.currentTool][Random.Range(0, buildingSpritesList[Tools.currentTool].Count)], Tools.currentTool);
                }                               
            }
        }
        else
        {
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    towerMap[x + i, y + j].setIsOccupied(false);
                    towerMap[x + i, y + j].setSprite(buildingSpritesList[Tools.currentTool][0], Tools.currentTool);
                }
            }
        }
        
       
        
    }
    private void setDesirability(Building b)
    {
        int x;
        int y;
        b.getCoordinates(out x, out y);
        int t = b.getBuildingType();

        float desire = 0;

        Building[,] neighbors = getNeighbors(x, y);
        foreach(Building building in neighbors)
        {
            if(building != null)
            {
                desire += desireChart[t, building.getBuildingType()];
            }
        }
        b.setDesirability(desire);
        
    }

    public Building[,] getTowerMap()
    {
        return towerMap;
    }

    //TODO:Make not suck
    //Should make prebuilt arrays of adjacency for quick and easy to read code.
    public Building[,] getNeighbors(int x, int y)
    {
        Building[,] neighbors = new Building[7,7];

        neighbors[3, 3] = towerMap[x, y];
        for(int i = -3; i < 4; i++)
        {
            for(int j = -3; j < 4; j++)
            {   
                //if(towerMap[x + i, x + j] != null && (i + j) >= -3 && (i + j <= 2))
                    if (x + i >= 0 && 
                    (x + i) < towerWidth && 
                    y + j >= 0 && 
                    (y + j) < towerHeight && 
                    (i + j) >= -3 && 
                    (i + j <= 2))
                    {
                    neighbors[3 + i, 3 + j] = towerMap[x + i, y + j];                   
                }
                
            }
        }


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
    public List<Building> getElevators()
    {
        return elevatorList;
    }
    public Building findElevator(int y)
    {
        
        foreach(Building b in elevatorList)
        {
            if (y == b.getFloor())
            {
                
                return b;
            }
        }
        return null;
    }
    public void setTotalPopulation(int i)
    {
        totalPopulation = i;
    }
    public void addPopulation(int i)
    {
        totalPopulation += i;
    }
    public Sprite getRandomBuildingSprite(int i)
    {
        if(buildingSpritesList[i].Count > 1)
        {
            return buildingSpritesList[i][Random.Range(1, buildingSpritesList[i].Count)];
        }
        else
        {
            return buildingSpritesList[i][0];
        }
    }
    public Sprite getEmptyBuildingSprite(int i)
    {
        return buildingSpritesList[i][0];
    }
}
