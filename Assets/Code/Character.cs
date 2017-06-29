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
    public Vector2 nextGoal;
    [SerializeField]
    private List<Elevator> route = new List<Elevator>();
    [SerializeField]
    private Stack<Elevator> qRoute = new Stack<Elevator>();
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
        qRoute = RouteManager.getElevatorRouteQueue(FloorSpaceManager.convertPositionToFloor(finalGoal.y), currentFloor);
    }

    public void executeRoute()
    {
        
        if (qRoute.Count > 0)
        {
            Debug.Log("qRoute size: " + qRoute.Count);
            Debug.Log(id + " going to " + qRoute.Peek().id + "to get to floor" + qRoute.Peek().transform.position);
            currentGoal = qRoute.Pop().transform.position;
            if(qRoute.Count > 0)
            {
                nextGoal = qRoute.Peek().transform.position;
            }
            else
            {
                nextGoal = currentGoal;
            }
            
        }
        else
        {
            currentGoal = finalGoal;
            Debug.Log(id + " going to " + finalGoal.y);
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
    public int getCurrentFloor()
    {
        return currentFloor;
    }
}
