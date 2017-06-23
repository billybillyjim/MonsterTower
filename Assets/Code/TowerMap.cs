using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class TowerMap : MonoBehaviour {

    [SerializeField]
    private int towerWidth;
    [SerializeField]
    private GameRun game;
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
    public static List<Elevator> elevatorList = new List<Elevator>();
    [SerializeField]
    private GameObject elevator;
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
    private List<BuildingData> buildingDataList = new List<BuildingData>();
    private List<int> lobbyFloors = new List<int>();

    public InspectMenu inspectMenu;

    public Toggle seeElevatorsToggle;

    public GameObject testCharacterObject;
    public Character witch;

    //TODO:Implement this.
    [SerializeField]
    private int highestFloor;
    private int currentMapType;

    public bool testing = false;
    public List<Sprite> testList = new List<Sprite>();

    private string spriteFolder = "Buildings";

    private int co;

    // Use this for initialization
    void Start () {

        setLobbyFloors();
        if (!testing)
        {
            towerHeight = 140;
            towerWidth = 100;

        }
        sizeRatio = 1f;
	    towerMap = new Building[towerWidth,towerHeight];
        
        loadSpritesList(spriteFolder);

        loadDesireChart();

        createTower();
        
        //witch = Instantiate(testCharacterObject, new Vector3(0,20.5f,0), Quaternion.identity).GetComponent<Character>();
        //witch.setCurrentFloor(1);
       
    }
    public void setGoal(Building b)
    {
        //witch.setGoal(b);
        //witch.executeRoute();
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
                t.transform.SetParent(this.transform);
            }
        }
    }

    //This makes the dirt ground.
    private void createDirt(int j, GameObject b)
    {
        if (j == 40)
        {
            b.GetComponent<Building>().makeGroundSprites(buildingDataList.Find(x => x.typeName.Equals("Ground") == true).getFullSpriteByName("Ground_Grass"), 3);
        }
        if (j > 15 && j < 40)
        {
            b.GetComponent<Building>().makeGroundSprites(buildingDataList.Find(x => x.typeName.Equals("Ground") == true).getFullSpriteByName("Ground_1"), 3);
        }
        if (j == 15)
        {
            b.GetComponent<Building>().makeGroundSprites(buildingDataList.Find(x => x.typeName.Equals("Ground") == true).getFullSpriteByName("Ground_Deep"), 3);
        }
        if (j < 15)
        {
            b.GetComponent<Building>().makeGroundSprites(buildingDataList.Find(x => x.typeName.Equals("Ground") == true).getFullSpriteByName("Ground_Deeper"), 3);
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
        b.GetComponent<SpriteRenderer>().sortingOrder = -1;
    }

    //Loads the sprites, adding them to buildingData. Each buildingData contains a set of sprites organized by name.
    //Names are decided by the word before the first underscore.
    //All empty rooms must have "Empty" somewhere in their name.
    private void loadSpritesList(string location)
    {
        List<Sprite> allSprites = new List<Sprite>();
        allSprites.AddRange(Resources.LoadAll<Sprite>(location));

        foreach (Sprite s in allSprites)
        {           
            string type = s.name.Split('_')[0];
            BuildingData bd = new BuildingData();
            bool exists = false;

            for (int i = 0; i < buildingDataList.Count; i++)
            {
                if (buildingDataList[i].getTypeName().Equals(type))
                {
                    bd = buildingDataList[i];
                    exists = true;
                }
            }

            if (exists == false)
            {
                bd = new BuildingData(type);
                buildingDataList.Add(bd);
            }

            if (s.name.Contains("Empty"))
            {
                bd.addEmpty(s);
            }
            else
            {
                bd.addFull(s);
            }
        }
    }
    //Builds at given coordinates using the current tool.
    public void build(int x, int y)
    {
        //Checks if the spaces are available to build and if the player has enough money to play. Tool 9 is the bulldozer and has different rules.
        if(checkIfBuildable(x,y, Tools.currentTool.getHeight()) && GameRun.cash >= Tools.currentTool.getCost() && Tools.currentTool.getName() != "Empty"){

            //The rules for building Lobbies
            if (y == 41 && !Tools.currentTool.getName().Equals("Lobby"))
            {
                return;
            }
            if(!lobbyFloors.Contains(y) && Tools.currentTool.getName().Equals("Lobby"))
            {               
                return;
            }
            if((y != 41 && lobbyFloors.Contains(y)) && Tools.currentTool.getName().Equals("Lobby") && !towerMap[x, y - 1].getBuildingTypeString().Equals("Lobby"))
            {
                return;
            }


            occupy(x, y);
            towerMap[x, y].setSprite(buildingDataList.Find(b => b.getTypeName().Equals(Tools.currentTool.getName())).getEmptySprite(), Tools.currentTool);      
            GameRun.chargeMoney(Tools.currentTool.getCost());
            DesiribilityModifier baseModifier = new DesiribilityModifier("Base", 15f);
            towerMap[x, y].addDesiribilityModifier(baseModifier);
           
            if (Tools.currentTool.getName().Equals("Lobby"))
            {
                updateLobbySprites(x,y);
            }
            if (Tools.currentTool.getName().Equals("Office"))
            {
                testCharacterObject.GetComponent<Character>().setGoal(towerMap[x,y]);
                
            }
            buildingsList.Add(towerMap[x, y]);
       
        }
        //Bulldoze
        if(Tools.currentTool.getName().Equals("Empty"))
        {
            //Only bulldozes non-empty buildings.
            if(towerMap[x,y].getBuildingType() != 9)
            {
                
                bulldoze(x, y);
                
            }            
        }       
    }
    public void inspect(int x, int y)
    {
        inspectMenu.updateTexts(towerMap[x, y]);
    }
    //Checks to see if you can build somewhere based on the current tool. Returns true if all spaces are unoccupied.
    private bool checkIfBuildable(int x, int y, int height)
    {
            for (int i = 0; i < Tools.currentTool.getWidth(); i++)
            {
                for (int j = 0; j < Tools.currentTool.getHeight(); j++)
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
                        towerMap[x + i, y + Tools.currentTool.getHeight()].getIsOccupied() ||
                        towerMap[x + i, y + Tools.currentTool.getHeight()].getBuildingType() == 9)
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
        for (int i = 0; i < Tools.currentTool.getWidth(); i++)
        {
            for (int j = 0; j < Tools.currentTool.getHeight(); j++)
            {
                towerMap[x + i, y + j].GetComponent<SpriteRenderer>().sprite = null;
                towerMap[x + i, y + j].setIsOccupied(true);
            }
        }

        setCollisionBox(x, y);


    }
    //TODO: Make it suck less. Currently it changes all empties to a different sprite if the random chance hits.
    public void bulldoze(int x, int y)
    {
        int w = towerMap[x, y].getWidth();
        int h = towerMap[x, y].getHeight();
        GameRun.chargeMoney(Tools.currentTool.getCost());
        buildingsList.Remove(towerMap[x, y]);

        for (int i = 0; i < w; i++)
        {
            for(int j = 0; j < h; j++)
            {
                towerMap[x + i, y + j].setIsOccupied(false);
                towerMap[x + i, y + j].setSpriteToEmpty(buildingDataList.Find(b => b.getTypeName().Equals("Empty")).getEmptySprite());
                towerMap[x + i, y + j].gameObject.GetComponent<SpriteRenderer>().color = getCurrentMapColor(towerMap[x + i, y + j]);
                towerMap[x + i, y + j].gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }
        setCollisionBox(x, y);
    }
    public void buildElevator()
    {
        Camera c = GameRun.camera;

        double x = Math.Floor((c.ScreenToWorldPoint(Input.mousePosition).x));
        double y = Math.Round(c.ScreenToWorldPoint(Input.mousePosition).y);
        if (towerMap[(int)x, (int)y].getBuildingTypeString().Equals("Empty") ||
            towerMap[(int)x, (int)y].getIsOccupied())
        {
        
            GameObject newElevator = Instantiate(elevator, new Vector2((float)(x), (float)(y)), Quaternion.identity);

            elevatorList.Add(newElevator.GetComponent<Elevator>());
        }
    }

    public void bulldozeElevator(Elevator e)
    {
        elevatorList.Remove(e);
        Destroy(e.gameObject);
    }
    public void showHideElevators(bool b)
    {
        seeElevatorsToggle.isOn = b;
        foreach(Elevator e in elevatorList)
        {
            e.toggleEnabled(b);
            e.makeTransparent(b);
           
        }
    }

    //TODO: Make it faster. Maybe use a preset array?
    private void setDesirability(Building b)
    {
        b.addDesiribilityModifier(new DesiribilityModifier("Happy", 15f));
    }
    public void addToInspectList(Building b)
    {
        game.addBuildingToSelectedBuildings(b);
    }
    //Sets the collision box to the size of the room.
    private void setCollisionBox(int x, int y)
    {
        towerMap[x, y].gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(Tools.currentTool.getWidth() / 4.0f, (Tools.currentTool.getHeight() - 1) * .25f);
        towerMap[x, y].gameObject.GetComponent<BoxCollider2D>().size = new Vector2(Tools.currentTool.getWidth() / 2.0f, Tools.currentTool.getHeight() / 2.0f);

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
    private void setLobbyFloors()
    {
        lobbyFloors.Add(41);
        lobbyFloors.Add(42);
        lobbyFloors.Add(43);
        lobbyFloors.Add(56);
    }
    private void updateLobbySprites(int x, int y)
    {
        if(towerMap[x, y - 2].getBuildingTypeString().Equals("Lobby"))
        {
            towerMap[x, y - 2].setSprite(buildingDataList.Find(b => b.getTypeName().Equals("LobbyBot")).getEmptySprite());
            towerMap[x, y - 1].setSprite(buildingDataList.Find(b => b.getTypeName().Equals("LobbyMid")).getEmptySprite());
            towerMap[x, y].setSprite(buildingDataList.Find(b => b.getTypeName().Equals("LobbyTop")).getEmptySprite());
        }
        else if (towerMap[x, y - 1].getBuildingTypeString().Equals("Lobby"))
        {
            towerMap[x, y].setSprite(buildingDataList.Find(b => b.getTypeName().Equals("LobbyTop")).getEmptySprite());
            towerMap[x, y - 1].setSprite(buildingDataList.Find(b => b.getTypeName().Equals("LobbyBot")).getEmptySprite());
        }
        else
        {
            towerMap[x, y].setSprite(buildingDataList.Find(b => b.getTypeName().Equals("LobbyOne")).getEmptySprite());
        }
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
    public List<Elevator> getElevators()
    {
        return elevatorList;
    }
 
    public void setTotalPopulation(int i)
    {
        totalPopulation = i;
    }
    public void addPopulation(int i)
    {
        totalPopulation += i;
    }

    public int getPopulation()
    {
        return totalPopulation;
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
    public List<BuildingData> getBuildingData()
    {
        return buildingDataList;
    }
    public InspectMenu getInspectMenu()
    {
        return inspectMenu;
    }
}
