using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    private float moveSpeed = 1f;
    private Vector3 goal;
    private Vector3 finalGoal;
    private int currentFloor;
    
    void Start()
    {
        finalGoal = new Vector3(10, 0);
        currentFloor = 2;
    }

    void Update()
    {
        moveTowardsGoal();
    }
    void OnMouseUp()
    {
        setGoal(10,0);
        
    }

    private void moveTo(float x)
    {       
        gameObject.transform.Translate(new Vector3(x * Time.deltaTime, 0, 0));
    }
    public void setGoal(int x, int y)
    {
        goal = new Vector3(x, y);
    }
    private void moveTowardsGoal()
    {
        if(goal.y == transform.position.y)
        {
            moveTo(goal.x);
        }
        else
        {
            findElevator();
        }
    }
    private void findElevator()
    {
        Building e = GameObject.Find("Tower").GetComponent<TowerMap>().findElevator(currentFloor);
        Debug.Log(e);
        if (e != null)
        {
            setGoal(e.getX(), e.getY());
        }
        else
        {
            findStairs();
        }
    }
    private void findStairs()
    {

    }
    private void leave()
    {

    }
}
