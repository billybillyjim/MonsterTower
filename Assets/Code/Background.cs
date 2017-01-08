using UnityEngine;
using System.Collections.Generic;

public class Background : MonoBehaviour {

    private int backgroundWidth = 120;
    private int backgroundHeight = 420;
    private Transform[,] bgUnits;
    [SerializeField]
    private GameObject bgUnit;

    void Start()
    {
        bgUnits = new Transform[backgroundWidth, backgroundHeight];
        makeBackground();
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
}
