using UnityEngine;
using System.Collections;

public class Result {

    int id;
    int type;
    int val;
    string resultName;
    string resultText;
    bool resultBool;
    float resultFloat;
    int resultInt;

    public void Init(int i, int t, int v, string n, string text, bool b)
    {
        id = i;
        type = t;
        val = v;
        resultName = n;
        resultBool = b;
        resultText = text;
    }
    public void Init(int i, int t, int v, string n, string text, float f)
    {
        id = i;
        type = t;
        val = v;
        resultName = n;
        resultFloat = f;
        resultText = text;
    }
    public void Init(int i, int t, int v, string n, string text, int c)
    {
        id = i;
        type = t;
        val = v;
        resultName = n;
        resultInt = c;
        resultText = text;
    }
    public string getName()
    {
        return resultName;
    }
    public string getText()
    {
        return resultText;
    }
    public int getID()
    {
        return id;
    }
}
