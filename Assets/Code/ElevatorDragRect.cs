using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDragRect : MonoBehaviour {

    private Elevator elevator;
    [SerializeField]
    private bool isBeingDragged = false;
    public bool isTopRect = false;

    public void setElevatorAsParent(Elevator e)
    {
        elevator = e;
    }
    public void setIsTopRect(bool b)
    {
        isTopRect = b;
    }

    void Update()
    {
        if (isBeingDragged)
        {
            Vector3 pos = GameRun.camera.ScreenToWorldPoint(Input.mousePosition);
            float newY = Mathf.Round(pos.y);
            if (isTopRect)
            {
                transform.position = new Vector3(transform.position.x, newY + .5f);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, newY - .5f);
            }
            elevator.addAndRemoveFloors();
            
        }
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            elevator.setBeingDragged(true);
            isBeingDragged = true;
        }
        
    }
    void OnMouseUp()
    {
        isBeingDragged = false;
    }
}
