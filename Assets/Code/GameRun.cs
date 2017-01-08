using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameRun : MonoBehaviour {

    [SerializeField]
    private TowerMap tower;

    private Building[,] towerMap;

    public static float cash;
    public Text cashtext;
    public Text Date;
    private int[] daysInMonths;
    public static float hour;
    private float day;
    private int month;
    private float year;
    private float daysInMonth;
    public static float gameSpeed;


	// Use this for initialization
	void Start () {
        gameSpeed = .3f;
        cash = 100000;
        month = 0;
        daysInMonth = 31;
        day = 1;
        hour = 12;
        year = 2017;
        daysInMonths = new int[12];
        loadDaysInMonths();
	}
	void Update()
    {
        cashtext.text = "$" + cash;
        hour += gameSpeed;
        if(hour >= 24)
        {
            day++;
            hour = 0;

            if(day > daysInMonth)
            {
                month++;

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
                
                if(month >= 11)
                {
                    year++;
                    month = 0;
                }
            }
        }
        Date.text = "Date:" + daysInMonth + ", " + (month + 1) + "/" + day + "/" + year + " and " + hour + " Hours";

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
}
