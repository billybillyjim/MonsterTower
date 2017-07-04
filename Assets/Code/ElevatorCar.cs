using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class ElevatorCar : MonoBehaviour {

    [SerializeField]
    private int capacity;
    [SerializeField]
    private float speed = .125f;
    [SerializeField]
    private int maxCapacity;
    [SerializeField]
    private int currentFloor;
    [SerializeField]
    private UniqueQueue<int> floorQueue = new UniqueQueue<int>();
    [SerializeField]
    private List<GameObject> contents = new List<GameObject>();

    [SerializeField]
    private bool waiting = false;

    private int minFloor;
    private int maxFloor;

    public void pickUp(GameObject g)
    {
        contents.Add(g);
        if (g.GetComponent<Character>())
        {
            g.GetComponent<Character>().updateToNextGoal();
            addFloorToQueue(FloorSpaceManager.convertPositionToFloor(g.GetComponent<Character>().currentGoal.y));
        }
        g.SetActive(false);
    }
    public void dropOff(GameObject g)
    {
        if (g.GetComponent<Character>())
        {
            if(g.GetComponent<Character>().currentGoal.x > transform.position.x)
            {
                g.transform.position = transform.position + new Vector3(1.1f, 0);
            }
            else
            {
                g.transform.position = transform.position - new Vector3(.5f, 0);
            }
            Physics2D.IgnoreCollision(g.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
        g.SetActive(true);
    }
    private void checkForDropOffs()
    {
        List<GameObject> contentsToRemove = new List<GameObject>();
        foreach(GameObject g in contents)
        {
            if (g.gameObject.GetComponent<Character>())
            {
                if(g.gameObject.GetComponent<Character>().currentGoal.y == transform.position.y)
                {
                    dropOff(g);
                    contentsToRemove.Add(g);
                }
            }
        }
        contents = contents.Except<GameObject>(contentsToRemove).ToList();
            }
    public void goToFloor(int floor)
    {
        if(currentFloor != floor)
        {
            transform.position = new Vector2(transform.position.x, Mathf.MoveTowards(transform.position.y, FloorSpaceManager.convertFloorToPosition(floor), speed));
            if(Mathf.Approximately(transform.position.y, Mathf.Round(transform.position.y)))
            {
                currentFloor = FloorSpaceManager.convertPositionToFloor(transform.position.y);
            }
        }
        else
        {
            floorQueue.Dequeue();
        }
    }
    public void tick()
    {
        if (floorQueue.Count > 0)
        {
            waiting = false;
            goToFloor(floorQueue.Peek());
            checkForDropOffs();
        }
        else if (contents.Count > 0)
        {
            waiting = false;
            addFloorToQueue(FloorSpaceManager.convertPositionToFloor(contents[0].GetComponent<Character>().currentGoal.y));
        }
        else
        {
            floorQueue.ClearHash();
            waiting = true;
        }
    }
    public void addFloorToQueue(int i)
    {

        floorQueue.Enqueue(i);
        Debug.Log("i is " + i + "Floorqueuecount is " + floorQueue.Count);
    }
    public void setCurrentFloor(int i)
    {
        currentFloor = i;
    }
    public void setFloorMinMax(int min, int max)
    {
        minFloor = min;
        maxFloor = max;
    }
    public bool getWaitStatus()
    {
        return waiting;
    }
}
