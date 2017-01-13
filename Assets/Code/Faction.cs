using UnityEngine;
using System.Collections;

public class Faction : MonoBehaviour {

    private string title;
    private float prestige;
    private float relations;

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
    }
    public void setRelations(float r)
    {
        relations = r;
    }
    public void addRelations(float r)
    {
        relations += r;
    }
    public float getPrestige()
    {
        return prestige;
    }
}
