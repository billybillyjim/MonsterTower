using UnityEngine;
using System.Collections.Generic;

public class Flag {

    private bool value;
    private int ID;
    private string flagName;
    private List<Condition> conditions;

    public void Init(int i, string s, List<Condition> c)
    {
        ID = i;
        flagName = s;
        conditions = c;
    }
    public List<Condition> getCondition()
    {
        return conditions;
    }
    public string getName()
    {
        return flagName;
    }
    public int getID()
    {
        return ID;
    }
    public bool getValue()
    {
        return value;
    }
}
