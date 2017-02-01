using UnityEngine;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

    private List<Condition> conditionList = new List<Condition>();
    private List<Flag> flagList = new List<Flag>();
    private List<Event> eventList = new List<Event>();
    private List<Result> resultList = new List<Result>();
    private int currentConditionCheck = 0;

    public GameRun game;
    public TowerMap tower;

    void Start()
    {
        loadConditions();
        loadFlags();
        loadResults();
        loadEvents();
        //updateConditions();
    }
    void Update()
    {
        updateConditions();
        
    }

    private void updateConditions()
    {
        updateCondition(conditionList[currentConditionCheck]);       
        if(currentConditionCheck + 1 >= conditionList.Count)
        {
            currentConditionCheck = 0;
        }
        else
        {
            currentConditionCheck++;
        }
        
    }
    private void updateCondition(Condition c)
    {
        int v = c.getType();
        if(v == 0)
        {
            updateConditionValue(c, c.getInt());
        }
        else if(v == 1)
        {
            updateConditionValue(c, c.getFloat());
        }
    }
    //Pay close attention to comparators. It's backwards from what you might think.
    private void updateConditionValue(Condition c, int t)
    {
        int i = c.getVal();
        
        switch (i)
        {
            case -1:
                if(t > tower.getPopulation())
                {
                    c.setValidity(true);
                }
                break;
            case 0:
                if (t < game.getCurrentYear())
                {
                    c.setValidity(true);
                }
                break;
            case 1:
                if(t < tower.getPopulation())
                {
                    
                    c.setValidity(true);
                }
                break;
        }
        
    }

    private void updateConditionValue(Condition c, float f)
    {
        int i = c.getType();
        switch (i)
        {
            case -6:
                if(f > game.getMoney())
                {
                    c.setValidity(true);
                }
                break;
            case 6:
                if(f < game.getMoney())
                {
                    c.setValidity(true);
                }
                break;
        }
        
    }


    private void loadConditions()
    {
        string file = System.IO.File.ReadAllText("Assets/Resources/Conditions.csv");
        string[] lines = file.Split("\n"[0]);

        for (int j = 0; j < lines.Length; j++)
        {
            string[] lineData = (lines[j].Trim()).Split(","[0]);

            Condition c = new Condition();
            int id = int.Parse(lineData[0]);
            string s = lineData[1];
            
            int t = 0;
            int v = 0;
            if(lineData[2] != "")
            {
                t = int.Parse(lineData[2]);
            }
            if(lineData[3] != "")
            {
                v = int.Parse(lineData[3]);
            }
            bool b;
            if (bool.TryParse(lineData[4], out b))
            {
                c.Init(id, t, v, s, b);
            }
            float f;
            if(float.TryParse(lineData[5], out f))
            {
                c.Init(id, t, v, s, f);
            }        
            int i;
            if (int.TryParse(lineData[6], out i)){
                c.Init(id, t, v, s, i);
            }
            conditionList.Add(c);
        }
    }
    private void loadFlags()
    {
        string file = System.IO.File.ReadAllText("Assets/Resources/Flags.csv");
        string[] lines = file.Split("\n"[0]);
        
        for (int j = 0; j < lines.Length; j++)
        {
            string[] lineData = (lines[j].Trim()).Split(","[0]);
            List<Condition> con = new List<Condition>();
            int id = int.Parse(lineData[0]);
            string s = lineData[1];
            
            string[] conditions = lineData[2].Split("/"[0]);
            
            foreach(string c in conditions)
            {
                int pos = int.Parse(c);
                con.Add(conditionList[pos]);
            }
            Flag flag = new Flag();
            flag.Init(id, s, con);
            flagList.Add(flag);
        }
    }
    private void loadResults()
    {
        string file = System.IO.File.ReadAllText("Assets/Resources/Results.csv");
        string[] lines = file.Split("\n"[0]);

        for (int j = 0; j < lines.Length; j++)
        {
            string[] lineData = (lines[j].Trim()).Split(","[0]);

            Result r = new Result();
            int id = int.Parse(lineData[0]);
            string s = lineData[1];
            string txt = lineData[2];
            int t = 0;
            int v = 0;
            if (lineData[3] != "")
            {
                t = int.Parse(lineData[3]);
            }
            if (lineData[4] != "")
            {
                v = int.Parse(lineData[4]);
            }
            bool b;
            if (bool.TryParse(lineData[5], out b))
            {
                r.Init(id, t, v, s, txt, b);
            }
            float f;
          
            if (float.TryParse(lineData[6], out f))
            {
                r.Init(id, t, v, s, txt, f);
            }
            

            int i;
            if (int.TryParse(lineData[7], out i))
            {
                r.Init(id, t, v, s, txt, i);
            }
            resultList.Add(r);
        }
    }

    private void loadEvents()
    {
        string file = System.IO.File.ReadAllText("Assets/Resources/Events.csv");
        string[] lines = file.Split("\n"[0]);

        for (int j = 0; j < lines.Length; j++)
        {
            Event e = new Event();
            string[] lineData = (lines[j].Trim()).Split(","[0]);
            int id = int.Parse(lineData[0]);
            string title = lineData[1];
            string text = lineData[2];
            int[] flagIDs = new int[10];
            string[] flags = (lineData[3].Trim().Split("/"[0]));
            for(int i = 0; i < flags.Length; i++)
            {
                if(flags[i] != "")
                {
                    flagIDs[i] = int.Parse(flags[i]);
                }
                
            }
            int[] resultIDs = new int[10];
            string[] results = lineData[4].Trim().Split("/"[0]);
            for(int i = 0; i < results.Length; i++)
            {
                if(results[i] != "")
                {
                    resultIDs[i] = int.Parse(results[i]);
                }
                
            }
            float weight = 0f;
            if (lineData[5] != "")
            {
                weight = float.Parse(lineData[5]);
            }
            
            e.Init(getFlagsFromList(flagIDs), getResultsFromList(resultIDs), id, title, text, weight);
            eventList.Add(e);
        }
        foreach(Event e in eventList)
        {
            Debug.Log(e.getTitle() + ", " + e.getText());
        }
    }
    private List<Flag> getFlagsFromList(int[] f)
    {
        List<Flag> returnList = new List<Flag>();
        for(int i = 0; i < f.Length; i++)
        {
            foreach (Flag flag in flagList)
            {

                if (flag.getID() == f[i])
                {
                    returnList.Add(flag);
                }

            }
        }
        return returnList;
        
    }
    private List<Result> getResultsFromList(int[] r)
    {
        List<Result> returnList = new List<Result>();
        for(int i = 0; i < r.Length; i++)
        {
            foreach(Result result in resultList)
            {
                if(result.getID() == r[i])
                {
                    returnList.Add(result);
                }
            }
        }
        return returnList;
    }
}
