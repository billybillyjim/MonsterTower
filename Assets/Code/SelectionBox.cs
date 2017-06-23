using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour {

    List<Building> buildingsList = new List<Building>();
    private GameRun game;
    private int life = 5;

    void OnTriggerEnter2D(Collider2D other)
    {
        
        Building b = other.GetComponent<Building>();
        if (b != null &&
           !buildingsList.Contains(other.GetComponent<Building>()) && 
           other.GetComponent<Building>().getBuildingType() != -1)
        {
            buildingsList.Add(other.gameObject.GetComponent<Building>());
        }
        
    }
    void LateUpdate()
    {
        if(buildingsList.Count > 0)
        {
            selectBuildings();
            Destroy(this.gameObject);
        }
        if(life <= 0)
        {
            Destroy(this.gameObject);
        }
        life--;
    }
        
    public List<Building> getBuildings()
    {        
        return buildingsList;
    }
    private void selectBuildings()
    {
        game.addBuildingsToSelectedBuildings(buildingsList);
    }
    public void setGameRun(GameRun g)
    {
        game = g;
    }
}
