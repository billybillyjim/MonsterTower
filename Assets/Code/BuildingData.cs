using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingData {

    private List<Sprite> emptySprites = new List<Sprite>();
    private List<Sprite> fullSprites = new List<Sprite>();
    public string typeName;

    public BuildingData()
    {

    }
    public BuildingData(string n)
    {
        typeName = n;
    }
    public void addEmpty(Sprite s)
    {
        emptySprites.Add(s);
    }
    public void addFull(Sprite s)
    {
        fullSprites.Add(s);
    }

    public string getTypeName()
    {
        return typeName;
    }
    public Sprite getEmptySprite()
    {
        if(emptySprites.Count != 0)
        {
            return emptySprites[Random.Range(0, emptySprites.Count)];
        }
        return getFullSprite();
    }
    public Sprite getFullSprite()
    {
        if(fullSprites.Count != 0)
        {
            return fullSprites[Random.Range(0, fullSprites.Count)];
        }
        return getEmptySprite();
    }
    public Sprite getFullSpriteByName(string s)
    {
        foreach (Sprite sprite in fullSprites)
        {
            if (sprite.name.Equals(s))
            {
                return sprite;
            }
        }
        return null;
    }
}
