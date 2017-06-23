using UnityEngine;
using System.Collections;

public class Result : Condition{

    int id;
    string value;
    string resultName;
    
    public void Init(int i, string s, string n)
    {
        id = i;
        value = s;
        resultName = n;
    }

    public string getName()
    {
        return resultName;
    }
    new public int getID()
    {
        return id;
    }
    new public string getValue()
    {
        return value;
    }
}
