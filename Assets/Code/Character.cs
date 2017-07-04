using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    [SerializeField]
    private float moveSpeed = .1f;
    [SerializeField]
    private int currentFloor;

    public int id;
    
    public Vector2 finalGoal;
    public Vector2 currentGoal;

    [SerializeField]
    private Stack<Vector2> route = new Stack<Vector2>();

    void OnEnable()
    {
        currentFloor = FloorSpaceManager.convertPositionToFloor(transform.position.y);
        GetComponent<SpriteRenderer>().sortingOrder = 50;
    }
    public void tick()
    {
        goTowardGoal();
    }
    private void goTowardGoal()
    {
        transform.position = new Vector2(Mathf.MoveTowards(transform.position.x, currentGoal.x, moveSpeed), transform.position.y);
        if(Mathf.Approximately(transform.position.x, finalGoal.x) && Mathf.Approximately(transform.position.y, finalGoal.y))
        {
            gameObject.SetActive(false);
        }
    }
    public void setGoal(Building b)
    {
        finalGoal = b.transform.position;
        findRoute();
    }
    public void findRoute()
    {
        route.Push(finalGoal);
        Stack<Vector2> r = RouteManager.findRouteToGoal(finalGoal, currentFloor);
        while(r.Count > 0)
        {
            route.Push(r.Pop());
        }
    }
    public void updateToNextGoal()
    {
        if(route.Count > 0)
        {
            currentGoal = route.Pop();

        }
    }
    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "ElevatorCar" && route.Count > 0)
        {
            coll.gameObject.GetComponent<ElevatorCar>().pickUp(this.gameObject);           
        }
        else if(coll.gameObject.tag == "Elevator" && route.Count > 0)
        {
            coll.gameObject.GetComponent<Elevator>().callCar(currentFloor);
            coll.gameObject.GetComponent<Elevator>().addToWaitQueue(this);
        }

    }
    public void setCurrentFloor(int i)
    {
        currentFloor = i;
    }
    public int getCurrentFloor()
    {
        return currentFloor;
    }
}
