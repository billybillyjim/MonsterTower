using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Tools : MonoBehaviour{

    public static Tool currentTool;
    public static Tool empty;

    [SerializeField]
    private Sprite selectedButtonSprite;
    [SerializeField]
    private Sprite buttonSprite;

    private List<Tool> toolList = new List<Tool>();

    private Transform currentButton;
    
    [SerializeField]
    private List<Transform> buttonList = new List<Transform>();

    [SerializeField]
    private Color humanColor;
    [SerializeField]
    private Color zombieColor;
    [SerializeField]
    private Color witchColor;
    [SerializeField]
    private Color demonColor;

    private int currentPanel;

    public void setPanelColor(int i)
    {
        if(i == 0)
        {
            gameObject.GetComponent<Image>().color = humanColor;
        }
        else if(i == 1)
        {
            gameObject.GetComponent<Image>().color = zombieColor;
        }
        else if (i == 2)
        {
            gameObject.GetComponent<Image>().color = witchColor;
        }
        else if (i == 3)
        {
            gameObject.GetComponent<Image>().color = demonColor;
        }
        else
        {
            gameObject.GetComponent<Image>().color = Color.white;
        }
        currentPanel = i;
        
        setTool(currentTool.getName());
    }
    public void setTool(string s)
    {

        if (currentPanel == 1 && 
            !s.Equals("Inspect"))
        {           
            s = s.Insert(0, "Zombie");           
        }
        if (s.Equals("Elevator") || s.Equals("Empty") || s.Equals("Inspect"))
        {
            TowerMap tower = GameObject.Find("Tower").GetComponent<TowerMap>();
            tower.showHideElevators(true);
        }
        else
        {
            TowerMap tower = GameObject.Find("Tower").GetComponent<TowerMap>();
            tower.showHideElevators(false);
        }

        currentTool = toolList.Find(x => x.getName().Equals(s));
        
    }
    void Start()
    {
        
        loadData();
        loadButtons();
        currentTool = toolList.Find(x => x.getName().Equals("Office"));
        //Sets the empty tool to the right one for access in the building class.
        empty = toolList[9];
        hideAllButtons();
        unlockButton("Office");
        unlockButton("Lobby");
        unlockButton("Bulldoze");
        unlockButton("Train");
        unlockButton("Elevator");
        unlockButton("Inspect");

    }
    private void loadData()
    {
        TextAsset textfile = Resources.Load<TextAsset>("BuildingData");
        string file = textfile.text;
        string[] lines = file.Split("\n"[0]);     
                
        for(int j = 0; j < lines.Length; j++)
        {
            string[] lineData = (lines[j].Trim()).Split(","[0]);        
               
            toolList.Add(new Tool(lineData[0], float.Parse(lineData[1]), int.Parse(lineData[2]), int.Parse(lineData[3]), int.Parse(lineData[4]), int.Parse(lineData[5]), j));
        }
    }
    private void loadButtons()
    {
        foreach (Transform t in gameObject.GetComponentInChildren<Transform>())
        {
            if(t.parent == this.transform)
            {
                buttonList.Add(t);
            }
        }
    }
    public void setButtonAsSelected(string s)
    {
        int i = toolList.IndexOf(toolList.Find(x => x.getName().Equals(s)));
        
        if (currentButton != null)
        {
            currentButton.GetComponent<Image>().sprite = buttonSprite;
        }
        currentButton = buttonList[i];
        currentButton.GetComponent<Image>().sprite = selectedButtonSprite;
    }
    public void setButtonAsSelected(Transform t)
    {
        if (currentButton != null)
        {
            currentButton.GetComponent<Image>().sprite = buttonSprite;
        }
        currentTool.setButton(t);
        currentButton = currentTool.getButton();
        currentButton.GetComponent<Image>().sprite = selectedButtonSprite;
    }
	public int getCurrentTool()
    {
        return toolList.IndexOf(currentTool);
    }
    public int getPopToMoveIn(int i)
    {
        return Random.Range(toolList[i].getMinPop(), toolList[i].getMaxPop());
    }
    public Tool getTool(int i)
    {
        return toolList[i];
    }
    private void hideAllButtons()
    {
        foreach(Transform t in buttonList)
        {
            t.gameObject.SetActive(false);
        }
    }
    private void unlockButton(int i)
    {
        buttonList[i].gameObject.SetActive(true);
    }
    private void unlockButton(string s)
    {
        //int i = toolList.IndexOf(toolList.Find(x => x.getName().Equals(s)));
        buttonList.Find(x => x.name.Contains(s)).gameObject.SetActive(true);
    }
}
