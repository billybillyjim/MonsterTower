using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    TowerMap tower;
    public GameObject upDragRect;
    public GameObject downDragRect;
    public GameObject shaft;

    public List<ElevatorCar> cars = new List<ElevatorCar>();

    public int highestFloor;
    public int lowestFloor;

    private bool isBeingDragged = false;

	// Use this for initialization
	void Start () {
        tower = GameObject.Find("Tower").GetComponent<TowerMap>();
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 10;
        upDragRect.GetComponent<ElevatorDragRect>().setElevatorAsParent(this);
        upDragRect.GetComponent<ElevatorDragRect>().setIsTopRect(true);
        downDragRect.GetComponent<ElevatorDragRect>().setElevatorAsParent(this);
        cars.Add(GetComponentInChildren<ElevatorCar>());
        updateFloors();
        GetComponentInChildren<ElevatorCar>().setCurrentFloor(highestFloor);
        
    }
	public void tick()
    {
        foreach(ElevatorCar c in cars)
        {
            c.tick();
        }
    }
    void OnMouseEnter()
    {
        if (Tools.currentTool.getName().Equals("Empty"))
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.red;

        }
        else
        {
            Color c = GetComponentInChildren<SpriteRenderer>().color;
            GetComponentInChildren<SpriteRenderer>().color = new Color(c.r,c.g,c.b,.3f);
            
        }
    }
    void OnMouseExit()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;      
    }
    void OnMouseUp()
    {
        if (Tools.currentTool.getName().Equals("Empty"))
        {
            tower.bulldozeElevator(this);
        }
    }

    public void toggleEnabled(bool b)
    {
        GetComponent<BoxCollider2D>().enabled = b;
        upDragRect.GetComponent<BoxCollider2D>().enabled = b;
        downDragRect.GetComponent<BoxCollider2D>().enabled = b;
        shaft.GetComponent<BoxCollider2D>().enabled = b;
    }
    public void makeTransparent(bool b)
    {
        if (!b)
        {
            GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, .3f);
            upDragRect.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            downDragRect.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            shaft.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .3f);
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.white;
            upDragRect.GetComponent<SpriteRenderer>().color = Color.white;
            downDragRect.GetComponent<SpriteRenderer>().color = Color.white;
            shaft.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    public void addAndRemoveFloors()
    {

        shaft.transform.localScale = new Vector3(1, Mathf.Abs(upDragRect.transform.position.y - downDragRect.transform.position.y));
        shaft.transform.position = new Vector3(shaft.transform.position.x, downDragRect.transform.position.y);
        GetComponent<BoxCollider2D>().size = new Vector2(.5f, Mathf.Abs(upDragRect.transform.position.y - downDragRect.transform.position.y) / 2f);
        GetComponent<BoxCollider2D>().offset = new Vector2(GetComponent<BoxCollider2D>().offset.x, (upDragRect.transform.localPosition.y + downDragRect.transform.localPosition.y) / 2);
        updateFloors();

    }
    public void setBeingDragged(bool b)
    {
        isBeingDragged = b;
    }
    private void updateFloors()
    {
        highestFloor = FloorSpaceManager.convertPositionToFloor(upDragRect.transform.position.y - 1);
        lowestFloor = FloorSpaceManager.convertPositionToFloor(downDragRect.transform.position.y);
        foreach(ElevatorCar c in cars)
        {
            c.setFloorMinMax(lowestFloor, highestFloor);
        }
    }

    public bool checkForAccess(int i)
    {
        if(i >= lowestFloor && i <= highestFloor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void callCar(int i)
    {      
        cars[0].addFloorToQueue(i);
    }
}
