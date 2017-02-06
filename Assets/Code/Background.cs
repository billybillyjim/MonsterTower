using UnityEngine;
using System.Collections.Generic;

public class Background : MonoBehaviour {

    private int backgroundWidth = 55;
    private int backgroundHeight = 150;
    private Transform[,] bgUnits;
    [SerializeField]
    private GameObject bgUnit;
    [SerializeField]
    private GameObject bgBuilding;
    [SerializeField]
    private Transform cloud;
    [SerializeField]
    private Sprite[] towerSprites;
    void Start()
    {
        towerSprites = Resources.LoadAll<Sprite>("board/skyline");
        bgUnits = new Transform[backgroundWidth, backgroundHeight];
       // makeBackground();
        //makeBackgroundCity();
       // InvokeRepeating("spawnCloud", 5, 1);
    }
    private void makeBackground()
    {
        for(int i = 0; i < backgroundWidth; i++)
        {
            for(int j = 0; j < backgroundHeight; j++)
            {               
                GameObject t = (GameObject)Instantiate(bgUnit, new Vector3(i, j), Quaternion.identity) as GameObject;
                bgUnits[i, j] = t.transform;
                t.GetComponent<SpriteRenderer>().sprite = Resources.Load("board/sky", typeof(Sprite)) as Sprite;
            }
        }
    }
    private void makeBackgroundCity()
    {
        for(int i = 0; i < backgroundWidth; i++)
        {
            if(Random.Range(0,5) > 3)
            {
                buildTower(i);
            }
        }
    }

    private void buildTower(int i)
    {
        int r = Random.Range(0, 10);
        float baseHeight = 0;
        if(r < 8)
        {
            for(int j = 0; j < r; j++)
            {
                addSegment(i, baseHeight, r);
                baseHeight += .1239f;
            }
            addSegment(i, baseHeight, 15);          
        }
        else if(r > 7)
        {
            for (int j = 0; j < r; j++)
            {
                addSegment(i, baseHeight, r);
                baseHeight += .1239f;
            }
            addSegment(i, baseHeight, 14);
        }
    }

    private GameObject addSegment(int i, float j, int r)
    {
        GameObject g = (GameObject)Instantiate(bgBuilding, new Vector3(i, j + 1), Quaternion.identity);
        if(r == 15)
        {
            g.GetComponent<SpriteRenderer>().sprite = towerSprites[0];
        }
        else if (r == 14)
        {
            g.GetComponent<SpriteRenderer>().sprite = towerSprites[1];
        }
        else if(r < 2)
        {
            g.GetComponent<SpriteRenderer>().sprite = towerSprites[7];         
        }
        else if(r < 4)
        {
            g.GetComponent<SpriteRenderer>().sprite = towerSprites[12];
        }
        else if(r > 9)
        {
            g.GetComponent<SpriteRenderer>().sprite = towerSprites[4];
        }
        else if(r > 7)
        {
            g.GetComponent<SpriteRenderer>().sprite = towerSprites[8];
        }
        g.GetComponent<SpriteRenderer>().sortingOrder = 5;
        return g;

    }

    private void spawnCloud()
    {
        float r = Random.Range(0, backgroundHeight);
        if(r % 2 == 0)
        {
            Instantiate(cloud, new Vector3(backgroundWidth + 2, r), Quaternion.identity);

        }
    }
}
