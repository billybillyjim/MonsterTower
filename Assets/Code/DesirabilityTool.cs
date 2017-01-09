using UnityEngine;
using System.Collections.Generic;

public class DesirabilityTool : MonoBehaviour {

    private List<int> desirabilityModifierList = new List<int>();

	public static void checkDesirability(int i, Building[,] b)
    {      

        float desire = 0;


        b[3, 3].setDesirability(desire);

    }

    private List<int> setCurrentList(int i)
    {
        List<int> list = new List<int>();

        if(i == 1)
        {
            list.Add(2);
            list.Add(2);
            list.Add(1);
            list.Add(-2);
        }

        return list;
    }
}
