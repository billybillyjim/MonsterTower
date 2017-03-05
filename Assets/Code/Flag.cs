using UnityEngine;
using System.Collections.Generic;

public class Flag {

    private bool truthValue;
    private int ID;
    private string flagName;
    private List<Condition> conditions;

    public void Init(int i, string s, List<Condition> c)
    {
        ID = i;
        flagName = s;
        conditions = c;
        truthValue = false;
    }
    public List<Condition> getConditions()
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
    public bool getTruthValue()
    {
        return truthValue;
    }
    public void setTruthValue(bool b)
    {
        truthValue = b;
    }
}
