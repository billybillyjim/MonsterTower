using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectMenu : MonoBehaviour {

    public Text buildingTypeText;
    public Text buildingNameText;
    public Text buildingOccupantCountText;
    public Text buildingRentText;


    public void updateTexts(List<Building> b)
    {
        List<List<Building>> bList = new List<List<Building>>();

        foreach (Building bu in b)
        {
            if (bList.Find(x => x[0].getBuildingType() == bu.getBuildingType()) != null)
            {
                bList.Find(x => x[0].getBuildingType() == bu.getBuildingType()).Add(bu);
            }
            else
            {
                List<Building> newList = new List<Building>();
                newList.Add(bu);
                bList.Add(newList);
            }
        }

        string nametext = "";
        foreach(List<Building> l in bList)
        {
            if(l.Count > 0)
            {
                nametext += l.Count + " " + (l[0].getBuildingTypeString()) + "s, ";
            }
        }
        char[] charArray = new char[3];
        charArray[0] = ',';
        charArray[1] = ' ';
        nametext = nametext.TrimEnd(charArray);
        buildingNameText.text = nametext;
        
    }



    public void updateTexts(Building b)
    {
        buildingNameText.text = b.getBuildingTypeString();
        buildingTypeText.text = b.getBuildingTypeString();
        buildingOccupantCountText.text = "" + b.getPopulation();
        buildingRentText.text = "" + b.getRent();
    }
}
