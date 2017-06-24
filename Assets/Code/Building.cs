using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Building : MonoBehaviour {

    [SerializeField]
    private Sprite buildingSprite;
    public Sprite selectedSprite;
    [SerializeField]
    private int buildingType;
    private int buildingVariation;
    private string buildingTypeString;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private int floor;
    private float rentPay;
    private float utilityPay;
    [SerializeField]
    private bool isOccupied;
    private bool isDirt;
    private bool isInspecting;
    [SerializeField]
    private int wVal;
    [SerializeField]
    private int hVal;
    private float cost;
    [SerializeField]
    private float desirability;
    [SerializeField]
    private int population;
    private UnityEngine.EventSystems.EventSystem _eventSystem;
    TowerMap tower;
    private List<DesiribilityModifier> desireModifiers = new List<DesiribilityModifier>();

    void Awake()
    {
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();       
        buildingType = -1;
        
    }
    public void addDesiribilityModifier(DesiribilityModifier modifier)
    {
        desireModifiers.Add(modifier);
    }
    private void updateDesiribility()
    {
        foreach(DesiribilityModifier d in desireModifiers)
        {
            desirability += d.getValue();
        }
    }
    public void setSprite(Sprite s)
    {
        buildingSprite = s;
        GetComponent<SpriteRenderer>().sprite = s;
    }
    
    public void setSprite(Sprite s, Tool t)
    {
        int i = t.getPosition();
        buildingSprite = s;
        GetComponent<SpriteRenderer>().sprite = s;
        setType(i);
        width = Tools.currentTool.getWidth();
        height = Tools.currentTool.getHeight();
        rentPay = 100;
        utilityPay = i * 5;
        setTypeString(t.getName());      
    }
    
    public void makeGroundSprites(Sprite s, int i)
    {
        buildingSprite = s;
        GetComponent<SpriteRenderer>().sprite = s;
        setType(i);
        width = 1;
        height = 1;
        rentPay = 100;
        utilityPay = i * 5;
        setTypeString("Ground");
    }
    public void setSpriteToEmpty(Sprite s)
    {
        Tool t = Tools.empty;
        int i = t.getPosition();
        buildingSprite = s;
        GetComponent<SpriteRenderer>().sprite = s;
        setType(i);
        width = Tools.currentTool.getWidth();
        height = Tools.currentTool.getHeight();
        rentPay = 100;
        utilityPay = i * 5;
        setTypeString(t.getName());
    }
    public void setTypeString(string s)
    {
        buildingTypeString = s;
    }
    public void setColor(Color c)
    {
        GetComponent<SpriteRenderer>().color = c;
    }
    public void setType(int i)
    {
        buildingType = i;
    }

    public void Init(int w, int h)
    {
        wVal = w;
        hVal = h;
        tower = GameObject.Find("Tower").GetComponent<TowerMap>();
    }
    void OnMouseUp()
    {
        //over a UI element
        if (_eventSystem.IsPointerOverGameObject())
        {          
            return;
        }
        //A tool is selected
        if (Tools.currentTool != null && Tools.currentTool.getName() != "Inspect")
        {
            if (Tools.currentTool.getName().Equals("Elevator"))
            {
                tower.buildElevator();
            }
            else
            {
                tower.build(wVal, hVal);
            }          
        }          
    }

    void OnMouseEnter()
    {
       //Checks to make sure the mouse isn't also over a UI element.
        if (_eventSystem.IsPointerOverGameObject())
        {
            return;
        }
        //If a tool is selected
        if (Tools.currentTool != null && Tools.currentTool.getName() != "Inspect" && Tools.currentTool.getName() != "Elevator")
        {
            if (Tools.currentTool.getName().Equals("Empty") && buildingType != 3 && buildingType != 9)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                
            }
            TowerMap tower = GameObject.Find("Tower").GetComponent<TowerMap>();
            //If the building type isnt empty or not yet set
            if ((buildingType == -1 || buildingType == 3 || buildingType == 9) && !isOccupied)
            {
                GetComponent<SpriteRenderer>().sprite = GameObject.Find("Tower").GetComponent<TowerMap>().getBuildingData().Find(x => x.getTypeName().Equals(Tools.currentTool.getName())).getEmptySprite();
                GetComponent<SpriteRenderer>().color = tower.getCurrentMapColor(this);
                GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
        }
        if (Input.GetMouseButton(0))
        {
            isInspecting = false;
        }
             
    }
    //For drag and release building
    void OnMouseOver()
    {
        //Checks to make sure the mouse isn't also over a UI element.
        if (_eventSystem.IsPointerOverGameObject())
        {
            return;
        }
        if(Tools.currentTool != null)
        {
            if (!Tools.currentTool.getName().Equals("Inspect"))
            {

                if (Input.GetMouseButton(0))
                {
                    if(buildingType == 2)
                    {
                        return;
                    }
                    if ((buildingType == -1 || buildingType == 3 || buildingType == 9) && !isOccupied)
                    {
                        tower.build(wVal, hVal);
                        GetComponent<SpriteRenderer>().sortingOrder = 10;
                    }
                    else if (!(buildingType == -1 || buildingType == 3 || buildingType == 9) && Tools.currentTool.getName().Equals("Empty"))
                    {
                        tower.bulldoze(wVal, hVal);
                        GetComponent<SpriteRenderer>().sortingOrder = 10;
                    }
                }
            }
            else if (Tools.currentTool.getName().Equals("Inspect"))
            {
                if (Input.GetMouseButtonUp(0) && isInspecting)
                {
                    tower.addToInspectList(this);                   
                }
            }
        }
        
    }
    //Removes the cursor sprite
    void OnMouseExit()
    {
        TowerMap tower = GameObject.Find("Tower").GetComponent<TowerMap>();
        GetComponent<SpriteRenderer>().sprite = buildingSprite;
        if (!(buildingType == -1 || buildingType == 3 || buildingType == 9))
        {            
            GetComponent<SpriteRenderer>().color = tower.getCurrentMapColor(this);
            GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else if(buildingType == 3 || buildingType == 9)
        {
            GetComponent<SpriteRenderer>().color = tower.getCurrentMapColor(this);
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = null;
        }
        isInspecting = true;      
    }

    public void moveIn(int i)
    {
        population = i;
    }
    public void moveOut()
    {
        population = 0;
    }

    public bool getIsOccupied()
    {
        return isOccupied;
    }
    public void setIsOccupied(bool b)
    {
        isOccupied = b;
    }
    public int getBuildingType()
    {
        return buildingType;
    }
    public void getCoordinates(out int x, out int y)
    {
        x = wVal;
        y = hVal;
    }
    public int getX()
    {
        return wVal;

    }
    public int getY()
    {
        return hVal;
    }
    public int getWidth()
    {
        return width;
    }
    public void setFloor(int i)
    {
        floor = i;
    }
    public int getFloor()
    {
        return floor;
    }
    public void setRent(float r)
    {
        rentPay = r;
    }
    public float getRent()
    {
        return rentPay;
    }
    public void setDirt(bool b)
    {
        isDirt = b;
    }
    public bool getDirt()
    {
        return isDirt;
    }
    public float getUtilityPay()
    {
        return utilityPay;
    }
    public int getHeight()
    {
        return height;
    }
    public int getPopulation()
    {
        return population;
    }
    public float getDesirability()
    {
        float f = 0;
        foreach(DesiribilityModifier d in desireModifiers)
        {
            f += d.getValue();
        }
        return f;
    }
    public string getBuildingTypeString()
    {
        return buildingTypeString;
    }
    public Sprite getBuildingSprite()
    {
        return buildingSprite;
    }

}
