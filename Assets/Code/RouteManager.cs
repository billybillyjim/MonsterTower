using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour {

    public static int safety = 0;

	public static Elevator scanForElevatorsOnFloor(int floor, int charFloor)
    {
        Elevator bestFit = TowerMap.elevatorList[0];

        foreach(Elevator elevator in TowerMap.elevatorList)
        {
            if (elevator.checkForAccess(floor))
            {
                bestFit = elevator;
                if (bestFit.checkForAccess(charFloor))
                {
                    return bestFit;
                }
            }
        }
        Debug.Log("goalfloor: " + floor + ", charFloor: " + charFloor + ", eid: " + bestFit.id);
        return bestFit;
    }

    public static Stack<Elevator> getElevatorRouteQueue(int goalFloor, int startFloor)
    {
        Stack<Elevator> elevatorStack = new Stack<Elevator>();
        int newGoalFloor = goalFloor;
        safety++;
        foreach (Elevator elevator in TowerMap.elevatorList)
        {
            if (elevator.checkForAccess(newGoalFloor))
            {
                elevatorStack.Push(elevator);

                if (elevatorStack.Peek().checkForAccess(startFloor))
                {
                    return elevatorStack;
                }
                else
                {
                    if(startFloor < goalFloor)
                    {
                        newGoalFloor = elevatorStack.Peek().lowestFloor;
                    }
                    else
                    {
                        newGoalFloor = elevatorStack.Peek().highestFloor;
                    }
                    elevatorStack = getElevatorRouteQueue(newGoalFloor, startFloor, elevatorStack);
                }
            }
        }

        return elevatorStack;
    }
    public static Stack<Elevator> getElevatorRouteQueue(int goalFloor, int startFloor, Stack<Elevator> elevatorStack)
    {
        int newGoalFloor = goalFloor;
        safety++;
        if(safety > 1000)
        {
            return elevatorStack;
        }
        foreach (Elevator elevator in TowerMap.elevatorList)
        {
            if (elevator.checkForAccess(newGoalFloor))
            {
                elevatorStack.Push(elevator);

                if (elevatorStack.Peek().checkForAccess(startFloor))
                {
                    return elevatorStack;
                }
            }
        }
        return elevatorStack;
    }
}
