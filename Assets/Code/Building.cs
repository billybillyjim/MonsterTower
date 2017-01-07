using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Building : MonoBehaviour {

    [SerializeField]
    private Sprite buildingSprite;
    [SerializeField]
    private int buildingType;
    private bool occupied;
    private int wVal;
    private int hVal;
    private float cost;

    public void setSprite(Sprite s)
    {
        buildingSprite = s;
        GetComponent<SpriteRenderer>().sprite = s;
    }
    public void setSprite(Sprite s, int i)
    {
        buildingSprite = s;
        GetComponent<SpriteRenderer>().sprite = s;
        buildingType = i;
    }
    public void setType(int i)
    {
        buildingType = i;
        setCost(i);
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
    private void setCost(int i)
    {
        if(i == 1)
        {
            cost = 100;
        }
        else if(i == 2)
        {
            cost = 200;
        }
    }

    public bool getIsOccupied()
    {
        return occupied;
    }
    public void setIsOccupied(bool b)
    {
        occupied = b;
    }
    public float getCost()
    {
        return cost;
    }

}
