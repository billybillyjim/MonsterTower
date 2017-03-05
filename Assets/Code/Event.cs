using UnityEngine;
using System.Collections.Generic;

public class Event{

    private List<Flag> flagList = new List<Flag>();
    private List<Result> resultList = new List<Result>();
    private int ID;
    private string title;
    private string text;
    private int weight;
    private bool fired;

    public void Init(List<Flag> fList, List<Result> rList, int i, string t, string txt, int w)
    {
        flagList = fList;
        resultList = rList;
        ID = i;
        title = t;
        text = txt;
        weight = w;
    }

    public string getTitle()
    {
        return title;
    }
    public string getText()
    {
        return text;
    }
    public int getID()
    {
        return ID;

    }
    public int getWeight()
    {
        return weight;
    }
    public List<Result> getResults()
    {
        Debug.Log(resultList.Count);
        return resultList;
    }
    public List<Flag> getFlags()
    {
        return flagList;
    }
    public void setFired(bool b)
    {
        fired = b;
    }
    public bool getFired()
    {
        return fired;
    }
}
