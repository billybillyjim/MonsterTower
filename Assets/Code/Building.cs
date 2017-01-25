using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Building : MonoBehaviour {

    [SerializeField]
    private Sprite buildingSprite;
    [SerializeField]
    private int buildingType;
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
        GameObject.Find("Tower").GetComponent<TowerMap>().build(wVal, hVal);
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            GameObject.Find("Tower").GetComponent<TowerMap>().build(wVal, hVal);
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
