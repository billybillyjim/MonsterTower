using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

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
    private GameObject eventToolTip;
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
        updateFlags();
        addPotentialEvents();
        InvokeRepeating("addPotentialEvents", 5, addPotentialEventInterval);
        InvokeRepeating("updateFlags", 4, updateFlagInterval);
        InvokeRepeating("checkPotentialEvents", 6, checkPotentialEventInterval);
        foreach(Result r in resultList)
        {
            Debug.Log(r.getName());
        }
    }
    void Update()
    {
        updateFlags();

        
    }
    private void setRepeatings() {
       
        InvokeRepeating("addPotentialEvents", 0, addPotentialEventInterval);
        InvokeRepeating("updateFlags", 0, updateFlagInterval);
        InvokeRepeating("checkPotentialEvents", 0, checkPotentialEventInterval);
    }

    public void updateEvents()
    {
        addPotentialEvents();
        updateFlags();
        checkPotentialEvents();
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

            Text toolText = res.GetComponentsInChildren<Text>()[1];
            toolText.text = r.getValue();                     
            button.onClick.AddListener(() => { doResult(r.getValue()); });           
            button.onClick.AddListener( () => { o.SetActive(false); });
            button.onClick.AddListener(() => { Destroy(o); });
            
            Debug.Log(r.getID() + ", " + r.getValue() + ", " + r.getName()); 
            res.GetComponentInChildren<Text>().text = r.getName();
        }
        e.setFired(true);
        potentialEventList.Remove(e);
    }

    private void updateFlags()
    {
        foreach(Flag f in flagList)
        {
            foreach(Condition c in f.getConditions())
            {
                if(checkConditionValue(c) == false)
                {
                    f.setTruthValue(false);
                    break;
                }
                else
                {
                    f.setTruthValue(true);
                }
            }

        }
    }
    private bool checkConditionValue(Condition c)
    {
        //data[0] contains a string for the type of value to check, data[1] contains the number.
        string[] data = c.getValue().Split(':');
        //Debug.Log(data[0] + ", " + data[1]);
        if(data[0].Equals("Population"))
        {
            if(int.Parse(data[1]) < tower.getPopulation())
            {
                return true;
            }
        }       
        else if(data[0].Equals("Highest Floor"))
        {
            if (int.Parse(data[1]) < tower.getHighestFloor())
            {
                return true;
            }
        }
        else if(data[0].Equals("Human Population"))
        {
            if(int.Parse(data[1]) < tower.getHumanPop())
            {
                return true;
            }
        }
        else if (data[0].Equals("Result"))
        {

        }
        return false;
    }

    public void setToolTip(Result r)
    {
        GameObject tip;
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

                    if (flagList[f.getID()].getTruthValue() == false)
                    {
                        truth = false;
                        
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

    public void doResult(string result)
    {
        Debug.Log(result);
        string[] data = result.Split(':');
        if (data[0].Equals("Money"))
        {
            game.setMoney(game.getMoney() + int.Parse(data[1]));
        }
    }

    private void checkPotentialEvents()
    {
        //Debug.Log("Checking " + potentialEventList.Count + " Events");
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

            c.Init(id, s);           
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
            r.Init(id, txt, s);
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

            text = text.Replace('_', ',');
                 
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
                Debug.Log(results[i]);
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
           // Debug.Log("Name:" + e.getTitle() + ", Requirement: " + e.getFlags()[0].getName() + " is " + e.getFlags()[0].getTruthValue());
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
