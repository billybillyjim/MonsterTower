using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    private int[] buildingNumbers;

    private float[] rents;
    private float[] expenses;

    private int currentPop;

    public bool testing = false;

    //Not currently using this
    enum Days { Sat, Sun, Mon, Tue, Wed, Thu, Fri };


    // Use this for initialization
    void Start () {
        //Normal Play Speed
        gameSpeed = .3f;
        //Starting Cash
        //TODO: Make this load on game load.
        cash = 100000;
        //Starting Month
        month = 0;
        //Starting days in January
        daysInMonth = 31;
        day = 1;
        hour = 12;
        year = 2017;
        daysInMonths = new int[12];
        rents = new float[50];
        expenses = new float[50];
        loadDaysInMonths();
	}
	void Update()
    {
        //Time step
        hour += gameSpeed;
        if(hour >= 24)
        {
            day++;
            hour = 0;
            em.updateEvents();

            if(day > daysInMonth)
            {
                tickMonth();

                //Accounts for leap years.
                if(year % 4 == 0 && year % 400 == 0 && year % 100 != 0)
                {
                    if(month == 1)
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
                if(month >= 11)
                {
                    year++;
                    month = 0;
                }
            }
        }
         setTexts();
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
    public void testEvents()
    {
        int i = em.getEventList().Count;
        foreach(Event e in em.getEventList())
        {
            setConditionsForEventTest(e);
            test();
        }
    }
    private void setConditionsForEventTest(Event e)
    {

    }
    private void test()
    {
        
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
                    b.setSprite(tower.getRandomBuildingSprite(towerMap[b.getX(), b.getY()].getBuildingType()));
                    tower.addPopulation(i);
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
                    b.setSprite(tower.getEmptyBuildingSprite(towerMap[b.getX(), b.getY()].getBuildingType()));
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
    public int getBuildingValue(int i)
    {
        return buildingNumbers[i];
    }
}
