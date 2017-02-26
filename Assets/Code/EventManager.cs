using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

    private List<Condition> conditionList = new List<Condition>();
    private List<Flag> flagList = new List<Flag>();
    private List<Event> eventList = new List<Event>();
    private List<Event> potentialEventList = new List<Event>();
    private List<Result> resultList = new List<Result>();
    private int currentConditionCheck = 0;
    [SerializeField]
    private GameObject eventPanel;
    [SerializeField]
    private GameObject eventChoice;
    [SerializeField]
    private Transform canvas;
    [SerializeField]
    private Vector3 eventPopUpPos;
    private int totalWeight;

    public int addPotentialEventInterval;
    public int updateFlagInterval;
    public int checkPotentialEventInterval;

    public GameRun game;
    public TowerMap tower;

    void Start()
    {
        loadConditions();
        loadFlags();
        loadResults();
        loadEvents();
        updateConditions();      
        addPotentialEvents();
        InvokeRepeating("addPotentialEvents", 5, addPotentialEventInterval);
        InvokeRepeating("updateFlags", 4, updateFlagInterval);
        InvokeRepeating("checkPotentialEvents", 6, checkPotentialEventInterval);
    }
    void Update()
    {
        updateConditions();
        
    }
    private void setRepeatings() {
       
        InvokeRepeating("addPotentialEvents", 0, addPotentialEventInterval);
        InvokeRepeating("updateFlags", 0, updateFlagInterval);
        InvokeRepeating("checkPotentialEvents", 0, checkPotentialEventInterval);
    }
    public void setNormalIntervals()
    {
        addPotentialEventInterval = 5;
        updateFlagInterval = 5;
        checkPotentialEventInterval = 5;
        CancelInvoke();
        setRepeatings();
    }
    public void setIntervals(int i)
    {
        addPotentialEventInterval = i;
        updateFlagInterval = i;
        checkPotentialEventInterval = i;
        CancelInvoke();
        setRepeatings();
    }
    public void setIntervalString(Text t)
    {
        string s = t.text;
        int i;
        int.TryParse(s, out i);
        if(i > 0)
        {
            setIntervals(i);
        }
    }
    private void runEvent(Event e)
    {
        if(e.getTitle() == "")
        {
            return;
        }
        //Debug.Log("Name:" + e.getTitle() + ", Requirement: " + e.getFlags()[0].getName() + " is " + e.getFlags()[0].getValue());
        GameObject t = (GameObject)Instantiate(eventPanel, eventPanel.transform.position, Quaternion.identity);
        GameObject o = t.gameObject;
        o.transform.SetParent(canvas);
        o.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        o.GetComponent<RectTransform>().position = eventPopUpPos;

        string title = e.getTitle();
        string text = e.getText();
        Text[] eventTexts = o.GetComponentsInChildren<Text>();
        eventTexts[0].text = title;
        eventTexts[1].text = text;
        Transform[] optionsList = o.GetComponentsInChildren<Transform>();
       
        foreach(Result r in e.getResults())
        {
            GameObject re = (GameObject)Instantiate(eventChoice, eventChoice.transform.position, Quaternion.identity);
            GameObject res = re.gameObject;
            res.transform.SetParent(optionsList[5]);
            res.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            Button button = res.GetComponent<Button>();
            button.onClick.AddListener( () => { doResult(r.getVal(), r.getFloat()); });
            button.onClick.AddListener( () => { o.SetActive(false); });
            res.GetComponentInChildren<Text>().text = r.getText();
        }
        e.setFired(true);
        potentialEventList.Remove(e);
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
        if(c.getID() == 0)
        {
            return;
        }
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
    //TO DO: Make not wasteful

    private void updateConditionValue(Condition c, int t)
    {
        int i = c.getVal();
        Debug.Log(c.getName() + c.getInt() + ", Value to Check:" + i + ", Value of Stat:" + t);
        switch (i)
        {
            case -1:
                if(t > tower.getPopulation())
                {
                    c.setValidity(true);
                }
                else
                {
                    c.setValidity(false);
                }
                break;
            case 0:
                if (t < game.getCurrentYear())
                {
                    c.setValidity(true);
                }
                else
                {
                    c.setValidity(false);
                }
                break;
            case 1:
                Debug.Log("T is " + t + ", and the tower population is " + tower.getPopulation());
                if(t < tower.getPopulation())
                {
                    
                    c.setValidity(true);
                }
                else
                {
                    c.setValidity(false);
                }
                break;
            case 2:
                break;
            case 3:
                if(t < tower.getHumanPop())
                {
                    c.setValidity(true);
                }
                else
                {
                    c.setValidity(false);
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

    private void updateFlags()
    {
        foreach(Flag f in flagList)
        {
            foreach(Condition c in f.getCondition())
            {
                if(c.getValidity() == false)
                {
                    f.setValue(false);
                    break;
                }
                else
                {
                    f.setValue(true);
                }
            }

        }
    }

    

    private void addPotentialEvents()
    {
        foreach(Event e in eventList)
        {
            if(e.getFired() == false)
            {
                bool truth = false;

                foreach (Flag f in e.getFlags())
                {
                   
                    if (flagList[f.getID()].getValue() == false)
                    {

                        break;
                    }
                    else
                    {
                        truth = true;
                    }
                }
                if (truth && potentialEventList.Contains(e) == false && e.getTitle() != "")
                {
                    potentialEventList.Add(e);
                }
            }                      
        }        
    }

    public void doResult(int val, int amount)
    {
        switch (val)
        {
            case 0:
                game.setCurrentYear(game.getCurrentYear() + amount);
                break;
            case 1:
                tower.setTotalPopulation(tower.getPopulation() + amount);
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
    public void doResult(int val, float amount)
    {
        switch (val)
        {
            case 6:
                game.setMoney(game.getMoney() + amount);
                break;
        }
    }

    private void checkPotentialEvents()
    {
        Debug.Log("Checking " + potentialEventList.Count + " Events");
        int eventToPop = Random.Range(0, totalWeight);
        int iterateWeight = 0;
        foreach(Event e in potentialEventList)
        {
            if(e.getWeight() == 100)
            {
                runEvent(e);
                break;
            }
            else
            {
                iterateWeight += e.getWeight();
                if(iterateWeight >= eventToPop)
                {
                    runEvent(e);
                    break;
                }
            }
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
            for(int i = 0; i < text.Length; i++)
            {
                if(text[i] == '_')
                {
                    char[] c = text.ToCharArray();
                    c[i] = ',';
                    text = c.ToString();
                }
            }
                    
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
            int weight = 0;
            if (lineData[5] != "")
            {
                weight = int.Parse(lineData[5]);
            }
           
            e.Init(getFlagsFromList(flagIDs), getResultsFromList(resultIDs), id, title, text, weight);
            eventList.Add(e);
            Debug.Log("Name:" + e.getTitle() + ", Requirement: " + e.getFlags()[0].getName() + " is " + e.getFlags()[0].getValue());
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
                if(result.getID() == r[i] && result.getID() != 0)
                {
                    returnList.Add(result);
                    
                }
            }
        }
        //Debug.Log(returnList.Count);
        return returnList;
    }
    public List<Event> getEventList()
    {
        return eventList;
    }
}
