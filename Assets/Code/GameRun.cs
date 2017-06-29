using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using cakeslice;

public class GameRun : MonoBehaviour {

    [SerializeField]
    private TowerMap tower;
    [SerializeField]
    private MoneyManager mm;
    [SerializeField]
    private FactionManager fm;
    [SerializeField]
    private Tools tool;
    [SerializeField]
    private EventManager em;
    [SerializeField]
    private CashManager cm;

    [SerializeField]
    private Character witch;
    [SerializeField]
    private GameObject witchObject;

    private Building[,] towerMap;

    public static float cash;
    public static float gameSpeed;

    public Text cashtext;
    public Text Date;

    public Text utilityTotalText;

    private int[] daysInMonths;
    public static float hour;
    private float day;
    private int month;
    private int year;
    private float daysInMonth;
    private float lastMonthRent;
    private float lastMonthUtilities;

    private float[] rents;
    private float[] expenses;

    private int currentPop;
    public static Camera camera;
    public bool testing = false;
    public Sprite selecte;

    public List<Building> selectedBuildings = new List<Building>();
    public List<Character> characters = new List<Character>();

    private int witchID;

    //Not currently using this
    enum Days { Sat, Sun, Mon, Tue, Wed, Thu, Fri };
    private Vector2 boxStartPos = Vector2.zero;
    private Vector2 boxEndPos = Vector2.zero;
    public Texture SelectionTexture;
    // Use this for initialization
    void Start () {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //Normal Play Speed
        gameSpeed = .0f;
        //Starting Cash
        //TODO: Make this load on game load.
        cash = 100000;
        //Starting Month
        month = 0;
        //Starting days in January
        daysInMonth = 31;
        day = 30;
        hour = 12;
        year = 2017;
        daysInMonths = new int[12];
        rents = new float[50];
        expenses = new float[50];
        loadDaysInMonths();
	}
	void Update()
    {
        getInputs();
        tick();
        setTexts();
        // Called while the user is holding the mouse down.
        if (Input.GetKey(KeyCode.Mouse0) && Tools.currentTool.getName().Equals("Inspect"))
        {
            // Called on the first update where the user has pressed the mouse button.
            if (Input.GetKeyDown(KeyCode.Mouse0))
                boxStartPos = Input.mousePosition;
            else  // Else we must be in "drag" mode.
                boxEndPos = Input.mousePosition;
        }
        else
        {
            // Handle the case where the player had been drawing a box but has now released.
            if (boxEndPos != Vector2.zero && boxStartPos != Vector2.zero)
                selectBuildings(boxStartPos, boxEndPos);
            // Reset box positions.
            boxEndPos = boxStartPos = Vector2.zero;
        }
    }
    private void selectBuildings(Vector2 start, Vector2 end)
    {

        Vector2 newStart = camera.ScreenToWorldPoint(start);
        Vector2 newEnd = camera.ScreenToWorldPoint(end);

        float width = Math.Abs(newStart.x - newEnd.x);
        float height = Math.Abs(newStart.y - newEnd.y);

        if (Math.Abs(width) > .25f || Math.Abs(height) > .25f)
        {
        GameObject selectorBox = new GameObject();

        BoxCollider2D box = selectorBox.AddComponent<BoxCollider2D>();
        SelectionBox select = selectorBox.AddComponent<SelectionBox>();
        Rigidbody2D body = selectorBox.AddComponent<Rigidbody2D>();
        select.setGameRun(this);
        body.gravityScale = 0;
            
            box.isTrigger = true;
            if (newStart.x > newEnd.x)
            {
                box.transform.position = newEnd;
            }
            else if (newStart.x <= newEnd.x)
            {
                box.transform.position = newStart;
            }
            if(newStart.y < newEnd.y)
            {
                box.transform.position = new Vector3(box.transform.position.x, newEnd.y);
            }
            else if(newStart.y >= newEnd.y)
            {
                box.transform.position = new Vector3(box.transform.position.x, newStart.y);
            }

            box.offset = new Vector3(width / 2, -height / 2);
            box.size = new Vector2(width, height);

            selectedBuildings.AddRange(select.getBuildings());
        }
    }

