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
    [SerializeField]
    private List<Sprite> buildingSpriteList = new List<Sprite>();
    [SerializeField]
    private List<List<Sprite>> buildingSpritesList = new List<List<Sprite>>();
    [SerializeField]
    private List<Building> elevatorList = new List<Building>();
    [SerializeField]
    private int totalPopulation;
    [SerializeField]
    private int humanPopulation;
    [SerializeField]
    private int zombiePopulation;
    [SerializeField]
    private int witchPopulation;
    [SerializeField]
    private int demonPopulation;
    private List<Building> buildingsList = new List<Building>();
    
    //TODO:Implement this.
    [SerializeField]
    private int highestFloor;
    private int currentMapType;

    public bool testing = false;
    public List<Sprite> testList = new List<Sprite>();

    private string humanSpriteFolder = "Buildings/Humans";
    private string zombieSpriteFoler = "Buildings/Zombies";
    private string witchSpriteFolder = "Buildings/Witches";
    private string demonSpriteFolder = "Buildigns/Demons";
    private int co;

    // Use this for initialization
    void Start () {
        if (!testing)
        {
            towerHeight = 140;
            towerWidth = 100;

        }
        sizeRatio = .5f;
	    towerMap = new Building[towerWidth,towerHeight];
        
        loadSpriteList(humanSpriteFolder);
        loadSpriteList(zombieSpriteFoler);
        loadSpriteList(witchSpriteFolder);
        loadSpriteList(demonSpriteFolder);
        loadDesireChart();
        createTower();
        //InvokeRepeating("SpawnWitch", 10, 1);
       
    }


	//Makes the whole tower
    private void createTower()
    {
        for(int i = 0; i < towerWidth; i++)
        {
            for(int j = 0; j < towerHeight; j++)
            {
                //Unity is garbage and for some reason gets mad unless you do this.          
                GameObject t = (GameObject)Instantiate(building, new Vector3(i * sizeRatio, j * sizeRatio), Quaternion.identity);
                GameObject b = t.gameObject;
                towerMap[i, j] = b.GetComponent<Building>();
                createDirt(j, b);
                b.GetComponent<Building>().Init(i, j);
            }
        }
    }

    //This makes the dirt ground.
    private void createDirt(int j, GameObject b)
    {
        if (j == 40)
        {
            b.GetComponent<Building>().setSprite(buildingSpritesList[3][0], 3);

        }
        if (j > 15 && j < 40)
        {
            b.GetComponent<Building>().setSprite(buildingSpritesList[3][1], 3);

        }
        if (j == 15)
        {
            b.GetComponent<Building>().setSprite(buildingSpritesList[3][2], 3);

        }
        if (j < 15)
        {
            b.GetComponent<Building>().setSprite(buildingSpritesList[3][3], 3);

        }
        
        if (j > 40)
        {
            b.GetComponent<Building>().setFloor(j - 40);
        }
        else
        {
            b.GetComponent<Building>().setFloor(j - 41);
            b.GetComponent<Building>().setDirt(true);
        }
    }
    //Loads Sprites
    //TODO: Make it not suck
    private void loadSpriteList(string location)
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
        List<Sprite> utilitySprites = new List<Sprite>();

        allSprites.AddRange(Resources.LoadAll<Sprite>(location));

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
            else if(n == 'U')
            {
                utilitySprites.Add(s);
            }
            
        }
        //This currently loads in a specific order that the Tools class uses in order to differentiate factional buildings.
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
        //12
        buildingSpritesList.Add(utilitySprites);
        
    }

    //Builds at given coordinates using the current tool.
    public void build(int x, int y)
    {
        //Checks if the spaces are available to build and if the player has enough money to play. Tool 9 is the bulldozer and has different rules.
        if(checkIfBuildable(x,y, Tools.toolHeight) && GameRun.cash >= Tools.currentToolCost && Tools.currentTool != 9){
            
                towerMap[x, y].setSprite(buildingSpritesList[Tools.currentTool][0], Tools.currentTool);      
                GameRun.chargeMoney(Tools.currentToolCost);
                occupy(x, y);
                setDesirability(towerMap[x, y]);

            if(Tools.currentTool == 2)
            {
                elevatorList.Add(towerMap[x, y]);
            }
            buildingsList.Add(towerMap[x, y]);
       
        }
        //Bulldoze
        if(Tools.currentTool == 9)
        {
            //Only bulldozes non-empty buildings.
            if(towerMap[x,y].getBuildingType() != 9)
            {
                bulldoze(x, y);
                GameRun.chargeMoney(Tools.currentToolCost);
                setDesirability(towerMap[x, y]);
                buildingsList.Remove(towerMap[x, y]);
            }            
        }       
    }

    //Checks to see if you can build somewhere based on the current tool. Returns true if all spaces are unoccupied.
    private bool checkIfBuildable(int x, int y, int height)
    {
            for (int i = 0; i < Tools.toolWidth; i++)
            {
                for (int j = 0; j < Tools.toolHeight; j++)
                {
                    //If the location isn't occupied, do nothing.
                    if (!towerMap[x + i, y + j].getIsOccupied())
                    {

                    }
                    else
                    {
                        return false;
                    }
                //Checks for above ground.
                if (towerMap[x, y].getFloor() > 0)
                {
                    if (x != towerWidth &&
                        x + i <= towerWidth &&
                        (towerMap[x + i, y - 1].getIsOccupied() || 
                        towerMap[x + i, y - 1].getDirt()) || 
                        towerMap[x + i, y - 1].getBuildingType() == 9)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
                //Checks for below ground.
                else if(towerMap[x,y].getFloor() < 0)
                {
                    if(x != towerWidth &&
                        x + i <= towerWidth &&
                        towerMap[x + i, y + Tools.toolHeight].getIsOccupied() ||
                        towerMap[x + i, y + Tools.toolHeight].getBuildingType() == 9)
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
    //TODO: Make it suck less. Currently it changes all empties to a different sprite if the random chance hits.
    private void bulldoze(int x, int y)
    {
        //Random chance of a different empty sprite.
        int chance = Random.Range(0, 50);
        int w = towerMap[x, y].getWidth();
        int h = towerMap[x, y].getHeight();

        //If the deleted building is an elevator
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
    //TODO: Make it faster. Maybe use a preset array?
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
            if(building != null && building.getBuildingType() != -1)
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
    public int getPopulation()
    {
        return totalPopulation;
    }
    public List<List<Sprite>> getBuildingSpritesList()
    {
        return buildingSpritesList;
    }
    public void setMap(int i)
    {
        currentMapType = 1;
        if(i == 0)
        {
            foreach (Building b in buildingsList)
            {
                b.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        else if(i == 1)
        {
            foreach (Building b in buildingsList)
            {
                float red = b.getDesirability();
                

                b.GetComponent<SpriteRenderer>().color = new Color(1/(10-red), 0, 0);
            }
        }

    }
    public int getHumanPop()
    {
        return humanPopulation;
    }
    public int getZombiePop()
    {
        return zombiePopulation;
    }
    public int getWitchPop()
    {
        return witchPopulation;
    }
    public int getDemonPop()
    {
        return demonPopulation;
    }
    public void addHumanPop(int i)
    {
        humanPopulation += i;
        totalPopulation += i;
    }
    public void addZombiePop(int i)
    {
        zombiePopulation += i;
        totalPopulation += i;
    }
    public void addWitchPop(int i)
    {
        witchPopulation += i;
        totalPopulation += i;
    }
    public void addDemonPop(int i)
    {
        demonPopulation += i;
        totalPopulation += i;
    }
    public void setHighestFloor(int i)
    {
        highestFloor = i;
    }
    public int getHighestFloor()
    {
        return highestFloor;
    }
    public Color getCurrentMapColor(Building b)
    {
        if(currentMapType == 0)
        {
            return Color.white;
        }
        else if(currentMapType == 1)
        {
            return b.GetComponent<SpriteRenderer>().color = new Color(1 / (10 - b.getDesirability()), 0, 0);
        }
        else
        {
            return Color.white;
        }
    }
}
