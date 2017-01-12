using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    private float moveSpeed = 1f;
    private Building goal;
    private Building finalGoal;
    [SerializeField]
    private int currentFloor;
    private bool isStopped = false;
    
    void Start()
    {
        currentFloor = 1;

    }
    public void Init(Building b)
    {
        finalGoal = b;
        Debug.Log("Final Goal:" + b.getFloor());
    }

    void Update()
    {
        if (!isStopped)
        {
            moveTowardsGoal();
        }       
    }
    void OnMouseUp()
    {
        
        
    }
    private void stop()
    {
        isStopped = true;
    }
    private void moveTo(float x)
    {       
        if(Mathf.Abs(transform.position.x - x) < 1)
        {
            stop();
        }
        if(transform.position.x < x)
        {
            gameObject.transform.Translate(new Vector3(x * Time.deltaTime, 0, 0));
        }
        else if (transform.position.x > x)
        {
            gameObject.transform.Translate(new Vector3(-x * Time.deltaTime, 0, 0));
        }

    }
    public void setGoal(Building b)
    {
        goal = b;
    }
    private void moveTowardsGoal()
    {
        if (!isStopped)
        {
            if (currentFloor == finalGoal.getFloor())
            {
                Debug.Log("Moving");
                moveTo(finalGoal.getX());
            }
            else
            {
                findElevator();
            }
        }        
    }
    private void findElevator()
    {
        Building e = GameObject.Find("Tower").GetComponent<TowerMap>().findElevator(currentFloor);
        
        if (e != null)
        {         
            setGoal(e);
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