    public void addBuildingsToSelectedBuildings(List<Building> b)
    {
        
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            foreach (Building bu in selectedBuildings)
            {
                bu.GetComponent<cakeslice.Outline>().enabled = false;
            }
            selectedBuildings.Clear();
        }
        selectedBuildings.AddRange(b);

        if(selectedBuildings.Count > 0)
        {
            foreach (Building bu in selectedBuildings)
            {
                if (bu.gameObject.GetComponent<SpriteRenderer>().sprite != null)
                {                  
                    bu.gameObject.GetComponent<cakeslice.Outline>().enabled = true;
                }

            }
        }
        tower.getInspectMenu().updateTexts(b);
    }

    public void addBuildingToSelectedBuildings(Building b)
    {

        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            foreach (Building bu in selectedBuildings)
            {
                bu.GetComponent<cakeslice.Outline>().enabled = false;
            }
            selectedBuildings.Clear();
        }


        selectedBuildings.Add(b);

        if (selectedBuildings.Count > 0)
        {
            foreach (Building bu in selectedBuildings)
            {
                if (bu.gameObject.GetComponent<SpriteRenderer>().sprite != null)
                {
                    bu.gameObject.GetComponent<cakeslice.Outline>().enabled = true;
                }

            }
        }
        tower.getInspectMenu().updateTexts(b);
    }

    void OnGUI()
    {
        if (boxStartPos != Vector2.zero && boxEndPos != Vector2.zero)
        {
            // Create a rectangle object out of the start and end position while transforming it
            // to the screen's cordinates.
            var rect = new Rect(boxStartPos.x, Screen.height - boxStartPos.y,
                                boxEndPos.x - boxStartPos.x,
                                -1 * (boxEndPos.y - boxStartPos.y));
            // Draw the texture.
            //GUI.DrawTexture(rect, SelectionTexture);
            GUI.DrawTexture(new Rect(boxStartPos.x, Screen.height - boxStartPos.y,
                                5,
                                 -1 * (boxEndPos.y - boxStartPos.y)), SelectionTexture);
            GUI.DrawTexture(new Rect(boxStartPos.x, Screen.height - boxStartPos.y,
                               boxEndPos.x - boxStartPos.x,
                                5), SelectionTexture);
            GUI.DrawTexture(new Rect(boxEndPos.x - 5, Screen.height - boxStartPos.y,
                                5,
                                 -1 * (boxEndPos.y - boxStartPos.y)), SelectionTexture);
            GUI.DrawTexture(new Rect(boxStartPos.x, Screen.height - boxEndPos.y - 5,
                               boxEndPos.x - boxStartPos.x,
                                5), SelectionTexture);
        }
    }
    private void tick()
    {
        //Time step
        hour += gameSpeed;
        foreach(Elevator e in tower.getElevators())
        {
            e.tick();
        }
        foreach (Character c in characters)
        {
            c.tick();
        }
        if (hour >= 24)
        {
            day++;
            hour = 0;
            em.updateEvents();

            if (day > daysInMonth)
            {
                tickMonth();

                //Accounts for leap years.
                if (year % 4 == 0 && year % 400 == 0 && year % 100 != 0)
                {
                    if (month == 1)
                    {
                        daysInMonth = 29;
                    }
                }
                else
                {
                    daysInMonth = daysInMonths[month];
                }

                day = 1;

                //Months go from 0-11
                if (month >= 11)
                {
                    year++;
                    month = 0;
                }
            }
        }
    }
    private void getInputs()
    {
        if (Input.GetKeyDown("o"))
        {
            tool.setTool("Office");
            tool.setButtonAsSelected("Office");
        }
    }
    private void tickMonth()
    {
        month++;

        earnRent();
        payUtilities();

        checkMoveIns();
        checkMoveOuts();

        fm.trendFactionsTowardNeutral();
    }

    private void setTexts()
    {

            Date.text = "Date:" + (month + 1) + "/" + day + "/" + year + " and " + hour + " Hours";
            cashtext.text = "$" + cash;
            mm.updateRents();
             
    }

    private void earnRent()
    {
        towerMap = tower.getTowerMap();
        float total = 0;
        rents = new float[50];
        foreach(Building b in towerMap)
        {
            if(b.getBuildingType() >= 0)
            {
                rents[b.getBuildingType()] += b.getRent();
                total += b.getRent();
            }          
        }

        cash += total;
        lastMonthRent = total;
    }
    public void setGoal()
    {
        tower.setGoal(selectedBuildings[0]);
    }
    private void payUtilities()
    {
        towerMap = tower.getTowerMap();
        float total = 0;
        expenses = new float[50];
        foreach(Building b in towerMap)
        {
            if(b.getBuildingType() >= 0)
            {
                expenses[b.getBuildingType()] += b.getUtilityPay();
                total += b.getUtilityPay();
            }          
        }
        cash -= total;
        lastMonthUtilities = total;
    }
    private void checkMoveIns()
    {
        towerMap = tower.getTowerMap();
        
        foreach(Building b in towerMap)
        {   
            if(b.getPopulation() == 0)
            {
                if (b.getDesirability() > 10)
                {
                    int i = tool.getPopToMoveIn(b.getBuildingType());
                    b.moveIn(i);
                    b.setSprite(tower.getBuildingData().Find(x => x.getTypeName().Equals(b.getBuildingTypeString())).getFullSprite());
                    tower.addPopulation(i);
                    //for(int j = 0; j < i; j++)
                   // {
                        Character witch = Instantiate(witchObject, new Vector3(UnityEngine.Random.Range(-1f, 1f), 41f, 0), Quaternion.identity).GetComponent<Character>();
                        characters.Add(witch);
                    witch.id = witchID;
                    witchID++;
                        b.addTennant(witch);
                        witch.setCurrentFloor(1);
                        witch.setGoal(b);
                        witch.executeRoute();
                  //  }                 
                }
            }          
        }
    }

    //TODO: Make it not only lower population by 10.
    private void checkMoveOuts()
    {
        towerMap = tower.getTowerMap();
        foreach(Building b in towerMap)
        {
            if(b.getPopulation() != 0)
            {
                if(b.getDesirability() < 5)
                {
                    b.moveOut();
                    b.setSprite(tower.getBuildingData().Find(x => x.getTypeName().Equals(b.getBuildingTypeString())).getEmptySprite());
                    tower.addPopulation(-10);
                }
            }
        }
    }

    public static void chargeMoney(float f)
    {
        cash -= f;
    }
    public void addMoney(float f)
    {
        cash += f;
    }
    public float getHour()
    {
        return hour;
    }
    public void setSpeed(float f)
    {
        gameSpeed = f;
    }
    private void loadDaysInMonths()
    {
        //Jan
        daysInMonths[0] = 31;
        //Feb
        daysInMonths[1] = 28;
        //Mar
        daysInMonths[2] = 31;
        //Apr
        daysInMonths[3] = 30;
        //May
        daysInMonths[4] = 31;
        //June
        daysInMonths[5] = 30;
        //Jul
        daysInMonths[6] = 31;
        //Aug
        daysInMonths[7] = 31;
        //Sep
        daysInMonths[8] = 30;
        //Oct
        daysInMonths[9] = 31;
        //Nov
        daysInMonths[10] = 30;
        //Dec
        daysInMonths[11] = 31;
    }

    public float[] getRents()
    {
        return rents;
    }
    public float[] getExpenses()
    {
        return expenses;
    }
    public int getCurrentYear()
    {
        return year;
    }
    public float getMoney()
    {
        return cash;
    }
    public void setMoney(float f)
    {
        cash = f;
    }
    public void setCurrentYear(int i)
    {
        year = i;
    }
}
