﻿using UnityEngine;
using System.Collections.Generic;

public class TowerMap : MonoBehaviour {

    [SerializeField]
    private int towerWidth;
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
        towerHeight = 300;
        towerWidth = 100;
        sizeRatio = .5f;
	    towerMap = new Building[towerWidth,towerHeight];

        loadSpriteList();
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
                    b.GetComponent<Building>().setSprite(buildingSpriteList[0]);
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
        Sprite d = Resources.Load("Buildings/Dirt", typeof(Sprite)) as Sprite;
        Sprite[] e = Resources.LoadAll<Sprite>("Buildings/Offices");
        Sprite l = Resources.Load("Buildings/Lobby", typeof(Sprite)) as Sprite;
        Sprite p = Resources.Load("Buildings/EmptyHalf", typeof(Sprite)) as Sprite;
        Debug.Log(Resources.Load("Buildings/Offices_1", typeof(Sprite)) as Sprite);
        buildingSpriteList.Add(d);
        buildingSpriteList.AddRange(e);
        buildingSpriteList.Add(l);
        buildingSpriteList.Add(p);

    }

    //Builds at given coordinates using the current tool.
    public void build(int x, int y)
    {
        Debug.Log(towerMap[x, y]);
        if(checkIfBuildable(x,y) && GameRun.cash >= Tools.currentToolCost){
            if(Tools.currentTool == 1)
            {
                towerMap[x, y].setSprite(buildingSpriteList[Random.Range(1,4)], Tools.currentTool);
            }
            else
            {
                towerMap[x, y].setSprite(buildingSpriteList[Tools.currentTool], Tools.currentTool);
            }            
            
            GameRun.chargeMoney(Tools.currentToolCost);
            occupy(x, y);
        }       
    }
    //Checks to see if you can build somewhere based on the current tool.
    //TODO:Make it simple
    private bool checkIfBuildable(int x, int y)
    {
        if(Tools.currentTool == 1)
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
        else if(Tools.currentTool == 2)
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
        if(Tools.currentTool == 1)
        {
            towerMap[x, y].setIsOccupied(true);
            towerMap[x + 1, y].setIsOccupied(true);
        }
        else if(Tools.currentTool == 2)
        {
            towerMap[x, y].setIsOccupied(true);
        }
    }

    public Building[,] getTowerMap()
    {
        return towerMap;
    }
}