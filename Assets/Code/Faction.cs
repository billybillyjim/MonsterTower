using UnityEngine;
using System.Collections;

public class Faction{

    private string title;
    private float prestige;
    private float relations;
    private float neutral;
    
    public Faction(string t, float p, float r, float n)
    {
        title = t;
        prestige = p;
        relations = r;
        neutral = n;
    }

	public void setTitle(string s)
    {
        title = s;
    }
    public void setPrestige(float p)
    {
        prestige = p;
    }
    public void addPrestige(float p)
    {
        prestige += p;
        if(prestige >= 100)
        {
            prestige = 100;
        }
        if(prestige <= -100)
        {
            prestige = -100;
        }
    }
    public void setRelationship(float r)
    {
        relations = r;
    }
    public void addRelations(float r)
    {
        relations += r;
        if(relations >= 200)
        {
            relations = 200;
        }
        if(relations <= -200)
        {
            relations = -200;
        }
    }
    public float getPrestige()
    {
        return prestige;
    }
    public float getNeutral()
    {
        return neutral;
    }
    public float getRelationship()
    {
        return relations;
    }
}
