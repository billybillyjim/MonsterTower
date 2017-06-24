using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    [SerializeField]
    private float moveSpeed = .1f;
    [SerializeField]
    private int currentFloor;
    
    public Vector2 finalGoal;
    public Vector2 currentGoal;
    [SerializeField]
    private List<Elevator> route = new List<Elevator>();
    [SerializeField]
    private Queue<Elevator> qRoute = new Queue<Elevator>();
    [SerializeField]
    private Queue<float[,]> qFloatRoute = new Queue<float[,]>();

    void OnEnable()
    {
        currentFloor = FloorSpaceManager.convertPositionToFloor(transform.position.y);
        GetComponent<SpriteRenderer>().sortingOrder = 50;
    }
    public void setGoal(Building b)
    {
        finalGoal = new Vector2(b.getX(), b.getY()) ;
        planRoute();
    }
    public void planRoute()
    {

        Elevator e = RouteManager.scanForElevatorsOnFloor(FloorSpaceManager.convertPositionToFloor(finalGoal.y), currentFloor);

        qRoute.Enqueue(e);

        if (e.checkForAccess(currentFloor))
        {
            return;
        }
        else
        {
            planRoute(e);
        }

    }
    public void planRoute(Elevator elevator)
    {
        Elevator e;
        if(currentFloor < FloorSpaceManager.convertPositionToFloor(finalGoal.y))
        {
            e = RouteManager.scanForElevatorsOnFloor(elevator.lowestFloor, currentFloor);
        }
        else
        {
            e = RouteManager.scanForElevatorsOnFloor(elevator.highestFloor, currentFloor);
        }

        qRoute.Enqueue(e);

        if (e.checkForAccess(currentFloor))
        {
            return;
        }
        else
        {
            planRoute(e);
        }
    }
    public void executeRoute()
    {
        if(qRoute.Count > 0)
        {
            currentGoal = qRoute.Dequeue().transform.position;
            
        }
    }
    private void goToElevator(Elevator e)
    {
        transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, e.transform.position.x, moveSpeed), transform.position.y);
    }
    private void goToBuilding(Building b)
    {
        transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, b.transform.position.x, moveSpeed), transform.position.y);
    }
    private void goToGoal()
    {
        transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, finalGoal.x, moveSpeed), transform.position.y);
        if(Mathf.Approximately(finalGoal.x, transform.position.x))
        {
            gameObject.SetActive(false);
        }
    }
    public void tick()
    {
        if (FloorSpaceManager.convertPositionToFloor(finalGoal.y) == currentFloor)
        { 
            goToGoal();
        }
        else
        {
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, currentGoal.x, moveSpeed), transform.position.y);
        }
    }
    void OnMouseUp()
    {
        executeRoute();
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Elevator")
        {
            coll.gameObject.GetComponent<Elevator>().callCar(currentFloor);
        }
    }
    public void setCurrentFloor(int i)
    {
        currentFloor = i;
    }
}
