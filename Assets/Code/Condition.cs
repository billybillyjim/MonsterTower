using UnityEngine;
using System.Collections;

public class Condition {

    int id;
    int type;
    int val;
    string conName;
    bool conBool;
    float conFloat;
    int conInt;
    bool validity;


    public void Init(int i, int t, int v, string n, bool b)
    {
        id = i;
        type = t;
        val = v;
        conName = n;
        conBool = b;
    }
    public void Init(int i, int t, int v, string n, float f)
    {
        id = i;
        type = t;
        val = v;
        conName = n;
        conFloat = f;
    }
    public void Init(int i, int t, int v, string n, int c)
    {
        id = i;
        type = t;
        val = v;
        conName = n;
        conInt = c;
    }
    public int getID()
    {
        return id;
    }
    public int getType()
    {
        return type;
    }
    public int getVal()
    {
        return val;
    }
    public int getInt()
    {
        return conInt;
    }
    public float getFloat()
    {
        return conFloat;
    }
    public bool getBool()
    {
        return conBool;
    }
    public bool getValidity()
    {
        return validity;
    }
    public void setValidity(bool b)
    {
        validity = b;
    }
    public string getName()
    {
        return conName;
    }
}
