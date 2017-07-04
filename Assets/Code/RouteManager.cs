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
        return bestFit;
    }
    public static Stack<Vector2> findRouteToGoal(Vector2 finalGoal, int currentFloor)
    {
        Stack<Vector2> goals = new Stack<Vector2>();
        int finalGoalFloor = FloorSpaceManager.convertPositionToFloor(finalGoal.y);

        foreach(Elevator elevator in TowerMap.elevatorList)
        {
            if(elevator.checkForAccess(currentFloor, finalGoalFloor))
            {
                goals.Push(elevator.transform.position);
                return goals;
            }
            else if (elevator.checkForAccess(finalGoalFloor))
            {
                goals.Push(elevator.transform.position);
                if(currentFloor < finalGoalFloor)
                {
                    goals = findRouteToGoal(finalGoal, elevator.lowestFloor, goals);
                }
                else
                {
                    goals = findRouteToGoal(finalGoal, elevator.highestFloor, goals);
                }
            }
        }
        return goals;
    }
    public static Stack<Vector2> findRouteToGoal(Vector2 finalGoal, int currentFloor, Stack<Vector2> goals)
    {       
        int finalGoalFloor = FloorSpaceManager.convertPositionToFloor(finalGoal.y);
        int count = goals.Count;

        foreach (Elevator elevator in TowerMap.elevatorList)
        {
            if (elevator.checkForAccess(currentFloor, finalGoalFloor))
            {
                goals.Push(elevator.transform.position);
                return goals;
            }
            else if (elevator.checkForAccess(finalGoalFloor) && (elevator.lowestFloor < FloorSpaceManager.convertPositionToFloor(goals.Peek().y) || elevator.highestFloor > FloorSpaceManager.convertPositionToFloor(goals.Peek().y)))
            {
                goals.Push(elevator.transform.position);
                if(currentFloor < finalGoalFloor)
                {
                    goals = findRouteToGoal(finalGoal, elevator.lowestFloor, goals);
                }
                else
                {
                    goals = findRouteToGoal(finalGoal, elevator.highestFloor, goals);
                }
            }
        }
        return goals;
    }
}
