using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class ElevatorCar : MonoBehaviour {

    [SerializeField]
    private int capacity;
    [SerializeField]
    private int maxCapacity;
    [SerializeField]
    private int currentFloor;
    [SerializeField]
    private UniqueQueue<int> floorQueue = new UniqueQueue<int>();
    [SerializeField]
    private List<GameObject> contents = new List<GameObject>();
    [SerializeField]
    private UniqueQueue<Character> waitQueue = new UniqueQueue<Character>();

    private int minFloor;
    private int maxFloor;

    private float lerpTime = 1;
    private float currentLerpTime = 0;
    private float time = 0;

    public void addFloorToQueue(int i)
    {
        floorQueue.Enqueue(i);
    }
    private void goToFloor(int i)
    {
        //Mathf.Lerp(transform.position.y, (float)i, time)
        //Debug.Log("Lerping to" + i + " from " + currentFloor);
        //Debug.Log("Heading to position " + FloorSpaceManager.convertFloorToPosition(i) + " which is supposedly floor " + i);
        i = (int)Mathf.Clamp(i, minFloor, maxFloor);
        transform.position = new Vector2(transform.position.x, Mathf.MoveTowards(transform.position.y, FloorSpaceManager.convertFloorToPosition(i), .1f));
    }

    // Update is called once per frame
 /*   void Update()
    {
        time = time * time * (3 - 2 * time);
        
        if (floorQueue.Count > 0)
        {
            currentFloor = FloorSpaceManager.convertPositionToFloor(transform.position.y);
            if (Mathf.Approximately(floorQueue.Peek(), currentFloor))
            {
                floorQueue.Dequeue();
                dropOffContents();
            }
            else
            {
                goToFloor(floorQueue.Peek());
            }
        }
    }
    */
    public void tick()
    {
        time = time * time * (3 - 2 * time);

        if (floorQueue.Count > 0)
        {
            Debug.Log("Elevator " + gameObject.GetComponentInParent<Elevator>().id + " going to floor " + floorQueue.Peek());
            if (FloorSpaceManager.convertPositionToFloorIfEqual(transform.position.y) != 0)
            {
                currentFloor = FloorSpaceManager.convertPositionToFloorIfEqual(transform.position.y);
                
                addCharacterQueueToCar();
            }      
            if (floorQueue.Peek() == currentFloor)
            {
                floorQueue.Dequeue();
                dropOffContents();
            }
            else
            {
                goToFloor(floorQueue.Peek());
            }
        }
    }
    private void addCharacterQueueToCar()
    {
        while(waitQueue.Count != 0)
        {
            Character c = waitQueue.Dequeue();
            contents.Add(c.gameObject);
            addFloorToQueue(FloorSpaceManager.convertPositionToFloor(c.gameObject.GetComponent<Character>().currentGoal.y));
            c.gameObject.SetActive(false);
        }
    }
    public void setCurrentFloor(int i)
    {
        currentFloor = i;
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Character")
        {
            // contents.Add(coll.gameObject);
            if (coll.gameObject.GetComponent<Character>().getCurrentFloor() == currentFloor){
                waitQueue.Enqueue(coll.gameObject.GetComponent<Character>());
                
            }
            else
            {
                waitQueue.Enqueue(coll.gameObject.GetComponent<Character>());
            }
            //capacity++;
            // addFloorToQueue(FloorSpaceManager.convertPositionToFloor(coll.gameObject.GetComponent<Character>().finalGoal.y));
            // coll.gameObject.SetActive(false);
        }
    }
    private void dropOffContents()
    {
        List<GameObject> removeList = new List<GameObject>();
        for(int i = 0; i < contents.Count; i++)
        {
            GameObject g = contents[i];

           // Debug.Log("Current Floor: " + currentFloor + ", Character Final Goal Y: " + g.GetComponent<Character>().finalGoal.y + ", Converted Goal Y: " + FloorSpaceManager.convertPositionToFloor(g.GetComponent<Character>().finalGoal.y));
            if(g.GetComponent<Character>() != null && FloorSpaceManager.convertPositionToFloor(g.GetComponent<Character>().currentGoal.y) == currentFloor)
            {
                Character c = g.GetComponent<Character>();
                Debug.Log(c.nextGoal + ", " + c.currentGoal);

                    Physics2D.IgnoreCollision(g.GetComponent<Collider2D>(), GetComponent<Collider2D>());

                    if (transform.position.x > c.finalGoal.x)
                    {
                        g.transform.position = transform.position - new Vector3(.1f, 0);
                    }
                    else
                    {
                        g.transform.position = transform.position + new Vector3(.6f, 0);
                    }

                    c.setCurrentFloor(currentFloor);

                    g.SetActive(true);
                    capacity--;
                    removeList.Add(g);
                    c.executeRoute();
                

            }
        }
        contents = contents.Except(removeList).ToList();
    }

    public void setFloorMinMax(int min, int max)
    {
        minFloor = min;
        maxFloor = max;
    }
}
