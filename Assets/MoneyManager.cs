using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneyManager : MonoBehaviour {

    private float[] rents;
    private float[] expenses;
    [SerializeField]
    private GameRun game;

    public Text totalRentText;
    public Text totalOfficeRentText;
    public Text totalRestaurantRentText;
    public Text totalCondoRentText;
    public Text totalHotelRentText;

    private float totalRent;
    private float totalOfficeRent;
    private float zombieOfficeRent;
    private float totalRestaurantRent;
    private float totalHotelRent;
    private float totalCondoRent;

    private float totalExpenses;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        totalRentText.text = "Total Rent: $" + totalRent;
        totalOfficeRentText.text = "Office Rent: $" + totalOfficeRent;
        totalRestaurantRentText.text = "Restaurant Rent: $" + totalRestaurantRent;
        totalHotelRentText.text = "Hotel Rent: $" + totalHotelRent;
        totalCondoRentText.text = "Condo Rent: $" + totalCondoRent;
	}

    public void updateRents()
    {
        rents = game.getRents();
        totalRent = 0;
        foreach(float f in rents)
        {
            totalRent += f;
            
        }
        totalOfficeRent = rents[0];
        totalRestaurantRent = rents[1];
        totalCondoRent = rents[7];
        totalHotelRent = (rents[5] + rents[6] + rents[10]);

    }
    public void updateExpenses()
    {
        expenses = game.getExpenses();
        totalExpenses = 0;
        foreach(float f in expenses)
        {
            totalExpenses += f;
        }

    }

}
