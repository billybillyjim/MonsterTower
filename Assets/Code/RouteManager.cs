using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour {

	public static Elevator scanForElevatorsOnFloor(int floor, int charFloor)
    {
        Elevator bestFit = TowerMap.elevatorList[0];

        foreach(Elevator elevator in TowerMap.elevatorList)
        {
            if (elevator.checkForAccess(floor))
            {
               if(charFloor >= floor)
                {
                    if(elevator.highestFloor > bestFit.highestFloor)
                    {
                        bestFit = elevator;
                    }
                }
                else
                {
                    if (elevator.lowestFloor < bestFit.lowestFloor)
                    {
                        bestFit = elevator;
                    }
                }
            }
        }

        Debug.Log(bestFit.highestFloor);
        return bestFit;
    }
    
}
