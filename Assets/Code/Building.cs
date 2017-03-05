using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Building : MonoBehaviour {

    [SerializeField]
    private Sprite buildingSprite;

    [SerializeField]
    private int buildingType;
    private int buildingVariation;
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
    private int wVal;
    private int hVal;
    private float cost;
    [SerializeField]
    private float desirability;
    [SerializeField]
    private int population;
    private UnityEngine.EventSystems.EventSystem _eventSystem;

    void Awake()
    {
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        buildingType = -1;
        
    }

    public void setSprite(Sprite s)
    {
        buildingSprite = s;
        GetComponent<SpriteRenderer>().sprite = s;
    }
    public void setSprite(Sprite s, int i)
    {
        buildingSprite = s;
        GetComponent<SpriteRenderer>().sprite = s;
        setType(i);
        width = Tools.toolWidth;
        height = Tools.toolHeight;
        rentPay = 100;
        utilityPay = i * 5;
        
    }
    public void setColor(Color c)
    {
        GetComponent<SpriteRenderer>().color = c;
    }
    public void setType(int i)
    {
        buildingType = i;
    }
    public void setDesirability(float f)
    {
        desirability = f;
    }

    public void Init(int w, int h)
    {
        wVal = w;
        hVal = h;

    }
    void OnMouseUp()
    {
        if (_eventSystem.IsPointerOverGameObject())
        {
            // we're over a UI element... peace out
            return;
        }
        if(Tools.currentTool != -1)
        {
            GameObject.Find("Tower").GetComponent<TowerMap>().build(wVal, hVal);
        }
        else
        {
            Debug.Log(population);
        }
        
    }
    void OnMouseOver()
    {
       
        if (_eventSystem.IsPointerOverGameObject())
        {
            // we're over a UI element... peace out
            return;
        }
        if(Tools.currentTool != -1)
        {
            TowerMap tower = GameObject.Find("Tower").GetComponent<TowerMap>();
            if (Input.GetMouseButton(0))
            {
                tower.build(wVal, hVal);
            }
            if ((buildingType == -1 || buildingType == 3) && !isOccupied)
            {
                GetComponent<SpriteRenderer>().sprite = GameObject.Find("Tower").GetComponent<TowerMap>().getBuildingSpritesList()[Tools.currentTool][0];
                GetComponent<SpriteRenderer>().color = tower.getCurrentMapColor(this);
                GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
        }
             
    }
    void OnMouseExit()
    {
        TowerMap tower = GameObject.Find("Tower").GetComponent<TowerMap>();
        GetComponent<SpriteRenderer>().sprite = buildingSprite;
        if (buildingType != -1)
        {
            
            GetComponent<SpriteRenderer>().color = tower.getCurrentMapColor(this);
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = null;

        }
       

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
        return desirability;
    }
}
