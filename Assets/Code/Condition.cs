using UnityEngine;
using System.Collections;

public class Condition {

    int id;
    string value;
    
    public void Init(int i, string v)
    {
        id = i;
        value = v;
    } 
    public int getID()
    {
        return id;
    }
    public string getValue()
    {
        return value;
    }
}
